using MTDBFramework.Data;
using MTDBFramework.Database;
using System.Collections.Generic;

namespace MTDBFramework.IO
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