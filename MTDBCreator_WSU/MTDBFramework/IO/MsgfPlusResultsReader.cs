//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MTDBFramework.Data;

//namespace MTDBFramework.IO
//{
//    public class MsgfPlusResultsReader : TableDataReaderBase<MsgfPlusResult>
//    {
//        private static string m_headerHitNum = "HitNum";
//        private static string m_headerScanNum = "ScanNum";
//        private static string m_headerChargeState = "ChargeState";
//        private static string m_headerMH = "MH";
//        private static string m_headerProtein = "Reference";
//        private static string m_headerPeptide = "Peptide";
//        private static string m_headerSpecProb = "MSGFDB_SpecEValue";
//        private static string m_headerNumTrypticEnds = "NTT";
//        private static string m_headerFdr = "QValue";
//        private static short m_columnHitNum = 0;
//        private static short m_columnScanNum = 1;
//        private static short m_columnChargeState = 2;
//        private static short m_columnMH = 3;
//        private static short m_columnReference = 4;
//        private static short m_columnPeptide = 5;
//        private static short m_columnNumTrypticEnds = 6;
//        private static short m_columnFdr = -1;
//        private static short m_columnSpecProb = 8;

//        public MsgfPlusResultsReader()
//        {
//            m_headerHitNum = m_headerHitNum.ToLower();
//            m_headerScanNum = m_headerScanNum.ToLower();
//            m_headerChargeState = m_headerChargeState.ToLower();
//            m_headerMH = m_headerMH.ToLower();
//            m_headerProtein = m_headerProtein.ToLower();
//            m_headerPeptide = m_headerPeptide.ToLower();
//            m_headerSpecProb = m_headerSpecProb.ToLower();
//            m_headerNumTrypticEnds = m_headerNumTrypticEnds.ToLower();
//            m_headerFdr = m_headerFdr.ToLower();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="colNames"></param>
//        protected override void SetHeaderColumns(string[] colNames)
//        {
//            for (int i = 0; i < colNames.Length; i++)
//            {
//                string columnName = colNames[i].ToLower();

//                if (columnName == m_headerHitNum)
//                    m_columnHitNum = (short)i;
//                else if (columnName == m_headerScanNum)
//                    m_columnScanNum = (short)i;
//                else if (columnName == m_headerChargeState || columnName == "charge")
//                    m_columnChargeState = (short)i;
//                else if (columnName == m_headerMH)
//                    m_columnMH = (short)i;
//                else if (columnName == m_headerProtein)
//                    m_columnReference = (short)i;
//                else if (columnName == m_headerPeptide)
//                    m_columnPeptide = (short)i;
//                else if (columnName == m_headerNumTrypticEnds)
//                    m_columnNumTrypticEnds = (short)i;
//                else if (columnName == m_headerSpecProb)
//                    m_columnSpecProb = (short)i;
//                else if (columnName == m_headerFdr)
//                    m_columnFdr = (short)i;
//            }
//        }

//        protected override MsgfPlusResult ProcessLine(string[] line)
//        {
//            MsgfPlusResult result = new MsgfPlusResult();
//            result.Scan = Convert.ToInt32(line[m_columnScanNum]);
//            result.Charge = Convert.ToInt16(line[m_columnChargeState]);
//            result.MonoisotopicMass = Convert.ToDouble(line[m_columnMH]);
//            result.Reference = line[m_columnReference];
//            result.Sequence = line[m_columnPeptide];
//            //result.CleanSequence = Target.CleanPeptide(result.Sequence);
//            result.SpectralProbability = Convert.ToDouble(line[m_columnSpecProb]);
//            result.NumTrypticEnds = Convert.ToInt16(line[m_columnNumTrypticEnds]);
//            if (m_columnFdr >= 0)
//            {
//                result.Fdr = Convert.ToDouble(line[m_columnFdr]);
//            }
//            return result;
//        }

//    }
//}
