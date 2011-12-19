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
                        var iframe =
                            browser.WaitForElement(
                                BySizzle.CssSelector("iframe[src^=\"https://www.facebook.com/plugins/comments.php\"]"));
                        browser.SwitchTo().Frame(iframe);

                        browser.FindElement(BySizzle.CssSelector("textarea")).SendKeys("First comment");
                        browser.FindElement(BySizzle.CssSelector("a[data-label^='Comment using']")).Click();
                        browser.FindElement(BySizzle.CssSelector("a[onclick*=setProvider]:contains('Facebook')")).Click();

                        foreach(string handle in browser.WindowHandles)
                        {
                            Console.WriteLine("window: " + handle);
                        }
                        ;
                    });
                });
            });
        }
    }
}
