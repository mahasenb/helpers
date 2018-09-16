using System;
using System.IO;

namespace dr_log
{
    public class Program
    {
        private static string markDownDirectory;
        private static string indexFileName;
        private static string riskFileName;

        static void Main(string[] arguments)
        {
            ProcessArguments(arguments);
            SetDefaultParametersIfNeeded();
            
            DecisionRecordFolder folder = new DecisionRecordFolder(markDownDirectory, indexFileName, riskFileName);
            IndexFile indexFile = new IndexFile(indexFileName);
            RisksFile riskFile = new RisksFile(riskFileName);

            foreach (var decisionRecord in folder.DecisionRecords)
            {
                indexFile.AppendDecisionEntry(decisionRecord);
                riskFile.Append(decisionRecord);
            }

            indexFile.Save();
            riskFile.Save();
        }

        private static void SetDefaultParametersIfNeeded()
        {
            if (string.IsNullOrWhiteSpace(markDownDirectory))
            {
                markDownDirectory = Environment.CurrentDirectory;
            }

            if (string.IsNullOrWhiteSpace(indexFileName))
            {
                indexFileName = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "index.md";
            }

            if (string.IsNullOrWhiteSpace(riskFileName))
            {
                indexFileName = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "risks.md";
            }
        }

        private static void ProcessArguments(string[] arguments)
        {
            if (arguments.Length > 3)
            {
                DisplayHelp();
                return;
            }

            foreach (var argument in arguments)
            {
                if (argument.StartsWith("h"))
                {
                    DisplayHelp();
                    return;
                }

                if (argument.StartsWith("d"))
                {
                    ExtractMarkDownDirectory(argument);
                }

                if (argument.StartsWith("index"))
                {
                    indexFileName = ExtractFileName(argument);
                }

                if (argument.StartsWith("risks"))
                {
                    riskFileName = ExtractFileName(argument);
                }
            }
        }

        private static void ExtractMarkDownDirectory(string argument)
        {
            markDownDirectory = argument.Substring(2);
            if(!Directory.Exists(markDownDirectory))
                throw new InvalidOperationException($"The folder {markDownDirectory} is not found.");
        }

        private static string ExtractFileName(string argument)
        {
            string fileName = argument.Substring(6);
            
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File '{fileName}' doesn't exist. A new file will be created.");
            }

            return fileName;
        }

        private static void DisplayHelp()
        {
            Console.Write("" +
                          "Usage: dr-log [-d:<directory>] [<input>] [-h]" + Environment.NewLine +
                          Environment.NewLine +
                          "  input:  The markdown file to contain the table of contents." + Environment.NewLine +
                          "          If no <input> file is specified, index.md file in the current directory is assumed." + Environment.NewLine +
                          Environment.NewLine +
                          "  -d:     Scans the given <directory> for .md files.'" + Environment.NewLine +
                          "          (Without this flag, the current working directory is chosen as default.)" + Environment.NewLine +
                          Environment.NewLine +
                          "  -h:     Shows this output" + Environment.NewLine);
        }
    }
}
