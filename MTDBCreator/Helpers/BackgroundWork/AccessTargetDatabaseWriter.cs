using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.IO;
using MTDBFramework.UI;

namespace MTDBCreator.Helpers.BackgroundWork
{
    class AccessTargetDatabaseWriter : ITargetDatabaseWriter
    {
        public void Write(MTDBFramework.Database.TargetDatabase database, MTDBFramework.Data.Options options, string path)
        {
            throw new NotImplementedException();
        }


        public event MtdbProgressChangedEventHandler ProgressChanged;

        protected void OnProgressChanged(MtdbProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }
    }
}
