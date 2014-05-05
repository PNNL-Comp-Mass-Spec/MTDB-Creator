using System;
using System.Collections ; 
using System.IO ;
using System.Collections.Generic; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsSeqInfo.
	/// </summary>
    public class Sequence
	{		        		
		public Sequence(int sequenceId, short modificationCount, string description,  double monoMass)
		{			
			Id                      = sequenceId;
			ModificationCount       = modificationCount;
            ModificationDescription = description;
			MonoisotopicMass        = monoMass;
		}

        public Sequence()
        {

        }
        
        public int      Id          { get; set; }
        public short    ModificationCount         { get; set; }
        public string   ModificationDescription   { get; set; }
        public double   MonoisotopicMass          { get; set; }

        public override string ToString()
        {
            return ModificationDescription;
        }
	}
}
