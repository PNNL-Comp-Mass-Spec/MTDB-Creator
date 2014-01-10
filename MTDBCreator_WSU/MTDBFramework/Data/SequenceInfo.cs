using System.Collections.Generic;

namespace MTDBFramework.Data
{
    public class SequenceInfo
    {
        public int Id { get; set; }
        public short ModificationCount { get; set; }
        public string ModificationDescription { get; set; }
        public double MonoisotopicMass { get; set; }

        public override string ToString()
        {
            return this.ModificationDescription;
        }
    }
}