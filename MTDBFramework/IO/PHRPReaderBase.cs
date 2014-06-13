using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;
using MTDBFramework.UI;
using PHRPReader;

namespace MTDBFramework.IO
{
    public abstract class PHRPReaderBase : IPhrpReader
    {

        public Data.Options ReaderOptions { get; set; }

        public abstract Data.LcmsDataSet Read(string path);

        protected static void StoreProteinInfo(clsPHRPReader reader, Dictionary<string, ProteinInformation> proteinInfos, Evidence result)
        {
            foreach (var p in reader.CurrentPSM.ProteinDetails)
            {
                string proteinName = p.Value.ProteinName;

                var protein = new ProteinInformation
                {
                    ProteinName = p.Value.ProteinName,
                    CleavageState = p.Value.CleavageState,
                    TerminusState = p.Value.TerminusState,
                    ResidueStart = p.Value.ResidueStart,
                    ResidueEnd = p.Value.ResidueEnd
                };

                if (!proteinInfos.ContainsKey(proteinName))
                {
                    proteinInfos.Add(proteinName, protein);
                }

                result.Proteins.Add(protein);
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
