using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using PHRPReader;

namespace MTDBFramework.IO
{
    public abstract class PHRPReaderBase : IPhrpReader
    {
        protected const int PROGRESS_PCT_START = 0;
        protected const int PROGRESS_PCT_PEPTIDES_LOADED = 98;
        protected const int PROGRESS_PCT_COMPLETE = 100;

        protected readonly Dictionary<string, TargetDataSet> mDatasetCache;

        protected readonly AnalysisReaderHelper mNETPredictor;

        protected bool mAbortRequested;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PHRPReaderBase()
        {
            mDatasetCache = new Dictionary<string, TargetDataSet>(StringComparer.CurrentCultureIgnoreCase);

            mNETPredictor = new AnalysisReaderHelper();
            
            mAbortRequested = false;
        }       

        public Data.Options ReaderOptions { get; set; }

        public abstract Data.LcmsDataSet Read(string path);

        public void AbortProcessing()
        {
            mAbortRequested = true;
        }

        protected void ComputeNETs(IEnumerable<Evidence> results)
        {
            if (!mAbortRequested)
            {
                UpdateProgress(99, "Loading elution times");

                mNETPredictor.CalculateObservedNet(results);

                UpdateProgress(99, "Initializing NET predictor");
                var netPredictor = RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType);

                UpdateProgress(99, "Computing NET values");
                mNETPredictor.CalculatePredictedNet(netPredictor, results);
            }
        }

        protected clsPHRPReader InitializeReader(string path)
        {
            mAbortRequested = false;

            var oStartupOptions = new PHRPReader.clsPHRPStartupOptions
            {
                LoadModsAndSeqInfo = true,
                LoadMSGFResults = true,
                LoadScanStatsData = false,
                MaxProteinsPerPSM = 100
            };

            UpdateProgress(0, "Initializing reader");
            
            var reader = new clsPHRPReader(path, oStartupOptions)
            {
                SkipDuplicatePSMs = true,
                FastReadMode = true
            };

            return reader;
        }

        protected void StoreDatasetInfo(Evidence result, string dataFilePath)
        {
            string datasetName = DatasetPathUtility.CleanPath(dataFilePath);
            StoreDatasetInfo(result, datasetName, dataFilePath);
        }

        protected void StoreDatasetInfo(Evidence result, clsPHRPReader reader, string dataFilePath)
        {
            StoreDatasetInfo(result, reader.DatasetName, dataFilePath);
        }

        protected void StoreDatasetInfo(Evidence result, string datasetName, string dataFilePath)
        {
            // Lookup this dataset in the dataset info cache

            TargetDataSet dataset;

            if (!mDatasetCache.TryGetValue(dataFilePath, out dataset))
            {
                dataset = new TargetDataSet
                {
                    Path = dataFilePath,
                    Name = datasetName
                };

                if (string.IsNullOrEmpty(dataset.Name))
                {
                    dataset.Name = DatasetPathUtility.CleanPath(dataFilePath);
                }
                mDatasetCache.Add(dataFilePath, dataset);
            }

            result.DataSet = dataset;
          
        }

        protected static void StoreProteinInfo(clsPHRPReader reader, Evidence result)
        {
            foreach (var p in reader.CurrentPSM.ProteinDetails)
            {
                var protein = new ProteinInformation
                {
                    ProteinName = p.Value.ProteinName,
                    CleavageState = p.Value.CleavageState,
                    TerminusState = p.Value.TerminusState,
                    ResidueStart = p.Value.ResidueStart,
                    ResidueEnd = p.Value.ResidueEnd
                };
               
                result.Proteins.Add(protein);
            }
        }

