#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public interface IPHRPReader
    {
        Options ReaderOptions { get; set; }

        LcmsDataSet Read(string path);
    }
}