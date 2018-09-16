using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dr_log
{
    public class IndexFile
    {
        private readonly string filePath;
        private readonly string fullContent;
        private string before;
        private string after;

        private readonly List<string> decisionRecordList = new List<string>();

        public IndexFile(string filePath)
        {
            this.filePath = filePath;
            if (File.Exists(filePath))
            {
                fullContent = File.ReadAllText(filePath);
            }

            ExtractBeforeAndAfter();
        }

        public void AppendDecisionEntry(DecisionRecord record)
        {
            string decisionMarkDown =
                $"- [{record.DecisionNumber}]({Path.GetRelativePath(Path.GetDirectoryName(filePath), record.FilePath)}) - {record.Title}";

            decisionRecordList.Add(decisionMarkDown);
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.Write(before);

                writer.Write(Environment.NewLine);
                writer.Write(Environment.NewLine);

                foreach (var record in decisionRecordList)
                {
                    writer.WriteLine(record);
                }
                writer.Write(after);
            }
        }

        private void ExtractBeforeAndAfter()
        {
            string drLogStartText = "<!-- drlog -->";
            string drLogEndText = "<!-- drlogstop -->";

            int indexOfLogStart = fullContent.IndexOf(drLogStartText, StringComparison.Ordinal);

            if (indexOfLogStart < 0)
            {
                before = fullContent + Environment.NewLine + drLogStartText;
                after = drLogEndText;
                return;
            }

            before = fullContent.Substring(0, indexOfLogStart + drLogStartText.Length).TrimEnd();

            int indexOfLogEnd = fullContent.IndexOf(drLogEndText, StringComparison.Ordinal);
            after = fullContent.Substring(indexOfLogEnd);
        }
    }
}
