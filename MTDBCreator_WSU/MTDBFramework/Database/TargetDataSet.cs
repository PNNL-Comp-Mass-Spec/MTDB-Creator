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
        //Added Target List
    }
}
