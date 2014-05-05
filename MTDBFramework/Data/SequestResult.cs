#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
    public class SequestResult : Evidence
    {
        #region Private Fields

        private short m_scanCount;
        private double m_xCorr;
        private double m_delCn;
        private double m_sp;
        private string m_reference;
        private double m_delCn2;
        private short m_rankSp;
        private short m_rankXc;
        private double m_xcRatio;
        private bool m_passFilt;
        private double m_fScore;
        private double m_mScore;
        private short m_numTrypticEnds;

        #endregion

        #region Public Properties

        public short ScanCount
        {
            get { return m_scanCount; }
            set
            {
                m_scanCount = value;
                OnPropertyChanged("ScanCount");
            }
        }

        public double XCorr
        {
            get { return m_xCorr; }
            set
            {
                m_xCorr = value;
                OnPropertyChanged("XCorr");
            }
        }

        public double DelCn
        {
            get { return m_delCn; }
            set
            {
                m_delCn = value;
                OnPropertyChanged("DelCn");
            }
        }

        public double Sp
        {
            get { return m_sp; }
            set
            {
                m_sp = value;
                OnPropertyChanged("Sp");
            }
        }

        public string Reference
        {
            get { return m_reference; }
            set
            {
                m_reference = value;
                OnPropertyChanged("Reference");
            }
        }

        public double DelCn2
        {
            get { return m_delCn2; }
            set
            {
                m_delCn2 = value;
                OnPropertyChanged("DelCn2");
            }
        }

        public short RankSp
        {
            get { return m_rankSp; }
            set
            {
                m_rankSp = value;
                OnPropertyChanged("RankSp");
            }
        }

        public short RankXc
        {
            get { return m_rankXc; }
            set
            {
                m_rankXc = value;
                OnPropertyChanged("RankXc");
            }
        }

        public double XcRatio
        {
            get { return m_xcRatio; }
            set
            {
                m_xcRatio = value; 
                OnPropertyChanged("XcRatio");
            }
        }

        public bool PassFilt
        {
            get { return m_passFilt; }
            set
            {
                m_passFilt = value;
                OnPropertyChanged("PassFilt");
            }
        }

        public double FScore
        {
            get { return m_fScore; }
            set
            {
                m_fScore = value;
                OnPropertyChanged("FScore");
            }
        }

        public double MScore
        {
            get { return m_mScore; }
            set
            {
                m_mScore = value;
                OnPropertyChanged("MScore");
            }
        }

        public short NumTrypticEnds
        {
            get { return m_numTrypticEnds; }
            set
            {
                m_numTrypticEnds = value;
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
            var charge = result.Charge;

            if (charge > MaxChargeForScore)
            {
                charge = MaxChargeForScore;
            }

            var constant = Consts[charge - 1];
            var xcorr = XCorrs[charge - 1];
            var delta = Deltas[charge - 1];
            var logRank = Ranks[charge - 1];
            var absMassDiff = MassDiffs[charge - 1];
            var maxPeptideLength = MaxPeptideLens[charge - 1];
            var numFrags = NumFrags[charge - 1];

            var cleanPeptideLength = result.CleanPeptide.Length;

            if (cleanPeptideLength > maxPeptideLength)
            {
                cleanPeptideLength = maxPeptideLength;
            }

            var lgXcorr = Math.Log(result.XcRatio);
            var lgCleanPeptideLength = Math.Log((float)(1.0 * cleanPeptideLength * numFrags));
            var adjustedXCorr = lgXcorr / lgCleanPeptideLength;

            var tot = constant;
            tot += xcorr * adjustedXCorr;
            tot += delta * result.DelCn2;
            var lgRankSp = Math.Log(1.0 * result.RankSp);
            tot += logRank * lgRankSp;
            tot += absMassDiff * Math.Abs(result.DelM);

            return tot;
        }
    }
}