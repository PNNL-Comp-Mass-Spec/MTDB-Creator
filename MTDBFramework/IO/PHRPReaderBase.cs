using System;
using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Base class for file readers that are part of PHRP
    /// </summary>
    public abstract class PHRPReaderBase : IPhrpReader
    {
        /// <summary>
        /// Progress status constant
        /// </summary>
        protected const int PROGRESS_PCT_START = 0;

        /// <summary>
        /// Progress status constant
        /// </summary>
        protected const int PROGRESS_PCT_PEPTIDES_LOADED = 98;

        /// <summary>
        /// Progress status constant
        /// </summary>
        protected const int PROGRESS_PCT_COMPLETE = 100;

        /// <summary>
        /// Data storage
        /// </summary>
        protected readonly Dictionary<string, TargetDataSet> DatasetCache;

        /// <summary>
        /// Elution time prediction
        /// </summary>
        protected readonly AnalysisReaderHelper NETPredictor;

        /// <summary>
        /// Allow thread cancellation
        /// </summary>
        protected bool AbortRequested;

        /// <summary>
        /// Cached peptide mass calculator
        /// </summary>
        protected clsPeptideMassCalculator mPeptideMassCalculator;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PHRPReaderBase()
        {
            DatasetCache = new Dictionary<string, TargetDataSet>(StringComparer.CurrentCultureIgnoreCase);

            NETPredictor = new AnalysisReaderHelper();

            AbortRequested = false;

            mPeptideMassCalculator = new clsPeptideMassCalculator();
        }

        /// <summary>
        /// ReaderOption
        /// </summary>
        public Options ReaderOptions { get; set; }

        /// <summary>
        /// File Reader
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract LcmsDataSet Read(string path);

        /// <summary>
        /// Allow thread cancellation
        /// </summary>
        public void AbortProcessing()
        {
            AbortRequested = true;
        }

        /// <summary>
        /// Calculate the Normal Elution Time
        /// </summary>
        /// <param name="results"></param>
        protected void ComputeNets(IEnumerable<Evidence> results)
        {
            if (!AbortRequested)
            {
                UpdateProgress(99, "Loading elution times");

                NETPredictor.CalculateObservedNet(results);

                UpdateProgress(99, "Initializing NET predictor");
                var netPredictor = RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType);

                UpdateProgress(99, "Computing NET values");
                NETPredictor.CalculatePredictedNet(netPredictor, results);
            }
        }

        /// <summary>
        /// PHRPReader configuration
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected clsPHRPReader InitializeReader(string path)
        {
            AbortRequested = false;

            var oStartupOptions = new clsPHRPStartupOptions
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

        /// <summary>
        /// Store Dataset Info
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dataFilePath"></param>
        protected void StoreDatasetInfo(Evidence result, string dataFilePath)
        {
            var datasetName = DatasetPathUtility.CleanPath(dataFilePath);
            StoreDatasetInfo(result, datasetName, dataFilePath);
        }

        /// <summary>
        /// Store Dataset Info
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reader"></param>
        /// <param name="dataFilePath"></param>
        protected void StoreDatasetInfo(Evidence result, clsPHRPReader reader, string dataFilePath)
        {
            StoreDatasetInfo(result, reader.DatasetName, dataFilePath);
        }

        /// <summary>
        /// Store Dataset Info
        /// </summary>
        /// <param name="result"></param>
        /// <param name="datasetName"></param>
        /// <param name="dataFilePath"></param>
        protected void StoreDatasetInfo(Evidence result, string datasetName, string dataFilePath)
        {
            // Lookup this dataset in the dataset info cache


            if (!DatasetCache.TryGetValue(dataFilePath, out var dataset))
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
                DatasetCache.Add(dataFilePath, dataset);
            }

            result.DataSet = dataset;

        }

        /// <summary>
        /// Store Protein Info
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="result"></param>
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

        /// <summary>
        /// Store PSM Data
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reader"></param>
        /// <param name="specProb"></param>
        protected void StorePsmData(Evidence result, clsPHRPReader reader, double specProb)
        {
            result.Charge = reader.CurrentPSM.Charge;
            result.CleanPeptide = reader.CurrentPSM.PeptideCleanSequence;
            result.SeqWithNumericMods = reader.CurrentPSM.PeptideWithNumericMods;
            result.MonoisotopicMass = reader.CurrentPSM.PeptideMonoisotopicMass;
            result.ObservedMonoisotopicMass = reader.CurrentPSM.PrecursorNeutralMass;
            result.MultiProteinCount = (short)reader.CurrentPSM.Proteins.Count;
            result.Scan = reader.CurrentPSM.ScanNumber;
            result.Sequence = reader.CurrentPSM.Peptide;
            result.Mz = mPeptideMassCalculator.ConvoluteMass(reader.CurrentPSM.PrecursorNeutralMass, 0,
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
                    var ptm = new PostTranslationalModification
                    {
                        Location = info.ResidueLocInPeptide,
                        Mass = info.ModDefinition.ModificationMass,
                        Formula = info.ModDefinition.MassCorrectionTag,
                        Name = info.ModDefinition.MassCorrectionTag
                    };
                    result.Ptms.Add(ptm);
                }

                var encodedSeq = result.Sequence[0] + ".";
                var j = 0;
                foreach (var ptm in result.Ptms)
                {
                    for (; j < ptm.Location; j++)
                    {
                        encodedSeq = encodedSeq + result.CleanPeptide[j];
                    }

                    encodedSeq += "[" + ((ptm.Mass > 0)? "+":"-") + ptm.Formula + "]";
                }
                for (; j < result.CleanPeptide.Length; j++)
                {
                    encodedSeq += result.CleanPeptide[j];
                }
                encodedSeq += "." + result.Sequence.Last();
                result.EncodedNonNumericSequence = encodedSeq;
            }
            else
            {
                result.EncodedNonNumericSequence = result.Sequence;
            }

        }

        #region ProgressHandlers

        /// <summary>
        /// Progress Handler
        /// </summary>
        /// <param name="percentComplete"></param>
        protected void UpdateProgress(float percentComplete)
        {
            UpdateProgress(percentComplete, string.Empty);
        }

        /// <summary>
        /// Progress Handler
        /// </summary>
        /// <param name="percentComplete"></param>
        /// <param name="currentTask"></param>
        protected void UpdateProgress(float percentComplete, string currentTask)
        {
            var percentCompleteEffective = PROGRESS_PCT_START + percentComplete * (PROGRESS_PCT_PEPTIDES_LOADED - PROGRESS_PCT_START) / 100;

            OnProgressChanged(new PercentCompleteEventArgs(percentCompleteEffective, currentTask));
        }

        #endregion

        #region Events

        /// <summary>
        /// Progress event
        /// </summary>
        public event PercentCompleteEventHandler ProgressChanged;

        /// <summary>
        /// Progress event handler
        /// </summary>
        /// <param name="e"></param>
        protected void OnProgressChanged(PercentCompleteEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        #endregion

    }
}
