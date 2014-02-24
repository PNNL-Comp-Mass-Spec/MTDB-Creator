using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
    class XTandemPHRPReader : IPHRPReader
    {
        public Options ReaderOptions { get; set; }

        public XTandemPHRPReader(Options options)
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

            //foreach (ResultToSequenceMap map in resultToSequenceMapReader.Read(path.Insert(path.LastIndexOf(".txt"), "_ResultToSeqMap")))
            //{
            //    resultToSequenceDictionary.Add(map.ResultId, map.UniqueSequenceId);
            //}

            // Get the Targets
            var reader = new PHRPReader.clsPHRPReader(path);
            while (reader.CanRead)
            {
                XTandemResult result = new XTandemResult();
                reader.MoveNext();

                result.AnalysisId = reader.CurrentPSM.ResultID;
                result.Charge = reader.CurrentPSM.Charge;
                result.CleanPeptide = reader.CurrentPSM.PeptideCleanSequence;
                result.SeqWithNumericMods = reader.CurrentPSM.PeptideWithNumericMods;
                result.MonoisotopicMass = reader.CurrentPSM.PeptideMonoisotopicMass;
                result.ObservedMonoisotopicMass = reader.CurrentPSM.PrecursorNeutralMass;
                result.MultiProteinCount = (short)reader.CurrentPSM.Proteins.Count;
                result.Scan = reader.CurrentPSM.ScanNumber;
                result.Sequence = reader.CurrentPSM.Peptide;

                result.PeptideInfo = new TargetPeptideInfo()
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide, 
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };

                result.BScore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["b_score"]);
                result.DeltaCn2 = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["DeltaCn2"]);
                result.DelM = Convert.ToDouble(reader.CurrentPSM.MassErrorDa);
                result.LogIntensity = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Intensity_Log(I)"]);
                result.LogPeptideEValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Expectation_Value_Log(e)"]);
                result.NumberBIons = Convert.ToInt16(reader.CurrentPSM.AdditionalScores["b_ions"]);
                result.NumberYIons = Convert.ToInt16(reader.CurrentPSM.AdditionalScores["y_ions"]);
                result.PeptideHyperscore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Hyperscore"]);
                result.TrypticState = reader.CurrentPSM.NumTrypticTerminii;
                result.YScore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["y_score"]);
                result.Mz = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(reader.CurrentPSM.PrecursorNeutralMass, 0, reader.CurrentPSM.Charge); 
                result.DelM_PPM = Convert.ToDouble(reader.CurrentPSM.MassErrorPPM);

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

                if (!filter.ShouldFilter(result))
                {
                    result.DataSet = new TargetDataSet() { Path = path };

                    result.ModificationCount = (short)reader.CurrentPSM.ModifiedResidues.Count;
                    result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;
                    

                    if (result.ModificationCount != 0)
                    {
                        foreach (clsAminoAcidModInfo info in reader.CurrentPSM.ModifiedResidues)
                        {
                            result.SeqInfoMonoisotopicMass += info.ModDefinition.ModificationMass;
                            result.ModificationDescription += info.ModDefinition.MassCorrectionTag + ":" + info.ResidueLocInPeptide + " ";
                        }
                    }

                    results.Add(result);
                }
            }
            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(this.ReaderOptions.PredictorType), results);
            
            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.XTandem, results);
        }
    }  
    
}
