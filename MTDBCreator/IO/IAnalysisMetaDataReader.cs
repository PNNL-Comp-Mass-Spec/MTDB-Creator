using System.Collections.Generic;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{
    public interface IAnalysisMetaDataReader
    {
        List<Analysis> ReadMetaData(string path);
    }
}
