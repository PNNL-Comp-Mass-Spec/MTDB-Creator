using MTDBFramework.Data;
using MTDBFramework.Database;
using System.Collections.Generic;

namespace MTDBFramework.IO
{
    public interface ITargetDatabaseReader
    {
        IEnumerable<LcmsDataSet> Read(string path);
    }
}