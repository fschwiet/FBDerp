using System;
using System.IO;
using FBDerp.Properties;
using NJasmine;

namespace FBDerp.TestDrivers
{
	public class IISExpressDriver : ProcessDriver
	{
		public string Url { get; private set;  }

		public void Start(string physicalPath, int port)
		{
		    var arguments = @"/systray:false /port:" + port + @" /path:" + physicalPath;

		    StartWithCommandLineArguments(arguments);
		}

        public void StartWithConfigurationFile(string applicationConfigurationPath)
        {
            StartWithCommandLineArguments(@"/systray:false /config:""" + applicationConfigurationPath + @"""");
        }
        
        private void StartWithCommandLineArguments(string arguments)
	    {
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

	    public static IISExpressDriver StartServer()
	    {
	        var physicalDirectory =
	            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	                Settings.Default.SimpleServerPath));

	        var applicationConfigPath =
	            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "applicationhost.config"));

	        var actualConfigPath =
	            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "applicationhost.fixed.config"));

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

	        var serverDriver = new IISExpressDriver();

	        serverDriver.StartWithConfigurationFile(actualConfigPath);

	        return serverDriver;
	    }
	}
}