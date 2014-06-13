using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.UI;

namespace MTDBFramework.IO
{
    public abstract class PHRPReaderBase : IPhrpReader
    {

        public Data.Options ReaderOptions { get; set; }

        public abstract Data.LcmsDataSet Read(string path);

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
