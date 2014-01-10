#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public interface IAnalysisReader
    {
        Options ReaderOptions { get; set; }

        LcmsDataSet Read(string path);
    }
}