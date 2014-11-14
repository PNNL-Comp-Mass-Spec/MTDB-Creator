namespace MTDBFramework.IO
{
    /// <summary>
    /// Interface for converting text file to a database file
    /// </summary>
    public interface ITextToDbConverter
    {
        void Convert(string path);
    }
}
