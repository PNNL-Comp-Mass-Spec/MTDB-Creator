using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class TargetPeptideInfo
    {
        public TargetPeptideInfo()
        {
        }
        public int Id { get; set; }
        public string PeptideInfoCleanPeptide { get; set; }
        public string PeptideInfoSequence { get; set; }
        //Added Target List
        public IList<Target> Targets { get; set; }
    }
}
