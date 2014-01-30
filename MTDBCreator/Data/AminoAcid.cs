using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public class AminoAcid
    {
        public AminoAcid(string name, double mass)
        {
            Symbol  = name;
            Mass    = mass;
        }

        public string Symbol { get; private set; }
        public double Mass { get; private set; }
    }
}
