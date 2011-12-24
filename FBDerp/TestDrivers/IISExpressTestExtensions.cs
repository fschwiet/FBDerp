using System;
using System.IO;
using NJasmine;

namespace FBDerp.TestDrivers
{
    public static class IISExpressTestExtensions
    {
        public static IISExpressDriver ArrangeServer(this GivenWhenThenFixture fixture)
        {
            var sitePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Properties.Settings.Default.SimpleServerPath);

            fixture.beforeAll(delegate()
            {
                var appDataPath = Path.Combine(sitePath, "App_Data");
                foreach(string file in Directory.GetFiles(appDataPath, "*"))
                {
                    File.Delete(file);

                    if (File.Exists(file))
                        throw new Exception("unable to delete file " + file);
                }
            });

            var serverUnderTest = fixture.beforeAll(() => new IISExpressDriver());

            sitePath = Path.GetFullPath(sitePath);

            fixture.beforeAll(() => serverUnderTest.Start(sitePath, 8084));

            return serverUnderTest;
        }
    }
}
