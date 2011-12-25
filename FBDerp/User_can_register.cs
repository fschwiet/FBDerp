using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBDerp.Common;
using FBDerp.TestDrivers;
using NJasmine;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SizSelCsZzz;

namespace FBDerp
{
    public class User_can_register : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var site = this.ArrangeServer();

            var siteUsername = Guid.NewGuid().ToString();

            var facebook = arrange(() => new FacebookClient());

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

            given("a logged in facebook user", delegate()
            {
                var userFullname = "Some Fbuser";
                var user = arrange(() => facebook.CreateTestUser(userFullname));

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
                    arrange(delegate()
                    {
                        browser.Navigate().GoToUrl(site.UrlFor("/Account/Register"));

                        browser_should_interact_with_registration_iframe(browser);

                        should_type_registration_fields(browser, new
                        {
                            nickname = siteUsername
                        });

                        var windowContext = new WhichWindowContext(browser);

                        click_registration_button(browser);

                        browser.SwitchTo().Window(windowContext.GetNewWindowName());
                        browser.FindElement(BySizzle.CssSelector("input[value=Continue]")).Click();
                        
                        browser.SwitchTo().Window(windowContext.GetOriginalWindowName());
                    });

                    it("shows the user has logged in", delegate()
                    {
                        expectEventually(() => browser.ContainsText("Welcome " + siteUsername), Constants.MSLongWait);
                    });
                });
            });

            given("a non-facebook user registers on the site", delegate()
            {
                var email = siteUsername + "@somesite.com";
                var password = siteUsername.Substring(0, 10);

                arrange(delegate()
                {
                    browser.Navigate().GoToUrl(site.UrlFor("/Account/Register"));

                    browser_should_interact_with_registration_iframe(browser);

                    should_have_registration_fields(browser, "nickname", "email", "password", "password_confirmation");

                    should_type_registration_fields(browser, new
                        {
                            nickname = siteUsername,
                            email = email,
                            password = password,
                            password_confirmation = password
                        });

                    click_registration_button(browser);
                });

                it("shows the user has logged in", delegate()
                {
                    expectEventually(() => browser.ContainsText("Welcome " + siteUsername), Constants.MSLongWait);
                });
            });
        }

        private void click_registration_button(ChromeDriver browser)
        {
            //  multiple submit buttons existed, we click the one that is visible
            var findElement =
                browser.FindElements(BySizzle.CssSelector("input[value=Register]")).First(e => e.Displayed);
            findElement.Click();
        }

        private void should_type_registration_fields(IWebDriver browser, object values)
        {
            var registrationTextFields = browser.FindElements(GetRegistrationInputSelector()).Where(e => e.Displayed);

            var names = registrationTextFields.Select(e => e.GetAttribute("name")).ToArray();
            Console.WriteLine("have fields: " + string.Join(",", names))
            ;

            foreach (var value in values.GetPropertyValues())
            {
                registrationTextFields.Single(e => e.GetAttribute("name") == value.Key).SendKeys(value.Value);
            }
        }

        private void should_have_registration_fields(IWebDriver browser, params string[] options)
        {
            browser.WaitForElement(GetRegistrationInputSelector());

            var allTheTextFields = browser.FindElements(GetRegistrationInputSelector()).Select(e => e.GetAttribute("name")).ToArray();

            Assert.That(allTheTextFields, Is.EquivalentTo(options));
        }

        private By GetRegistrationInputSelector()
        {
            return BySizzle.CssSelector(".fbRegistrationTextField,.fbRegistrationPasswordField");
        }

        private void browser_should_interact_with_registration_iframe(ChromeDriver browser)
        {
            var iframe =
                browser.WaitForElementEx(
                    BySizzle.CssSelector("iframe[src^=\"https://www.facebook.com/plugins/registration.php\"]"),
                    Constants.MSLongWait);

            browser.SwitchTo().Frame(iframe);
        }
    }
}
