using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Database
{
    public class Metadata
    {
        public Metadata()
        {
            
        }

        public int SequenceID { get; set; }
        public string Sequence { get; set; }
        public string CleanPeptide { get; set; }
    }
}