        protected void StorePSMData(Evidence result, clsPHRPReader reader, double specProb)
        {
            result.Charge = reader.CurrentPSM.Charge;
            result.CleanPeptide = reader.CurrentPSM.PeptideCleanSequence;
            result.SeqWithNumericMods = reader.CurrentPSM.PeptideWithNumericMods;
            result.MonoisotopicMass = reader.CurrentPSM.PeptideMonoisotopicMass;
            result.ObservedMonoisotopicMass = reader.CurrentPSM.PrecursorNeutralMass;
            result.MultiProteinCount = (short)reader.CurrentPSM.Proteins.Count;
            result.Scan = reader.CurrentPSM.ScanNumber;
            result.Sequence = reader.CurrentPSM.Peptide;
            result.Mz =
                clsPeptideMassCalculator.ConvoluteMass(reader.CurrentPSM.PrecursorNeutralMass, 0,
                                                       reader.CurrentPSM.Charge);
            result.SpecProb = specProb;
            result.DelM = Convert.ToDouble(reader.CurrentPSM.MassErrorDa);
            result.ModificationCount = (short)reader.CurrentPSM.ModifiedResidues.Count;

            result.PeptideInfo = new TargetPeptideInfo
            {
                Peptide = result.Sequence,
                CleanPeptide = result.CleanPeptide,
                PeptideWithNumericMods = result.SeqWithNumericMods
            };

            if (reader.CurrentPSM.MassErrorPPM.Length != 0)
            {
                result.DelMPpm = Convert.ToDouble(reader.CurrentPSM.MassErrorPPM);
            }

            result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;

            StoreProteinInfo(reader, result);

            if (result.ModificationCount != 0)
            {
                foreach (var info in reader.CurrentPSM.ModifiedResidues)
                {
                    result.ModificationDescription += info.ModDefinition.MassCorrectionTag + ":" + info.ResidueLocInPeptide + " ";
                    var ptm = new PostTranslationalModification();
                    ptm.Location = info.ResidueLocInPeptide;
                    ptm.Mass = info.ModDefinition.ModificationMass;
                    ptm.Formula = info.ModDefinition.MassCorrectionTag;
                    result.PTMs.Add(ptm);
                }
                // Build the PTMs
                // TODO: build an encoded non=numeric sequence here.

                var encodedSeq = result.Sequence[0] + ".";
                int j = 0;
                foreach (var ptm in result.PTMs)
                {
                    for (; j < ptm.Location; j++)
                    {
                        encodedSeq = encodedSeq + result.CleanPeptide[j];
                    }

                    encodedSeq += "[" + ((ptm.Mass > 0)? "+":"-") + ptm.Formula + "]";
                    //}

                }
                for (; j < result.CleanPeptide.Length; j++)
                {
                    encodedSeq += result.CleanPeptide[j];
                }
                encodedSeq += "." + result.Sequence.Last();
                result.EncodedNonNumericSequence = encodedSeq;






                // END TEST PLACE



                
            }
            else
            {
                result.EncodedNonNumericSequence = result.Sequence;
            }

        }

        #region ProgressHandlers

        protected void UpdateProgress(float percentComplete)
        {
            UpdateProgress(percentComplete, string.Empty);
        }

        protected void UpdateProgress(float percentComplete, string currentTask)
        {
            float percentCompleteEffective = PROGRESS_PCT_START + percentComplete * (PROGRESS_PCT_PEPTIDES_LOADED - PROGRESS_PCT_START) / 100;

            OnProgressChanged(new PercentCompleteEventArgs(percentCompleteEffective, currentTask));
        }

        void mNETPredictor_ProgressChanged(object sender, PercentCompleteEventArgs e)
        {
            float percentCompleteEffective = PROGRESS_PCT_PEPTIDES_LOADED + e.PercentComplete * (PROGRESS_PCT_COMPLETE - PROGRESS_PCT_PEPTIDES_LOADED) / 100;

            OnProgressChanged(new PercentCompleteEventArgs(percentCompleteEffective, "Computing NETs"));
        }

        #endregion

        #region Events

        public event PercentCompleteEventHandler ProgressChanged;

        protected void OnProgressChanged(PercentCompleteEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        #endregion
       
    }
}
