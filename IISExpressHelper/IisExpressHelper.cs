using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Threading;

namespace TargetApplication.Tests.Helpers
{
    /// <summary>
    /// Helps iisexpress.exe automation.
    /// </summary>
    public class IisExpressHelper
    {
        private readonly string executablePath;
        private readonly string appPath;
        private readonly int port;

        /// <summary>
        /// Initializes a new instance of the IisExpressHelper class with the specified parameters.
        /// </summary>
        /// <param name="path">The path of a iisexpress.exe file.</param>
        /// <param name="appPath">The full physical path of the application to run.</param>
        /// <param name="port">The port to which the application will bind.</param>
        public IisExpressHelper(string path, string appPath, int port)
        {
            if (port < 1) throw new ArgumentOutOfRangeException(nameof(port));

            executablePath = path ?? throw new ArgumentNullException(nameof(path));
            this.appPath = appPath ?? throw new ArgumentNullException(nameof(appPath));
            this.port = port;
        }

        /// <summary>
        /// Start the web application.
        /// </summary>
        public void Start()
        {
            var arguments = string.Join(" ", CreateArguments());

            using (Process.Start(executablePath, arguments))
            {
                var uri = new Uri($"http://localhost:{port}/");
                var timeout = DateTime.Now + TimeSpan.FromMinutes(3);

                while (DateTime.Now < timeout)
                {
                    using (var client = new HttpClient())
                    {
                        Thread.Sleep(1000);

                        var response = client.GetAsync(uri).ConfigureAwait(false).GetAwaiter().GetResult();

                        if ((((int)response.StatusCode) / 100) != 5) return;
                    }
                }

                throw new TimeoutException();
            }
        }

        public string BaseUrl => $"http://localhost:{port}/";

        /// <summary>
        /// Stop the web application.
        /// </summary>
        public void StopAll()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='iisexpress.exe'"))
            using (var processes = searcher.Get())
            {
                foreach (var process in processes.OfType<ManagementObject>())
                {
                    using (process)
                    {
                        var commandLine = Convert.ToString(process["CommandLine"]);

                        if (commandLine.Contains(appPath) && commandLine.Contains(port.ToString()))
                        {
                            try
                            {
                                process.InvokeMethod("Terminate", new object[] { });
                            }
                            catch
                            {
                                // ignore.
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<string> CreateArguments()
        {
            yield return $"/path:\"{appPath}\"";
            yield return $"/port:{port}";
        }
    }
}
