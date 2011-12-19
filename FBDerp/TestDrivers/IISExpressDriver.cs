using System;

namespace FBDerp.TestDrivers
{
	public class IISExpressDriver : ProcessDriver
	{
		public string Url { get; private set;  }

		public void Start(string physicalPath, int port)
		{
		    var arguments = @"/systray:false /port:" + port + @" /path:" + physicalPath;

            Console.WriteLine("Running IISExpress as: " + arguments);

		    StartProcess(@"c:\program files (x86)\IIS Express\IISExpress.exe",
				arguments);

			var match = WaitForConsoleOutputMatching(@"Successfully registered URL ""([^""]*)""");

			Url = match.Groups[1].Value.TrimEnd('/') + "/";
		}

        public string UrlFor(string path)
        {
            return Url + path.TrimStart('/');
        }

		protected override void Shutdown()
		{
			_process.Kill();

			if (!_process.WaitForExit(10000))
				throw new Exception("IISExpress did not halt within 10 seconds.");
		}
	}
}