using System;
using System.Collections ; 
using System.IO ;
using System.Collections.Generic;
using MTDBCreator.IO;
using MTDBCreator.Data; 

namespace MTDBCreator
{

    /// <summary>
    /// Maps data from the sequence protein map reader
    /// </summary>
    public class SequenceToProteinMapReader : FileReaderBase<SequenceToProteinMap>
    {        
		private const string mstrHeader_unique_seq_id           = "Unique_Seq_ID" ; 
		private const string mstrHeader_cleavage_state          = "Cleavage_State"; 
		private const string mstrHeader_terminus_state          = "Terminus_State" ; 
		private const string mstrHeader_protein_name            = "Protein_Name" ;
		private const string mstrHeader_protein_e_value         = "Protein_Expectation_Value_Log(e)" ;
		private const string mstrHeader_protein_intensity_log   = "Protein_Intensity_Log(I)" ;
        private const int   UNDEFINED                           = -100; 
        private static  short mshortColNum_unique_seq_id          = 0;
        private static  short mshortColNum_cleavage_state         = 1;
        private static  short mshortColNum_terminus_state         = 2;
        private static  short mshortColNum_protein_name           = 3;
        private static  short mshortColNum_protein_e_value        = 4;
        private static  short mshortColNum_protein_intensity_log  = 5;

        protected override SequenceToProteinMap ProcessLine(string[] columns)
        {            
            SequenceToProteinMap map    = new SequenceToProteinMap(); 
			map.UniqueSequenceId        = Convert.ToInt32(columns[mshortColNum_unique_seq_id]);
            map.CleavageState           = Convert.ToInt16(columns[mshortColNum_cleavage_state]);
            map.TerminusState           = Convert.ToInt16(columns[mshortColNum_terminus_state]);
            map.ProteinName             = columns[mshortColNum_protein_name];

            double eValue     = UNDEFINED;
			if (columns[mshortColNum_protein_e_value] != null && columns[mshortColNum_protein_e_value].Length > 0)
                eValue        = Convert.ToDouble(columns[mshortColNum_protein_e_value]);
            map.ProteinEValue = eValue;

            double logValue = UNDEFINED;
			if (columns[mshortColNum_protein_intensity_log] != null && columns[mshortColNum_protein_intensity_log].Length > 0)
                logValue    = Convert.ToDouble(columns[mshortColNum_protein_intensity_log]);
            map.ProteinIntensityLog = logValue;

            return map;
        }
        /// <summary>
        /// Sets the header data for columnar information
        /// </summary>
        /// <param name="headers"></param>
        protected override void SetHeaderColumns(string [] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                switch (headers[i])
                {
                    case mstrHeader_unique_seq_id:
                        mshortColNum_unique_seq_id = (short)i;
                        break;
                    case mstrHeader_cleavage_state:
                        mshortColNum_cleavage_state = (short)i;
                        break;
                    case mstrHeader_terminus_state:
                        mshortColNum_terminus_state = (short)i;
                        break;
                    case mstrHeader_protein_name:
                        mshortColNum_protein_name = (short)i;
                        break;
                    case mstrHeader_protein_e_value:
                        mshortColNum_protein_e_value = (short)i;
                        break;
                    case mstrHeader_protein_intensity_log:
                        mshortColNum_protein_intensity_log = (short)i;
                        break;
                    default:
                        break;
                }
            }
        }
        
	}
}
