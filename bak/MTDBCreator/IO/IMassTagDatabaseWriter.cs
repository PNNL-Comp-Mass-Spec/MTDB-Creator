using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{
    /// <summary>
    /// Interface for writing a mass tag database to a source.
    /// </summary>
    public interface IMassTagDatabaseWriter
    {        
        // operation so we dont couple writing with filtering.  I just dont want to filter the database.
        /// <summary>
        /// Write the database contents to the path.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="path"></param>
        void WriteDatabase(MassTagDatabase database, string path);
    }
}
