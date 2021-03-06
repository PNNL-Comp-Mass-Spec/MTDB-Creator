﻿#region Namespaces

using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.IO
{
    /// <summary>
    /// PHRP file reader interface
    /// </summary>
    public interface IPhrpReader
    {
        /// <summary>
        /// Options
        /// </summary>
        Options ReaderOptions { get; set; }

        /// <summary>
        /// Read and process a file
        /// </summary>
        /// <param name="path">File to read and process</param>
        /// <returns></returns>
        LcmsDataSet Read(string path);
    }
}