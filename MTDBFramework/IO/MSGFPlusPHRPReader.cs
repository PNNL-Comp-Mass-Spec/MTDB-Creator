using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Summary Description for MSGFPlusPHRP Reader
    /// </summary>
    public class MsgfPlusPhrpReader : IPhrpReader
    {
        public Options ReaderOptions { get; set; }

        public MsgfPlusPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        public LcmsDataSet Read(string path)
        {
            var results = new List<MsgfPlusResult>();
            var filter = new MsgfPlusTargetFilter(ReaderOptions);

            var proteinInfos = new Dictionary<int, ProteinInformation>();

            // Get the Evidences using PHRPReader which looks at the path that was passed in
            var reader = new clsPHRPReader(path);
            //foreach(var protein in reader.SeqToProteinMap)

            while (reader.CanRead)
            {
                reader.MoveNext();
                if(reader.CurrentPSM.SeqID == 0)
                {
                    continue;
                }
                var result = new MsgfPlusResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID,
                    Charge = reader.CurrentPSM.Charge,
                    CleanPeptide = reader.CurrentPSM.PeptideCleanSequence,
                    SeqWithNumericMods = reader.CurrentPSM.PeptideWithNumericMods,
                    MonoisotopicMass = reader.CurrentPSM.PeptideMonoisotopicMass,
                    ObservedMonoisotopicMass = reader.CurrentPSM.PrecursorNeutralMass,
                    MultiProteinCount = (short) reader.CurrentPSM.Proteins.Count,
                    Scan = reader.CurrentPSM.ScanNumber,
                    Sequence = reader.CurrentPSM.Peptide,
                    Mz =
                        clsPeptideMassCalculator.ConvoluteMass(reader.CurrentPSM.PrecursorNeutralMass, 0,
                            reader.CurrentPSM.Charge)
                };

                result.PeptideInfo = new TargetPeptideInfo
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide,
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };
                result.PrecursorMonoMass = reader.CurrentPSM.PrecursorNeutralMass;
                result.PrecursorMz = result.PrecursorMonoMass / result.Charge;
                result.Reference = null;
                foreach (var proteinName in reader.CurrentPSM.Proteins)
                {
                    result.Reference += proteinName;
                }
                result.SpecProb = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["MSGFDB_SpecProb"]);
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTerminii;
                result.Fdr = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["FDR"]);
                result.DeNovoScore = Convert.ToInt32(reader.CurrentPSM.AdditionalScores["DeNovoScore"]);
                result.MsgfScore = Convert.ToInt32(reader.CurrentPSM.AdditionalScores["MSGFScore"]);
                result.SpecEValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["MSGFDB_SpecEValue"]);
                result.RankSpecEValue = Convert.ToInt32(reader.CurrentPSM.AdditionalScores["Rank_MSGFDB_SpecEValue"]);
                result.EValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Evalue"]);
                result.QValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Qvalue"]);
                result.PepQValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["PepQvalue"]);
                result.IsotopeError = Convert.ToInt32(reader.CurrentPSM.AdditionalScores["IsotopeError"]);
                result.DelM = Convert.ToDouble(reader.CurrentPSM.MassErrorDa);
                if (reader.CurrentPSM.MassErrorPPM.Length != 0)
                {
                    result.DelMPpm = Convert.ToDouble(reader.CurrentPSM.MassErrorPPM);
                }
                result.ModificationCount = (short)reader.CurrentPSM.ModifiedResidues.Count;
				// If it passes the filter, check for if there are any modifications, add them if needed, and add the result to the list
                if (!filter.ShouldFilter(result))
                {
                    result.DataSet = new TargetDataSet
                    {
                        Path = path,
                        Name = DatasetPathUtility.CleanPath(path)
                    };

                    result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;

                    foreach (var p in reader.CurrentPSM.ProteinDetails)
                    {
                        var protein = new ProteinInformation
                        {
                            ProteinName = p.ProteinName,
                            CleavageState = p.CleavageState,
                            TerminusState = p.TerminusState,
                            ResidueStart = p.ResidueStart,
                            ResidueEnd = p.ResidueEnd
                        };

                        if (proteinInfos.ContainsValue(protein))
                        {
                            result.Proteins.Add(protein);
                        }
                        else
                        {
                            proteinInfos.Add((proteinInfos.Count + 1), protein);
                            result.Proteins.Add(protein);
                        }
                    }
                    
                    if (result.ModificationCount != 0)
                    {
                        foreach (var info in reader.CurrentPSM.ModifiedResidues)
                        {
                            result.SeqInfoMonoisotopicMass += info.ModDefinition.ModificationMass;
                            result.ModificationDescription += info.ModDefinition.MassCorrectionTag + ":" + info.ResidueLocInPeptide + " ";
                        }
                    }
                    results.Add(result);
                }
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType), results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MsgfPlus, results);
        }
    }
}
