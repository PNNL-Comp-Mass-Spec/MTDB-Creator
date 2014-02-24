#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

#endregion

namespace MTDBFramework.IO
{
    public class XTandemAnalysisReader : TableDataReaderBase<XTandemResult>, IAnalysisReader
    {
        public Options ReaderOptions { get; set; }

        public XTandemAnalysisReader(Options options)
        {
            this.ReaderOptions = options;
        }

        public LcmsDataSet Read(string path)
        {
            List<XTandemResult> results = new List<XTandemResult>();
            XTandemTargetFilter filter = new XTandemTargetFilter(this.ReaderOptions);

            // Get Result to Sequence Map

            ResultToSequenceMapReader resultToSequenceMapReader = new ResultToSequenceMapReader();
            Dictionary<int, int> resultToSequenceDictionary = new Dictionary<int, int>();

            foreach (ResultToSequenceMap map in resultToSequenceMapReader.Read(path.Insert(path.LastIndexOf(".txt"), "_ResultToSeqMap")))
            {
                resultToSequenceDictionary.Add(map.ResultId, map.UniqueSequenceId);
            }

            // Get Sequence Info

            SequenceInfoReader sequenceInfoReader = new SequenceInfoReader();
            Dictionary<int, SequenceInfo> sequenceInfoDictionary = new Dictionary<int, SequenceInfo>();

            foreach (SequenceInfo info in sequenceInfoReader.Read(path.Insert(path.LastIndexOf(".txt"), "_SeqInfo")))
            {
                sequenceInfoDictionary.Add(info.Id, info);
            }

            // Get Targets
            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    XTandemResult result = this.ProcessLine(line);

                    if (!filter.ShouldFilter(result))
                    {
                        // Database use
                        result.DataSet = new TargetDataSet() { Path = path };

                        if (resultToSequenceDictionary.ContainsKey(result.AnalysisId))
                        {
                            result.IsSeqInfoExist = 1;
                            result.ModificationCount = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].ModificationCount;
                            result.ModificationDescription = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].ModificationDescription;
                            result.SeqInfoMonoisotopicMass = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].MonoisotopicMass;
                        }

                        results.Add(result);
                    }
                }
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(this.ReaderOptions.PredictorType), results);

            List<XTandemResult> regResults = new List<XTandemResult>();
            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.XTandem, results);
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            DefaultHeaders header;

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

                bool result = Enum.TryParse(actualHeaders[i], true, out header);

                if (result)
                {
                    actualHeaderMaps.Add(header, i);
                }
            }
        }

        protected override XTandemResult ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            XTandemResult result = new XTandemResult();

            // Fields in Target

            result.Scan = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Scan]]);
            result.Charge = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Charge]]);
            result.MonoisotopicMass = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Peptide_MH]]) - 1.00727649;
            result.MultiProteinCount = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Multiple_Protein_Count]]);
            result.Sequence = lineCells[actualHeaderMaps[DefaultHeaders.Peptide_Sequence]];
            result.CleanPeptide = Target.CleanSequence(result.Sequence);
            result.Mz = result.MonoisotopicMass / result.Charge;
            result.AnalysisId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Result_ID]]);
            result.PeptideInfo = new TargetPeptideInfo()
            {
                Peptide/*InfoSequence*/ = result.Sequence,
                /*PeptideInfo*/CleanPeptide = result.Sequence
            };
            // Fields in XTandemResult

            if (actualHeaderMaps.ContainsKey(DefaultHeaders.Group_ID))
            {
                result.GroupId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Group_ID]]);
            }

                result.BScore = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.b_score]]);
            result.DeltaCn2 = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.DeltaCn2]]);
            result.DelM = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Delta_Mass]]);
            result.LogIntensity = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Peptide_Intensity_Log]]);
            result.LogPeptideEValue = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Peptide_Expectation_Value_Log]]);
            result.NumberYIons = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.y_ions]]);
            result.NumberBIons = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.b_ions]]);
            result.PeptideHyperscore = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.Peptide_Hyperscore]]);
            result.YScore = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.y_score]]);

            result.TrypticState = XTandemResult.CalculateTrypticState(result.Sequence);

            // I dont know where these magic numbers came from.  This is a deep thing... - Brian
            double highNorm = 0;

            if (result.Charge == 1)
            {
                highNorm = 0.082 * result.PeptideHyperscore;
            }
            else if (result.Charge == 2)
            {
                highNorm = 0.085 * result.PeptideHyperscore;
            }
            else
            {
                highNorm = 0.0872 * result.PeptideHyperscore;
            }

            result.HighNormalizedScore = highNorm;

            return result;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            Result_ID,
            Group_ID,
            Scan,
            Charge,
            Peptide_MH,
            Peptide_Hyperscore,
            Peptide_Expectation_Value_Log,
            Multiple_Protein_Count,
            Peptide_Sequence,
            DeltaCn2,
            y_score,
            y_ions,
            b_score,
            b_ions,
            Delta_Mass,
            Peptide_Intensity_Log,
            DelM_PPM
        };
    }
}