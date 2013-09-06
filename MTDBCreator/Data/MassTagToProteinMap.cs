using System;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsMassTagToProteinMap.
	/// </summary>
	public class MassTagToProteinMap
	{
		public int mint_mass_tag_id ; 
		public int mint_ref_id ; 
		public short mshort_cleavage_state ; 
		public short mshort_terminus_state ; 
		public short mshort_missed_cleavage_count ; 

		public MassTagToProteinMap()
		{
		}
	}
}
