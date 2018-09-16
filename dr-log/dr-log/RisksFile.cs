using System;
using System.IO;

namespace dr_log
{
    public class RisksFile
    {
        private string text = "# Full list of risks" + Environment.NewLine;
        private readonly string filePath;

        public RisksFile(string filePath)
        {
            this.filePath = filePath;
        }

        public void Append(DecisionRecord record)
        {
            text += Environment.NewLine +  $"## {record.DecisionNumber} - {record.Title}" + Environment.NewLine + Environment.NewLine;
            text += record.Risks + Environment.NewLine;
        }

        public void Save()
        {
            File.WriteAllText(filePath, text);
        }
    }
}
