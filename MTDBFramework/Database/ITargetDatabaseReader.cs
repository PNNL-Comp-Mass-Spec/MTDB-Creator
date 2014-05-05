namespace MTDBFramework.Database
{
    public interface ITargetDatabaseReader
    {
        TargetDatabase Read(string path);
    }
}