using System;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsMassTag.
	/// </summary>
	public class clsMassTag
	{
		public int mint_mass_tag_id ; 
		public string mstr_peptide ;
		public string mstr_clean_peptide ;
		public double mdbl_monoisotopic_mass ; 
		public short mshort_multiple_proteins ; 
		public int mint_number_of_peptides ; 
		public int mint_peptide_obs_count_passing_filter ; 
		public double mdbl_high_normalized_score ; 
		public double mdbl_high_peptide_prophet_probability ; 
		public double mdbl_min_log_evalue ; 
		public short mshort_mod_count ; 
		public string mstr_mod_description ; 
		public short mshort_PMT_Quality_Score ; 

		public double mdbl_min_ganet ; 
		public double mdbl_max_ganet ; 
		public double mdbl_agv_ganet ; 
		public double mshort_cnt_ganet ; 
		public double mdbl_std_ganet ; 
		public double mdbl_stderr_ganet ; 
		public double mdbl_predicted_net ; 

		public clsMassTag()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
