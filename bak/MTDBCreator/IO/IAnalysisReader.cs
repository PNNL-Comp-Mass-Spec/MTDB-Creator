using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{
    public interface IAnalysisReader
    {
        /// <summary>
        /// Reads the path provided for files.
        /// </summary>
        /// <param name="path"></param>
        Analysis Read(string path, string name);        
    }
}
