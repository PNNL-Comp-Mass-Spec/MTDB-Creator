#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public class SequenceInfoReader : TableDataReaderBase<SequenceInfo>
    {   
        public IEnumerable<SequenceInfo> Read(string path)
        {
            // ArgumentNullException?

            List<SequenceInfo> sequenceInfoList = new List<SequenceInfo>();

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    sequenceInfoList.Add(this.ProcessLine(line));
                }
            }

            return sequenceInfoList;
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override SequenceInfo ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            SequenceInfo info = new SequenceInfo();
            info.Id = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Unique_Seq_ID]]);
            info.ModificationCount = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Mod_Count]]);
            info.ModificationDescription = lineCells[actualHeaderMaps[DefaultHeaders.Mod_Description]];
            info.MonoisotopicMass = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Monoisotopic_Mass]]);

            return info;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();
        

        private enum DefaultHeaders
        {
            Unique_Seq_ID,
            Mod_Count,
            Mod_Description,
            Monoisotopic_Mass
        }
    }
}
