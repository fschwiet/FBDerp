using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJasmine;
using OpenQA.Selenium.Firefox;
using SizSelCsZzz;

namespace FBDerp
{
    public class User_can_associate_their_facebook_account_on_registration : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var facebook = arrange(() => new FacebookClient());

            given("a logged in facebook user", delegate()
            {
                var userFullname = "Some Fbuser";
                var user = arrange(() => facebook.CreateTestUser(userFullname));

                var browser = arrange(() => new FirefoxDriver());

                arrange(delegate()
                {
                    browser.Navigate().GoToUrl("http://www.facebook.com/");
                    browser.FindElement(BySizzle.CssSelector("input[name=email]")).SendKeys(user.email);
                    browser.FindElement(BySizzle.CssSelector("input[name=pass]")).SendKeys(user.password);
                    browser.FindElement(BySizzle.CssSelector(".menu_login_container input[type=Submit]")).Click();
                    browser.WaitForElement(BySizzle.CssSelector("input[value='Log Out']"));


                });

                it("does something", delegate()
                {

                });
            });
        }
    }
}
