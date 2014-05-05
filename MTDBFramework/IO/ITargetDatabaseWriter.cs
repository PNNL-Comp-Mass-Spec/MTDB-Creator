using MTDBFramework.Data;
using MTDBFramework.Database;

namespace MTDBFramework.IO
{
    public interface ITargetDatabaseWriter
    {
        void Write(TargetDatabase database, Options options, string path);
    }
}
