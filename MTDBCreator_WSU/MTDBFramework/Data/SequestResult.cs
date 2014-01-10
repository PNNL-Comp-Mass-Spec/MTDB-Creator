#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
    public class SequestResult : Target
    {
        #region Private Fields

        private short m_ScanCount;
        private double m_XCorr;
        private double m_DelCn;
        private double m_Sp;
        private string m_Reference;
        private double m_DelCn2;
        private short m_RankSp;
        private short m_RankXc;
        private double m_DelM;
        private double m_XcRatio;
        private bool m_PassFilt;
        private double m_FScore;
        private double m_MScore;
        private short m_NumTrypticEnds;

        #endregion

        #region Public Properties

        public short ScanCount
        {
            get { return m_ScanCount; }
            set
            {
                m_ScanCount = value;
                OnPropertyChanged("ScanCount");
            }
        }

        public double XCorr
        {
            get { return m_XCorr; }
            set
            {
                m_XCorr = value;
                OnPropertyChanged("XCorr");
            }
        }

        public double DelCn
        {
            get { return m_DelCn; }
            set
            {
                m_DelCn = value;
                OnPropertyChanged("DelCn");
            }
        }

        public double Sp
        {
            get { return m_Sp; }
            set
            {
                m_Sp = value;
                OnPropertyChanged("Sp");
            }
        }

        public string Reference
        {
            get { return m_Reference; }
            set
            {
                m_Reference = value;
                OnPropertyChanged("Reference");
            }
        }

        public double DelCn2
        {
            get { return m_DelCn2; }
            set
            {
                m_DelCn2 = value;
                OnPropertyChanged("DelCn2");
            }
        }

        public short RankSp
        {
            get { return m_RankSp; }
            set
            {
                m_RankSp = value;
                OnPropertyChanged("RankSp");
            }
        }

        public short RankXc
        {
            get { return m_RankXc; }
            set
            {
                m_RankXc = value;
                OnPropertyChanged("RankXc");
            }
        }

        public double DelM
        {
            get { return m_DelM; }
            set
            {
                m_DelM = value;
                OnPropertyChanged("DelM");
            }
        }

        public double XcRatio
        {
            get { return m_XcRatio; }
            set
            {
                m_XcRatio = value; 
                OnPropertyChanged("XcRatio");
            }
        }

        public bool PassFilt
        {
            get { return m_PassFilt; }
            set
            {
                m_PassFilt = value;
                OnPropertyChanged("PassFilt");
            }
        }

        public double FScore
        {
            get { return m_FScore; }
            set
            {
                m_FScore = value;
                OnPropertyChanged("FScore");
            }
        }

        public double MScore
        {
            get { return m_MScore; }
            set
            {
                m_MScore = value;
                OnPropertyChanged("MScore");
            }
        }

        public short NumTrypticEnds
        {
            get { return m_NumTrypticEnds; }
            set
            {
                m_NumTrypticEnds = value;
                OnPropertyChanged("NumTrypticEnds");
            }
        }

        #endregion

        private const short MaxChargeForScore = 3;

        private static readonly double[] Consts = { 0.646, -0.959, -1.460, -0.959, -0.959 };
        private static readonly double[] XCorrs = { 5.49, 8.362, 9.933, 8.362, 8.362 };
        private static readonly double[] Deltas = { 4.643, 7.386, 11.149, 7.386, 7.386 };
        private static readonly double[] Ranks = { -0.455, -0.194, -0.201, -0.194, -0.194 };
        private static readonly double[] MassDiffs = { -0.84, -0.314, -0.277, -0.314, -0.314 };
        private static readonly int[] MaxPeptideLens = { 100, 15, 25, 50, 50 };
        private static readonly int[] NumFrags = { 2, 2, 4, 6, 6 };

        public static double CalculatePeptideProphetDistriminantScore(SequestResult result)
        {
            short charge = result.Charge;

            if (charge > MaxChargeForScore)
            {
                charge = MaxChargeForScore;
            }

            double constant = Consts[charge - 1];
            double xcorr = XCorrs[charge - 1];
            double delta = Deltas[charge - 1];
            double logRank = Ranks[charge - 1];
            double absMassDiff = MassDiffs[charge - 1];
            int maxPeptideLength = MaxPeptideLens[charge - 1];
            int numFrags = NumFrags[charge - 1];

            int cleanPeptideLength = result.CleanPeptide.Length;

            if (cleanPeptideLength > maxPeptideLength)
            {
                cleanPeptideLength = maxPeptideLength;
            }

            double lgXcorr = Math.Log(result.XcRatio);
            double lgCleanPeptideLength = Math.Log((float)(1.0 * cleanPeptideLength * numFrags));
            double adjustedXCorr = lgXcorr / lgCleanPeptideLength;

            double tot = constant;
            tot += xcorr * adjustedXCorr;
            tot += delta * result.DelCn2;
            double lgRankSp = Math.Log(1.0 * result.RankSp);
            tot += logRank * lgRankSp;
            tot += absMassDiff * Math.Abs(result.DelM);

            return tot;
        }
    }
}