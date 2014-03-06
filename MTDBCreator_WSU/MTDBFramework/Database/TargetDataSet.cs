using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class TargetDataSet
    {
        public TargetDataSet()
        {

        }
        public int Id { get; set; }

        // Path only used to get the name of the data set for the database
        public string Path { get; set; }
        
        // Remove the appropriate extention ("_syn.txt" checked last to prevent potential overlap)
        // Strips extra information from the path name and returns the dataset name itself
        public string Name 
        {
            get 
            { 
                string[] toRemove = {"_msgfdb_syn.txt", "msalign_syn.txt", "_xt.txt", "_syn.txt" };
                return Path.Split('\\').Last().Split(toRemove, StringSplitOptions.RemoveEmptyEntries)[0];
            } 
            set
            {
                string[] toRemove = { "_msgfdb_syn.txt", "msalign_syn.txt", "_xt.txt", "_syn.txt" };
                Name = value.Split('\\').Last().Split(toRemove, StringSplitOptions.RemoveEmptyEntries)[0]; 
            } 
        }
        //Added Target List
    }
}
