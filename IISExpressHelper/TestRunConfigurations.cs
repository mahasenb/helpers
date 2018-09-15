using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetApplication.Tests.Helpers
{
    internal class TestRunConfigurations
    {
        public static string IisExpressExecutablePath
        {
            get
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                return programFiles + "/IIS Express/iisexpress.exe";
            }
        }

        public static int HttpPort => 2020;

        public static string TargetApplicationPath
        {
            get
            {
                var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
                return Path.Combine(solutionFolder ?? throw new InvalidOperationException(), "TargetApplication");
            }
        }
    }
}
