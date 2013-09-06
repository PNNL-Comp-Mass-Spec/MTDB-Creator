using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{
    public class SequestResultReader: FileReaderBase<SequestResult>
    {
        public static string mstrDefaultHeader = "HitNum	ScanNum	ScanCount	ChargeState	MH	XCorr	DelCn	Sp	Reference	MultiProtein	Peptide	DelCn2	RankSp	RankXc	DelM	XcRatio	PassFilt	MScore	NumTrypticEnds"; 

        private static string mstrHeader_HitNum = "";
        private static string mstrHeader_ScanNum = "";
        private static string mstrHeader_ScanCount = "";
        private static string mstrHeader_ChargeState = "";
        private static string mstrHeader_MH = "";
        private static string mstrHeader_XCorr = "";
        private static string mstrHeader_DelCn = "";
        private static string mstrHeader_Sp = "";
        private static string mstrHeader_Reference = "";
        private static string mstrHeader_MultiProtein = "";
        private static string mstrHeader_Peptide = "";
        private static string mstrHeader_DelCn2 = "";
        private static string mstrHeader_RankSp = "";
        private static string mstrHeader_RankXc = "";
        private static string mstrHeader_DelM = "";
        private static string mstrHeader_XcRatio = "";
        private static string mstrHeader_PassFilt = "";
        private static string mstrHeader_MScore = "";
        private static string mstrHeader_NumTrypticEnds = "";

        private static short mshortColNum_HitNum = 0;
        private static short mshortColNum_ScanNum = 1;
        private static short mshortColNum_ScanCount = 2;
        private static short mshortColNum_ChargeState = 3;
        private static short mshortColNum_MH = 4;
        private static short mshortColNum_XCorr = 5;
        private static short mshortColNum_DelCn = 6;
        private static short mshortColNum_Sp = 7;
        private static short mshortColNum_Reference = 8;
        private static short mshortColNum_MultiProtein = 9;
        private static short mshortColNum_Peptide = 10;
        private static short mshortColNum_DelCn2 = 11;
        private static short mshortColNum_RankSp = 12;
        private static short mshortColNum_RankXc = 13;
        private static short mshortColNum_DelM = 14;
        private static short mshortColNum_XcRatio = 15;
        private static short mshortColNum_PassFilt = 16;
        private static short mshortColNum_MScore = 17;
        private static short mshortColNum_NumTrypticEnds = 18;

        public SequestResultReader()
        {
            SetHeaderNames();
        }

        public static void SetHeaderNames()
        {
            string[] colNames = mstrDefaultHeader.Split(new char[] { '\t' });

            mstrHeader_HitNum = colNames[mshortColNum_HitNum];
            mstrHeader_ScanNum = colNames[mshortColNum_ScanNum];
            mstrHeader_ScanCount = colNames[mshortColNum_ScanCount];
            mstrHeader_ChargeState = colNames[mshortColNum_ChargeState];
            mstrHeader_MH = colNames[mshortColNum_MH];
            mstrHeader_XCorr = colNames[mshortColNum_XCorr];
            mstrHeader_DelCn = colNames[mshortColNum_DelCn];
            mstrHeader_Sp = colNames[mshortColNum_Sp];
            mstrHeader_Reference = colNames[mshortColNum_Reference];
            mstrHeader_MultiProtein = colNames[mshortColNum_MultiProtein];
            mstrHeader_Peptide = colNames[mshortColNum_Peptide];
            mstrHeader_DelCn2 = colNames[mshortColNum_DelCn2];
            mstrHeader_RankSp = colNames[mshortColNum_RankSp];
            mstrHeader_RankXc = colNames[mshortColNum_RankXc];
            mstrHeader_DelM = colNames[mshortColNum_DelM];
            mstrHeader_XcRatio = colNames[mshortColNum_XcRatio];
            mstrHeader_PassFilt = colNames[mshortColNum_PassFilt];
            mstrHeader_MScore = colNames[mshortColNum_MScore];
            mstrHeader_NumTrypticEnds = colNames[mshortColNum_NumTrypticEnds];
        }

        protected override void SetHeaderColumns(string[] colNames)
        {            
            for (int i = 0; i < colNames.Length; i++)
            {
                if (colNames[i] == mstrHeader_HitNum)
                    mshortColNum_HitNum = (short)i;
                else if (colNames[i] == mstrHeader_ScanNum)
                    mshortColNum_ScanNum = (short)i;
                else if (colNames[i] == mstrHeader_ScanCount)
                    mshortColNum_ScanCount = (short)i;
                else if (colNames[i] == mstrHeader_ChargeState)
                    mshortColNum_ChargeState = (short)i;
                else if (colNames[i] == mstrHeader_MH)
                    mshortColNum_MH = (short)i;
                else if (colNames[i] == mstrHeader_XCorr)
                    mshortColNum_XCorr = (short)i;
                else if (colNames[i] == mstrHeader_DelCn)
                    mshortColNum_DelCn = (short)i;
                else if (colNames[i] == mstrHeader_Sp)
                    mshortColNum_Sp = (short)i;
                else if (colNames[i] == mstrHeader_Reference)
                    mshortColNum_Reference = (short)i;
                else if (colNames[i] == mstrHeader_MultiProtein)
                    mshortColNum_MultiProtein = (short)i;
                else if (colNames[i] == mstrHeader_Peptide)
                    mshortColNum_Peptide = (short)i;
                else if (colNames[i] == mstrHeader_DelCn2)
                    mshortColNum_DelCn2 = (short)i;
                else if (colNames[i] == mstrHeader_RankSp)
                    mshortColNum_RankSp = (short)i;
                else if (colNames[i] == mstrHeader_RankXc)
                    mshortColNum_RankXc = (short)i;
                else if (colNames[i] == mstrHeader_DelM)
                    mshortColNum_DelM = (short)i;
                else if (colNames[i] == mstrHeader_XcRatio)
                    mshortColNum_XcRatio = (short)i;
                else if (colNames[i] == mstrHeader_PassFilt)
                    mshortColNum_PassFilt = (short)i;
                else if (colNames[i] == mstrHeader_MScore)
                    mshortColNum_MScore = (short)i;
                else if (colNames[i] == mstrHeader_NumTrypticEnds)
                    mshortColNum_NumTrypticEnds = (short)i;
            }
        }

        protected override SequestResult  ProcessLine(string[] line)
        {
            SequestResult result    = new SequestResult();            
            result.HitNum           = Convert.ToInt32(line[mshortColNum_HitNum]);
            result.Scan             = Convert.ToInt32(line[mshortColNum_ScanNum]);
            result.ScanCount        = Convert.ToInt16(line[mshortColNum_ScanCount]);
            result.Charge           = Convert.ToInt16(line[mshortColNum_ChargeState]);
            result.MH               = Convert.ToDouble(line[mshortColNum_MH]);
            result.XCorr            = Convert.ToDouble(line[mshortColNum_XCorr]);
            result.DelCn            = Convert.ToDouble(line[mshortColNum_DelCn]);
            result.Sp               = Convert.ToDouble(line[mshortColNum_Sp]);
            result.Reference        = line[mshortColNum_Reference];
            result.MultiProteinCount = Convert.ToInt16(line[mshortColNum_MultiProtein]);
            result.Sequence         = line[mshortColNum_Peptide];
            result.CleanSequence    = Target.CleanPeptide(result.Sequence);
            result.DelCn2           = Convert.ToDouble(line[mshortColNum_DelCn2]);
            result.RankSp           = Convert.ToInt16(line[mshortColNum_RankSp]);
            result.RankXc           = Convert.ToInt16(line[mshortColNum_RankXc]);
            result.DelM             = Convert.ToDouble(line[mshortColNum_DelM]);
            result.XcRatio          = Convert.ToDouble(line[mshortColNum_XcRatio]);
            result.PassFilt         = Convert.ToBoolean(Convert.ToInt16(line[mshortColNum_PassFilt]));
            result.MScore           = Convert.ToDouble(line[mshortColNum_MScore]);
            result.NumTrypticEnds   = Convert.ToInt16(line[mshortColNum_NumTrypticEnds]);
            result.FScore           = SequestResult.CalculatePeptideProphetDistriminantScore(result);

            result.MonoisotopicMass = result.MH;      
           

            return result;
        }
        
    }
}
