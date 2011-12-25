using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBDerp.TestDrivers;
using NJasmine;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SizSelCsZzz;

namespace FBDerp
{
    public class User_can_associate_their_facebook_account_on_registration : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var expectedUsername = Guid.NewGuid().ToString();

            var facebook = arrange(() => new FacebookClient());

            given("a logged in facebook user", delegate()
            {
                var userFullname = "Some Fbuser";
                var user = arrange(() => facebook.CreateTestUser(userFullname));

                var browser = arrange(() =>
                {

                    //  Firefox fails as it puts up a warning dialog
                    //  when posting to non-HTTPS from the HTTPS iframe
                    //var profile = new FirefoxProfile();
                    //profile.SetPreference("security.warn_entering_weak", false);
                    //profile.SetPreference("security.warn_entering_weak.show_once", false);
                    //profile.SetPreference("security.warn_submit_insecure", false);

                    //return new FirefoxDriver(profile);

                    return new ChromeDriver();
                });

                arrange(delegate()
                {
                    browser.Navigate().GoToUrl("http://www.facebook.com/");
                    browser.FindElement(BySizzle.CssSelector("input[name=email]")).SendKeys(user.email);
                    browser.FindElement(BySizzle.CssSelector("input[name=pass]")).SendKeys(user.password);
                    browser.FindElement(BySizzle.CssSelector(".menu_login_container input[type=Submit]")).Click();
                    browser.WaitForElement(BySizzle.CssSelector("input[value='Log Out']"));
                });

                given("the user registers on the site", delegate()
                {
                    var site = this.ArrangeServer();

                    arrange(delegate()
                    {
                        browser.Navigate().GoToUrl(site.UrlFor("/Account/Register"));

                        var iframe =
                            browser.WaitForElementEx(
                                BySizzle.CssSelector("iframe[src^=\"https://www.facebook.com/plugins/registration.php\"]"),
                                Constants.MSLongWait);

                        browser.SwitchTo().Frame(iframe);

                        browser.WaitForElement(BySizzle.CssSelector("input[name=nickname]")).SendKeys(expectedUsername);

                        var windowContext = new WhichWindowContext(browser);

                        //  multiple submit buttons existed, we click the one that is visible
                        var findElement =
                            browser.FindElements(BySizzle.CssSelector("input[value=Register]")).First(e => e.Displayed);
                        findElement.Click();

                        browser.SwitchTo().Window(windowContext.GetNewWindowName());
                        browser.FindElement(BySizzle.CssSelector("input[value=Continue]")).Click();

                        System.Threading.Thread.Sleep(5*1000);

                        
                        browser.SwitchTo().Window(windowContext.GetOriginalWindowName());
                    });

                    it("shows the user has logged in", delegate()
                    {
                        expectEventually(() => browser.ContainsText("Welcome " + expectedUsername), Constants.MSLongWait);
                    });
                });
            });
        }
    }
}
