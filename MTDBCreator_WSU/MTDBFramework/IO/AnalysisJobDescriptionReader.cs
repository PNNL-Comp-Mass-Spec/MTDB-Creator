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

            var analysisJobItems = new List<AnalysisJobItem>();

            using (var reader = new StreamReader(path))
            {
                SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    analysisJobItems.Add(ProcessLine(line));

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
            string[] actualHeaders = actualHeader.Split(Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                m_actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override AnalysisJobItem ProcessLine(string line)
        {
            string[] lineCells = line.Split(Delimiters, StringSplitOptions.None);

            var jobItem = new AnalysisJobItem
                (
                Path.Combine(lineCells[m_actualHeaderMaps[DefaultHeaders.BASE_FOLDER]], lineCells[m_actualHeaderMaps[DefaultHeaders.FILE_NAME]]),
                (LcmsIdentificationTool)Enum.Parse(typeof(LcmsIdentificationTool), lineCells[m_actualHeaderMaps[DefaultHeaders.TOOL]], true)
                );

            return jobItem;
        }

        private readonly Dictionary<DefaultHeaders, int> m_actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            TOOL,
            FILE_NAME,
            BASE_FOLDER
        }
    }
}
