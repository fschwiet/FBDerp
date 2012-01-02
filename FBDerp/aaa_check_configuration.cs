using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBDerp.TestDrivers;
using NJasmine;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using SizSelCsZzz;

namespace FBDerp
{
    public class aaa_check_configuration : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var site = beforeAll(() => IISExpressDriver.StartServer());

            beforeAll(delegate() {
                var hostName = new Uri(site.UrlFor("")).Host;
                
                expect(() => hostName.Contains("localhost"));

                PSHostsFile.HostsFile.Set(hostName, "127.0.0.1");
            });

            describe("the web application's configuration", delegate()
            {
                var browser = beforeAll(() => new FirefoxDriver());

                it("should have a functioning database connection string", delegate() {

                    browser.Navigate().GoToUrl(site.UrlFor("/Check"));

                    var errorElement = browser.FindElement(BySizzle.CssSelector("#error"));

                    string actual = errorElement.Text.Trim();

                    Assert.That(string.IsNullOrEmpty(actual), "SQL configuration error detected.  Do you have SQL express installed and setup?  If not, you can get it here: http://www.microsoft.com/download/en/details.aspx?id=26729 \nServer reported error:\n\n" + actual);
                });
            });
        }
    };
}
