using System;
using System.IO;
using NJasmine;

namespace FBDerp.TestDrivers
{
    public static class IISExpressTestExtensions
    {
        public static IISExpressDriver ArrangeServer(this GivenWhenThenFixture fixture)
        {
            var serverUnderTest = fixture.beforeAll(() => new IISExpressDriver());

            var sitePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Properties.Settings.Default.SimpleServerPath);

            sitePath = Path.GetFullPath(sitePath);

            fixture.beforeAll(() => serverUnderTest.Start(sitePath, 8084));

            return serverUnderTest;
        }
    }
}
