#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;

#endregion

namespace MTDBFramework.IO
{
    public class MSAlignAnalysisReader : TableDataReaderBase<MSAlignResult>, IAnalysisReader
    {
        public Options ReaderOptions { get; set; }

        public MSAlignAnalysisReader(Options options)
        {
            this.ReaderOptions = options;
        }

        public LcmsDataSet Read(string path)
        {
            List<MSAlignResult> results = new List<MSAlignResult>();

            // Get Targets

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    results.Add(this.ProcessLine(line));
                }
            }

            AnalysisReaderHelper.CalculateObservedNet(results);


            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            DefaultHeaders header;

            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                if (actualHeaders[i] == "Scan(s)")
                {
                    actualHeaders[i] = actualHeaders[i].Replace("(s)", String.Empty);
                }
                else if (actualHeaders[i] == "E-value")
                {
                    actualHeaders[i] = actualHeaders[i].Replace("-", String.Empty);
                }

                bool result = Enum.TryParse(actualHeaders[i], true, out header);

                if (result)
                {
                    actualHeaderMaps.Add(header, i);
                }
            }
        }

        protected override MSAlignResult ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            MSAlignResult result = new MSAlignResult();

            // Fields in Target

            result.Scan = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Scan]]);
            result.Charge = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Charge]]);
            result.MonoisotopicMass = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Adjusted_Precursor_Mass]]);
            result.EScore = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Evalue]]);
            result.Sequence = lineCells[actualHeaderMaps[DefaultHeaders.Peptide]];

            // TODO
            result.PeptideInfo = new TargetPeptideInfo()
            {
                Peptide/*InfoSequence*/ = result.Sequence,
                /*PeptideInfo*/CleanPeptide = result.Sequence
            };

            return result;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            Scan,
            Charge,
            Adjusted_Precursor_Mass,
            Evalue,
            Peptide
        };
    }
}