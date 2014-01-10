#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public class SequenceToProteinMapReader : TableDataReaderBase<SequenceToProteinMap>
    {
        public IEnumerable<SequenceToProteinMap> Read(string path)
        {
            // ArgumentNullException?

            List<SequenceToProteinMap> sequenceToProteinMaps = new List<SequenceToProteinMap>();

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    sequenceToProteinMaps.Add(this.ProcessLine(line));
                }
            }

            return sequenceToProteinMaps;
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                if (actualHeaders[i] == "Peptide_Expectation_Value_Log(e)")
                {
                    actualHeaders[i] = actualHeaders[i].Replace("(e)", String.Empty);
                }
                else if (actualHeaders[i] == "Peptide_Intensity_Log(I)")
                {
                    actualHeaders[i] = actualHeaders[i].Replace("(I)", String.Empty);
                }

                actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override SequenceToProteinMap ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            SequenceToProteinMap map = new SequenceToProteinMap();

            map.UniqueSequenceId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Unique_Seq_ID]]);
            map.CleavageState = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Cleavage_State]]);
            map.TerminusState = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Terminus_State]]);
            map.ProteinName = lineCells[actualHeaderMaps[DefaultHeaders.Protein_Name]];

            map.ProteinEValue = (!String.IsNullOrEmpty(lineCells[actualHeaderMaps[DefaultHeaders.Protein_Expectation_Value_Log]])) ? Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Protein_Expectation_Value_Log]]) : Convert.ToDouble(UNDEFINED);
            map.ProteinIntensityLog = (!String.IsNullOrEmpty(lineCells[actualHeaderMaps[DefaultHeaders.Protein_Expectation_Value_Log]])) ? Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Protein_Intensity_Log]]) : Convert.ToDouble(UNDEFINED);

            return map;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            Unique_Seq_ID,
            Cleavage_State,
            Terminus_State,
            Protein_Name,
            Protein_Expectation_Value_Log,
            Protein_Intensity_Log
        }

        private const int UNDEFINED = -100;
    }
}