using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBFramework.IO
{
    public interface ITargetDatabaseWriter
    {
        void Write(TargetDatabase database, Options options, string path);

        event MtdbProgressChangedEventHandler ProgressChanged;
    }
}
