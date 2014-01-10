#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public class ResultToSequenceMapReader : TableDataReaderBase<ResultToSequenceMap>
    {
        public IEnumerable<ResultToSequenceMap> Read(string path)
        {
            // ArgumentNullException?

            List<ResultToSequenceMap> resultToSequenceMaps = new List<ResultToSequenceMap>();

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    resultToSequenceMaps.Add(this.ProcessLine(line));
                }
            }

            return resultToSequenceMaps;
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override ResultToSequenceMap ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            ResultToSequenceMap map = new ResultToSequenceMap();

            map.ResultId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Result_ID]]);
            map.UniqueSequenceId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Unique_Seq_ID]]);

            return map;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            Result_ID,
            Unique_Seq_ID
        }
    }
}