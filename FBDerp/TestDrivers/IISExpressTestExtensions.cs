using System;
using System.IO;
using NJasmine;

namespace FBDerp.TestDrivers
{
    public static class IISExpressTestExtensions
    {
        public static IISExpressDriver ArrangeServer(this GivenWhenThenFixture fixture)
        {
            var physicalDirectory =
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    Properties.Settings.Default.SimpleServerPath));

            var applicationConfigPath = 
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "applicationhost.config"));

            var actualConfigPath =
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "applicationhost.fixed.config"));

            fixture.beforeAll(delegate()
            {
                var appDataPath = Path.Combine(physicalDirectory, "App_Data");
                foreach(string file in Directory.GetFiles(appDataPath, "*"))
                {
                    File.Delete(file);

                    if (File.Exists(file))
                        throw new Exception("unable to delete file " + file);
                }

                var originalConfig = File.ReadAllText(applicationConfigPath);

                var originalSitePhysicalDirectory = @"C:\src\FBDerp\SimpleWebApplication";

                if (originalConfig.IndexOf(originalSitePhysicalDirectory) == -1)
                {
                    throw new Exception("Did not find replacement target for physical directory in applicationhost.config");
                }

                var newConfig = originalConfig.Replace(originalSitePhysicalDirectory, physicalDirectory);

                File.WriteAllText(actualConfigPath, newConfig);            
            });

            var serverUnderTest = fixture.beforeAll(() => new IISExpressDriver());

            fixture.beforeAll(() => serverUnderTest.StartWithConfigurationFile(actualConfigPath));

            return serverUnderTest;
        }
    }
}
