﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
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

            // Get Result to Sequence Map
            ResultToSequenceMapReader resultToSequenceMapReader = new ResultToSequenceMapReader();
            Dictionary<int, int> resultToSequenceDictionary = new Dictionary<int, int>();

            // Get the Targets
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
                //result.Mz = result.MonoisotopicMass / result.Charge;
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

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }
    }
}
