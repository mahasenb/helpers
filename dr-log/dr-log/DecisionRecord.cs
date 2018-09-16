using System;
using System.IO;

namespace dr_log
{
    public class DecisionRecord
    {
        public string Title;
        public string DecisionNumber;
        public string[] Tags;
        public string Risks;
        public string FilePath;

        private string text;

        public DecisionRecord(string fileName)
        {
            ReadFile(fileName);
            FilePath = fileName;
        }

        private void ReadFile(string fileName)
        {
            text = File.ReadAllText(fileName);

            ExtractDecisionNumberAndTitle();
            ExtractRisks();
            ExtractTags();
        }

        private void ExtractRisks()
        {
            Risks = ExtractSection("Risks");
        }

        private void ExtractTags()
        {
            string tagText = ExtractSection("Tags");
            Tags = tagText.Split(", ");
        }

        private void ExtractDecisionNumberAndTitle()
        {
            if (!text.StartsWith("#"))
            {
                throw new InvalidOperationException("File doesn't start with a heading style.");
            }

            string firstLine = text.Substring(0, text.IndexOf(Environment.NewLine, StringComparison.Ordinal));
            int indexOfPeriod = firstLine.IndexOf('.', StringComparison.Ordinal);

            if (indexOfPeriod < 1)
            {
                throw new InvalidDataException($"Decision number not found in the title line: {firstLine}");
            }

            DecisionNumber = firstLine.Substring(1, indexOfPeriod - 1).Trim();
            FillZerosToDecisionNumber();

            Title = firstLine.Substring(indexOfPeriod + 1).Trim();
        }

        private string ExtractSection(string sectionTitle)
        {
            int startOfSection = text.IndexOf("## " + sectionTitle, StringComparison.Ordinal) + 3 + sectionTitle.Length;
            string remainderOfFile = text.Substring(startOfSection);
            int lengthOfSection = remainderOfFile.IndexOf("##", StringComparison.Ordinal);

            if (lengthOfSection < 0)
            {
                lengthOfSection = remainderOfFile.Length;
            }

            return remainderOfFile.Substring(0, lengthOfSection).Trim();
        }

        private void FillZerosToDecisionNumber()
        {
            while (DecisionNumber.Length < 4)
            {
                DecisionNumber = "0" + DecisionNumber;
            }
        }
    }
}
