using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using PHRPReader;

namespace MTDBFramework.IO
{
    public abstract class PHRPReaderBase : IPhrpReader
    {
        protected readonly Dictionary<string, TargetDataSet> mDatasetCache;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PHRPReaderBase()
        {
            mDatasetCache = new Dictionary<string, TargetDataSet>(StringComparer.CurrentCultureIgnoreCase);
        }

        public Data.Options ReaderOptions { get; set; }

        public abstract Data.LcmsDataSet Read(string path);

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
                }
            }

        }

        protected void UpdateProgress(float percentComplete)
        {
            OnProgressChanged(new PercentCompleteEventArgs(percentComplete));
        }

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
