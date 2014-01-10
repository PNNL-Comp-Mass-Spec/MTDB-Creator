#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public class AnalysisJobDescriptionReader : TableDataReaderBase<AnalysisJobItem>
    {
        public IEnumerable<AnalysisJobItem> Read(string path)
        {
            // ArgumentNullException?

            List<AnalysisJobItem> analysisJobItems = new List<AnalysisJobItem>();

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    analysisJobItems.Add(this.ProcessLine(line));

                    if (analysisJobItems[analysisJobItems.Count - 1].FilePath.StartsWith(@"."))
                    {
                        Environment.CurrentDirectory = Path.GetDirectoryName(path);

                        analysisJobItems[analysisJobItems.Count - 1].FilePath = Path.GetFullPath(analysisJobItems[analysisJobItems.Count - 1].FilePath);
                    }
                }
            }

            return analysisJobItems;
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override AnalysisJobItem ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            AnalysisJobItem jobItem = new AnalysisJobItem
                (
                Path.Combine(lineCells[actualHeaderMaps[DefaultHeaders.BaseFolder]], lineCells[actualHeaderMaps[DefaultHeaders.FileName]]),
                (LcmsIdentificationTool)Enum.Parse(typeof(LcmsIdentificationTool), lineCells[actualHeaderMaps[DefaultHeaders.Tool]], true)
                );

            return jobItem;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            Tool,
            FileName,
            BaseFolder
        }
    }
}
