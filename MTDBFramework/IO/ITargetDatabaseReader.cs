using MTDBFramework.Database;

namespace MTDBFramework.IO
{
    public interface ITargetDatabaseReader
    {
        TargetDatabase Read(string path);
    }
}