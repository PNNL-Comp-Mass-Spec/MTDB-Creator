using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Interface for MTDB database writing and monitoring
    /// </summary>
    public interface ITargetDatabaseWriter
    {
        /// <summary>
        /// Write the data to the MTDB database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="options"></param>
        /// <param name="path"></param>
        void Write(TargetDatabase database, Options options, string path);

        /// <summary>
        /// Event Handler
        /// </summary>
        event MtdbProgressChangedEventHandler ProgressChanged;
    }
}
