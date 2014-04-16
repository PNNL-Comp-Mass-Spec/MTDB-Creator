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

            var sequenceToProteinMaps = new List<SequenceToProteinMap>();

            using (var reader = new StreamReader(path))
            {
                SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    sequenceToProteinMaps.Add(ProcessLine(line));
                }
            }

            return sequenceToProteinMaps;
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            string[] actualHeaders = actualHeader.Split(Delimiters, StringSplitOptions.None);

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

                m_actualHeaderMaps.Add((DefaultHeaders)Enum.Parse(typeof(DefaultHeaders), actualHeaders[i]), i);
            }
        }

        protected override SequenceToProteinMap ProcessLine(string line)
        {
            string[] lineCells = line.Split(Delimiters, StringSplitOptions.None);

            var map = new SequenceToProteinMap
            {
                UniqueSequenceId = Convert.ToInt32(lineCells[m_actualHeaderMaps[DefaultHeaders.UNIQUE_SEQ_ID]]),
                CleavageState = Convert.ToInt16(lineCells[m_actualHeaderMaps[DefaultHeaders.CLEAVAGE_STATE]]),
                TerminusState = Convert.ToInt16(lineCells[m_actualHeaderMaps[DefaultHeaders.TERMINUS_STATE]]),
                ProteinName = lineCells[m_actualHeaderMaps[DefaultHeaders.PROTEIN_NAME]],
                ProteinEValue =
                    (!String.IsNullOrEmpty(lineCells[m_actualHeaderMaps[DefaultHeaders.PROTEIN_EXPECTATION_VALUE_LOG]]))
                        ? Convert.ToDouble(lineCells[m_actualHeaderMaps[DefaultHeaders.PROTEIN_EXPECTATION_VALUE_LOG]])
                        : Convert.ToDouble(UNDEFINED),
                ProteinIntensityLog =
                    (!String.IsNullOrEmpty(lineCells[m_actualHeaderMaps[DefaultHeaders.PROTEIN_EXPECTATION_VALUE_LOG]]))
                        ? Convert.ToDouble(lineCells[m_actualHeaderMaps[DefaultHeaders.PROTEIN_INTENSITY_LOG]])
                        : Convert.ToDouble(UNDEFINED)
            };

            return map;
        }

        private readonly Dictionary<DefaultHeaders, int> m_actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            UNIQUE_SEQ_ID,
            CLEAVAGE_STATE,
            TERMINUS_STATE,
            PROTEIN_NAME,
            PROTEIN_EXPECTATION_VALUE_LOG,
            PROTEIN_INTENSITY_LOG
        }

        private const int UNDEFINED = -100;
    }
}