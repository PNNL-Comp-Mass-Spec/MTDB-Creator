using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;
using System.IO;

namespace MTDBCreator.IO
{
    public class SequestMetaDataReader: IAnalysisMetaDataReader 
    {
        #region IAnalysisMetaDataReader Members
        public List<Analysis> ReadMetaData(string path)
        {
            List<Analysis> data  = new List<Analysis>();
            string name          = Path.GetFileName(path);
            name                 = name.Replace("_syn.txt", "");
            Analysis newAnalysis = new Analysis();
            newAnalysis.Tool     = SupportedTools.Sequest;
            newAnalysis.Name     = name;
            newAnalysis.FilePath = Path.GetDirectoryName(path);
            data.Add(newAnalysis);
            return data;
        }
        #endregion
    }
}
