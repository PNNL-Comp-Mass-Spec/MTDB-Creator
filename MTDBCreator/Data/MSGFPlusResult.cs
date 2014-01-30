using System;
using System.IO ; 
using System.Collections ;
using MTDBCreator.Data; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsXTandemResults.
	/// </summary>
	public class MsgfPlusResult: Target
	{
        public string Reference { get; set; }
        public short NumTrypticEnds { get; set; }
        public double Fdr { get; set; }
	}	
}
