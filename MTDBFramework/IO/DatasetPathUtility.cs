using System;
using System.Linq;

namespace MTDBFramework.IO
{
    public static class DatasetPathUtility
    {
        public static string CleanPath(string path)
        {
            var name = "";

            // Remove the appropriate extention ("_syn.txt" checked last to prevent potential overlap)
            // Strips extra information from the path name and returns the dataset name itself
            string[] toRemove = { "_msgfdb_syn.txt", "msalign_syn.txt", "_xt.txt", "_syn.txt" };
            var splitString = path.Split('\\').Last().Split(toRemove, StringSplitOptions.RemoveEmptyEntries);

            if (splitString.Length >= 1)
            {
                name = splitString[0];
            }

            return name;
        }
    }
}