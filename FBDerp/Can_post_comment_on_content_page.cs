using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FBDerp.TestDrivers;
using NJasmine;
using OpenQA.Selenium.Firefox;
using SizSelCsZzz;

namespace FBDerp
{
    public class Can_post_comment_on_content_page : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var msLongWait = 10*10000;

            var facebookClient = arrange(() => FacebookClient.Open());
            string userFullname = "User Name";

            given("a facebook user", delegate()
            {
                var user = arrange(() =>
                {
                    return facebookClient.ApiTestUser(userFullname);
                });

                given("the user visits a content page", delegate()
                {
                    var server = this.ArrangeServer();

                    var browser = arrange(() => new FirefoxDriver());

                    arrange(() => browser.Navigate().GoToUrl(server.UrlFor("/View/123")));

                    expect(() => browser.ContainsText("Welcome to 123"));

                    then("the user can post a comment", delegate()
                    {
                        var comment = Guid.NewGuid().ToString();

                        var iframe =
                            browser.WaitForElementEx(
                                BySizzle.CssSelector("iframe[src^=\"https://www.facebook.com/plugins/comments.php\"]"),
                                msLongWait);
                        
                        browser.SwitchTo().Frame(iframe);

                        browser.FindElement(BySizzle.CssSelector("textarea")).SendKeys(comment);
                        browser.FindElement(BySizzle.CssSelector("a[data-label^='Comment using']")).Click();

                        var windowContext = new WhichWindowContext(browser);
                        
                        browser.FindElement(BySizzle.CssSelector("a[onclick*=setProvider]:contains('Facebook')")).Click();

                        browser.SwitchTo().Window(windowContext.GetNewWindowName());

                        browser.FindElement(BySizzle.CssSelector("input[id=email]")).SendKeys(user.email);
                        browser.FindElement(BySizzle.CssSelector("input[id=pass]")).SendKeys(user.password);
                        browser.FindElement(BySizzle.CssSelector("input[name=login]")).Click();

                        browser.SwitchTo().Window(windowContext.GetOriginalWindowName());
                        browser.SwitchTo().Frame(iframe);


                        var waitForElement = browser.WaitForElement(BySizzle.CssSelector("input[value=Comment]"));
                        expectEventually(() => waitForElement.Displayed, msLongWait);
                        waitForElement.Click();

                        browser.FindElements(BySizzle.CssSelector("div.postText:contains('" + comment + "')"));
                    });
                });
            });
        }
    }
}
