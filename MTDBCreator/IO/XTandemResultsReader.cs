using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{

    public class XTandemResultsReader : FileReaderBase<XTandemResult>
    {
        public const string mstrDefaultHeader = "Result_ID	Group_ID	Scan	Charge	Peptide_MH	Peptide_Hyperscore	Peptide_Expectation_Value_Log(e)	Multiple_Protein_Count	Peptide_Sequence	DeltaCn2	y_score	y_ions	b_score	b_ions	Delta_Mass	Peptide_Intensity_Log(I)";

        private static string mstrHeader_result_id = "";
        private static string mstrHeader_group_id = "";
        private static string mstrHeader_scan = "";
        private static string mstrHeader_charge = "";
        private static string mstrHeader_peptide_mh = "";
        private static string mstrHeader_peptide_hyperscore = "";
        private static string mstrHeader_log_peptide_e_value = "";
        private static string mstrHeader_multi_protein_count = "";
        private static string mstrHeader_peptide_sequence = "";
        private static string mstrHeader_deltaCN2 = "";
        private static string mstrHeader_y_score = "";
        private static string mstrHeader_num_y_ions = "";
        private static string mstrHeader_b_score = "";
        private static string mstrHeader_num_b_ions = "";
        private static string mstrHeader_delta_mass = "";
        private static string mstrHeader_log_intensity = "";


        private static short mshortColNum_result_id = 0;
        private static short mshortColNum_group_id = 1;
        private static short mshortColNum_scan = 2;
        private static short mshortColNum_charge = 3;
        private static short mshortColNum_peptide_mh = 4;
        private static short mshortColNum_peptide_hyperscore = 5;
        private static short mshortColNum_log_peptide_e_value = 6;
        private static short mshortColNum_multi_protein_count = 7;
        private static short mshortColNum_peptide_sequence = 8;
        private static short mshortColNum_deltaCN2 = 9;
        private static short mshortColNum_y_score = 10;
        private static short mshortColNum_num_y_ions = 11;
        private static short mshortColNum_b_score = 12;
        private static short mshortColNum_num_b_ions = 13;
        private static short mshortColNum_delta_mass = 14;
        private static short mshortColNum_log_intensity = 15;

        public XTandemResultsReader()
        {
            SetHeaderNames();

        }

        public static void SetHeaderNames()
        {
            string[] colNames               = mstrDefaultHeader.Split(new char[] { '\t' });
            mstrHeader_result_id            = colNames[mshortColNum_result_id];
            mstrHeader_group_id             = colNames[mshortColNum_group_id];
            mstrHeader_scan                 = colNames[mshortColNum_scan];
            mstrHeader_charge               = colNames[mshortColNum_charge];
            mstrHeader_peptide_mh           = colNames[mshortColNum_peptide_mh];
            mstrHeader_peptide_hyperscore   = colNames[mshortColNum_peptide_hyperscore];
            mstrHeader_log_peptide_e_value  = colNames[mshortColNum_log_peptide_e_value];
            mstrHeader_multi_protein_count  = colNames[mshortColNum_multi_protein_count];
            mstrHeader_peptide_sequence     = colNames[mshortColNum_peptide_sequence];
            mstrHeader_deltaCN2             = colNames[mshortColNum_deltaCN2];
            mstrHeader_y_score              = colNames[mshortColNum_y_score];
            mstrHeader_num_y_ions           = colNames[mshortColNum_num_y_ions];
            mstrHeader_b_score              = colNames[mshortColNum_b_score];
            mstrHeader_num_b_ions           = colNames[mshortColNum_num_b_ions];
            mstrHeader_delta_mass           = colNames[mshortColNum_delta_mass];
            mstrHeader_log_intensity        = colNames[mshortColNum_log_intensity];
        }

        protected override XTandemResult ProcessLine(string[] line)
        {
            XTandemResult result        = new XTandemResult();
            result.ResultId             = Convert.ToInt32(line[mshortColNum_result_id]);
            result.GroupId              = Convert.ToInt32(line[mshortColNum_group_id]);
            result.Scan                 = Convert.ToInt32(line[mshortColNum_scan]);
            result.Charge               = Convert.ToInt16(line[mshortColNum_charge]);
            result.PeptideMh            = Convert.ToDouble(line[mshortColNum_peptide_mh]);
            result.PeptideHyperscore    = Convert.ToDouble(line[mshortColNum_peptide_hyperscore]);
            result.LogPeptideEValue     = Convert.ToDouble(line[mshortColNum_log_peptide_e_value]);
            result.MultiProteinCount    = Convert.ToInt16(line[mshortColNum_multi_protein_count]);
            result.Sequence             = line[mshortColNum_peptide_sequence];
            result.CleanSequence        = Target.CleanPeptide(result.Sequence);
            result.DeltaCn2             = Convert.ToDouble(line[mshortColNum_deltaCN2]);
            result.YScore               = Convert.ToDouble(line[mshortColNum_y_score]);
            result.NumberYIons          = Convert.ToInt16(line[mshortColNum_num_y_ions]);
            result.BScore               = Convert.ToDouble(line[mshortColNum_b_score]);
            result.NumberBIons          = Convert.ToInt16(line[mshortColNum_num_b_ions]);
            result.DeltaMass            = Convert.ToDouble(line[mshortColNum_delta_mass]);
            result.LogIntensity         = Convert.ToDouble(line[mshortColNum_log_intensity]);
            result.TrypticState         = Target.CalculateTrypticState(result.Sequence);

            // I dont know where these magic numbers came from.  This is a Deep thing...            
            double highNorm = 0;
            if (result.Charge == 1)
                highNorm = 0.082 * result.PeptideHyperscore;
            else if (result.Charge == 2)
                highNorm = 0.085 * result.PeptideHyperscore;
            else
                highNorm = 0.0872 * result.PeptideHyperscore;

            result.HighNormalizedScore = highNorm;

            return result;
        }
        protected override void SetHeaderColumns(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == mstrHeader_result_id)
                    mshortColNum_result_id = (short)i;
                else if (headers[i] == mstrHeader_group_id)
                    mshortColNum_group_id = (short)i;
                else if (headers[i] == mstrHeader_scan)
                    mshortColNum_scan = (short)i;
                else if (headers[i] == mstrHeader_charge)
                    mshortColNum_charge = (short)i;
                else if (headers[i] == mstrHeader_peptide_mh)
                    mshortColNum_peptide_mh = (short)i;
                else if (headers[i] == mstrHeader_peptide_hyperscore)
                    mshortColNum_peptide_hyperscore = (short)i;
                else if (headers[i] == mstrHeader_log_peptide_e_value)
                    mshortColNum_log_peptide_e_value = (short)i;
                else if (headers[i] == mstrHeader_multi_protein_count)
                    mshortColNum_multi_protein_count = (short)i;
                else if (headers[i] == mstrHeader_peptide_sequence)
                    mshortColNum_peptide_sequence = (short)i;
                else if (headers[i] == mstrHeader_deltaCN2)
                    mshortColNum_deltaCN2 = (short)i;
                else if (headers[i] == mstrHeader_y_score)
                    mshortColNum_y_score = (short)i;
                else if (headers[i] == mstrHeader_num_y_ions)
                    mshortColNum_num_y_ions = (short)i;
                else if (headers[i] == mstrHeader_b_score)
                    mshortColNum_b_score = (short)i;
                else if (headers[i] == mstrHeader_num_b_ions)
                    mshortColNum_num_b_ions = (short)i;
                else if (headers[i] == mstrHeader_delta_mass)
                    mshortColNum_delta_mass = (short)i;
                else if (headers[i] == mstrHeader_log_intensity)
                    mshortColNum_log_intensity = (short)i;
            }
        }
    }
}
