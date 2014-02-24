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
        public string Path { get; set; }
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
