using System;
using System.IO ; 
using System.Collections ;
using MTDBCreator.Data; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsXTandemResults.
	/// </summary>
	public class SequestResult: Target
	{
		public int      HitNum {get;set;} 		
		public short    ScanCount {get;set;} 
		public double   MH {get;set;}
		public double   XCorr {get;set;} 
		public double   DelCn {get;set;}
		public double   Sp {get;set;}
		public string   Reference {get;set;} 						
		public double   DelCn2 {get;set;} 
		public short    RankSp {get;set;} 
		public short    RankXc {get;set;} 
		public double   DelM {get;set;} 
		public double   XcRatio {get;set;} 
		public bool     PassFilt {get;set;} 
		public double   FScore {get;set;} 
		public double   MScore {get;set;} 
		public short    NumTrypticEnds {get;set;} 
		public double   Observed_net {get;set;} 		

		public const short MAX_CHARGE_FOR_FSCORE = 3 ;
		static readonly double [] consts = {0.646, -0.959, -1.460, -0.959, -0.959} ;
		static readonly double [] xcorrs = {5.49, 8.362, 9.933, 8.362, 8.362} ;
		static readonly double [] deltas = {4.643, 7.386, 11.149, 7.386, 7.386} ;
		static readonly double [] ranks = {-0.455, -0.194, -0.201, -0.194, -0.194} ;
		static readonly double [] massdiffs =  {-0.84, -0.314, -0.277, -0.314, -0.314} ;
		static readonly int [] max_pep_lens = {100, 15, 25, 50, 50} ;
		static readonly int [] num_frags = {2, 2, 4, 6, 6} ;

		public static double CalculatePeptideProphetDistriminantScore(SequestResult result)
		{
			short charge_ = result.Charge; 
			if (charge_ > MAX_CHARGE_FOR_FSCORE)
				charge_ = MAX_CHARGE_FOR_FSCORE ;
 
			double const_ = consts[charge_-1];
			double xcorr_p_wt_ = xcorrs[charge_-1];
			double delta_wt_ = deltas[charge_-1];
			double log_rank_wt_ = ranks[charge_-1];
			double abs_massd_wt_ = massdiffs[charge_-1];
			int max_pep_len_ = max_pep_lens[charge_-1];
			int num_frags_ = num_frags[charge_-1];

			int eff_pep_len = result.CleanSequence.Length ;
			if(eff_pep_len > max_pep_len_)
				eff_pep_len = max_pep_len_;
			double lg_xcorr = Math.Log(result.XcRatio) ; 
			double lg_eff_len = Math.Log((float)(1.0*eff_pep_len * num_frags_)) ; 
			double adjustedXCorr = lg_xcorr / lg_eff_len ;

			double tot = const_;
			tot += xcorr_p_wt_ * adjustedXCorr ;
			tot += delta_wt_ * result.DelCn2 ;
			double lg_val = Math.Log(1.0*result.RankSp) ; 
			tot += log_rank_wt_ * lg_val ;
			tot += abs_massd_wt_ * Math.Abs(result.DelM) ;	
			return tot ;
		}				
	}	
}
