using System;
using System.IO;
using System.Linq;

namespace MTDBFramework.IO
{    
    public static class DatasetPathUtility
    {        
        public static string CleanPath(string path)
        {
            var fiFile = new FileInfo(path);

            var name = fiFile.Name;

            // Remove the appropriate extension ("_syn.txt" checked last to prevent potential overlap)
            // Strips extra information from the path name and returns the dataset name itself
            string[] toRemove = { "_msgfdb_syn.txt", "msalign_syn.txt", "_xt.txt", "_syn.txt", "msgfplus.mzid", ".mzid" };
            var splitString = fiFile.Name.Split(toRemove, StringSplitOptions.RemoveEmptyEntries);

            if (splitString.Length > 0)
            {
                name = splitString[0];
            }

            if (name.Contains('.'))
            {
                name = Path.GetFileNameWithoutExtension(name);
            }

            return name;
        }
    }
}