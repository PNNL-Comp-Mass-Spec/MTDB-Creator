using System.Collections.Generic;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.IO
{
    /// <summary>
    /// Interface for Input file readers
    /// </summary>
    public interface ITargetDatabaseReader
    {
        /// <summary>
        /// Read function - read and process a file
        /// </summary>
        /// <param name="path">File to read and process</param>
        /// <returns></returns>
        IEnumerable<LcmsDataSet> Read(string path);
    }
}