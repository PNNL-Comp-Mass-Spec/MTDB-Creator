using System;
using System.IO ; 
using System.Collections ;
using MTDBCreator.Data; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsXTandemResults.
	/// </summary>
    public class XTandemResult : Target
	{        
		public double   BScore {get;set;} 
        public double   DeltaCn2 {get;set;}  
		public double   DeltaMass {get;set;} 
		public int      GroupId {get;set;} 		
		public double   LogIntensity {get;set;} 
		public short    NumberYIons {get;set;} 
		public short    NumberBIons{get;set;} 
		public double   ObservedNet{get;set;} 
		public double   PeptideMh{get;set;}
		public double   PeptideHyperscore {get;set;} 
        public int      ResultId { get; set; }
		public short    TrypticState{get;set;} 	
		public double   YScore {get;set;} 				
	}
}
