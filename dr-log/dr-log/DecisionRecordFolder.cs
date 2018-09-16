using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dr_log
{
    public class DecisionRecordFolder
    {
        public List<DecisionRecord> DecisionRecords = new List<DecisionRecord>();

        internal DecisionRecordFolder(string folderPath, string indexFileName, string riskFileName)
        {
            string[] markDownFilesInFolder = Directory.GetFiles(folderPath, "*.md");

            foreach (var markdownFile in markDownFilesInFolder)
            {
                if (!markdownFile.Equals(indexFileName) && !markdownFile.Equals(riskFileName))
                {
                    DecisionRecords.Add(new DecisionRecord(markdownFile));
                }
            }
        }
    }
}
