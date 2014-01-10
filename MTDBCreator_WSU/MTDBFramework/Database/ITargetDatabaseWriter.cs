using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public interface ITargetDatabaseWriter
    {
        void Write(TargetDatabase database, Options options, string path);
    }
}
