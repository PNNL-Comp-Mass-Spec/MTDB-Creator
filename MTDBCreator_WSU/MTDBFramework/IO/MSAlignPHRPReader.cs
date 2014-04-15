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
    /// <summary>
    /// Summary Description for MSAlignPHRP Reader
    /// </summary>
    class MSAlignPHRPReader : IPHRPReader
    {
        public Options ReaderOptions { get; set; }

        public MSAlignPHRPReader(Options options)
        {
            this.ReaderOptions = options;
        }

        public LcmsDataSet Read(string path)
        {
            List<MSAlignResult> results = new List<MSAlignResult>();
            MSAlignTargetFilter filter = new MSAlignTargetFilter(this.ReaderOptions);

            Dictionary<int, ProteinInformation> proteinInfos = new Dictionary<int, ProteinInformation>();

            // Get the Evidences using PHRPReader which looks at the path that was passed in
            var reader = new PHRPReader.clsPHRPReader(path);
            while (reader.CanRead)
            {
                MSAlignResult result = new MSAlignResult();
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
                result.Mz = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(reader.CurrentPSM.PrecursorNeutralMass, 0, reader.CurrentPSM.Charge);
                result.DelM = Convert.ToDouble(reader.CurrentPSM.MassErrorDa);
                if (reader.CurrentPSM.MassErrorPPM.Length != 0)
                {
                    result.DelM_PPM = Convert.ToDouble(reader.CurrentPSM.MassErrorPPM);
                }

                result.PeptideInfo = new TargetPeptideInfo()
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide,
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };

                result.EScore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["EValue"]);

				// If it passes the filter, check for if there are any modifications, add them if needed, and add the result to the list
                if (!filter.ShouldFilter(result))
                {
                    result.DataSet = new TargetDataSet() { Path = path };

                    result.ModificationCount = (short)reader.CurrentPSM.ModifiedResidues.Count;
                    result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;

                    foreach (var protein in reader.CurrentPSM.ProteinDetails)
                    {
                        ProteinInformation Protein = new ProteinInformation();
                        Protein.ProteinName = protein.ProteinName;
                        Protein.CleavageState = protein.CleavageState;
                        Protein.TerminusState = protein.TerminusState;
                        Protein.ResidueStart = protein.ResidueStart;
                        Protein.ResidueEnd = protein.ResidueEnd;

                        if (proteinInfos.ContainsValue(Protein))
                        {
                            result.Proteins.Add(Protein);
                        }
                        else
                        {
                            proteinInfos.Add((proteinInfos.Count + 1), Protein);
                            result.Proteins.Add(Protein);
                        }
                    }

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

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }
    }
}
