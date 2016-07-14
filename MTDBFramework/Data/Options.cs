#region Namespaces

using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.UI;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
    /// <summary>
    /// MTDBFramework configuration options
    /// </summary>
    public class Options : ObservableObject
    {
        #region Private Fields

        private DatabaseType m_databaseType;
        private RegressionType m_regressionType;
        private short m_regressionOrder;
        private TargetWorkflowType m_targetFilterType;
        private RetentionTimePredictionType m_predictorType;
        private int m_maxModsForAlignment;
        private short m_minObservationsForExport;
        private double m_minXCorrForAlignment;
        private double m_maxLogEValForXTandemAlignment;
        private double m_maxLogEValForMsAlignAlignment;
        private short m_maxRankForExport;
        private double m_msgfQValue;
        private double m_maxMsgfSpecProb;
        private double m_minimumObservedNet;
        private double m_maximumObservedNet;
        private MsgfFilterType m_msfgFilterType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Format of saved Database
        /// </summary>
        public DatabaseType DatabaseType
        {
            get { return m_databaseType; }
            set
            {
                m_databaseType = value;
                OnPropertyChanged("DatabaseType");
            }
        }

        /// <summary>
        /// Regression
        /// </summary>
        public RegressionType RegressionType
        {
            get
            {
                return m_regressionType;
            }
            set
            {
                m_regressionType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        /// <summary>
        /// Options Changed
        /// </summary>
        public bool OptionsChanged { get; set; }

        /// <summary>
        /// Regression Order
        /// </summary>
        public short RegressionOrder
        {
            get
            {
                return m_regressionOrder;
            }
            set
            {
                m_regressionOrder = value;
                OnPropertyChanged("RegressionOrder");
            }
        }

        // General
        /// <summary>
        /// Target Filter Type
        /// </summary>
        public TargetWorkflowType TargetFilterType
        {
            get
            {
                return m_targetFilterType;
            }
            set
            {
                m_targetFilterType = value;
                OnPropertyChanged("TargetFilterType");
            }
        }

        /// <summary>
        /// Retention Time Predictor Type
        /// </summary>
        public RetentionTimePredictionType PredictorType
        {
            get
            {
                return m_predictorType;
            }
            set
            {
                m_predictorType = value;
                OnPropertyChanged("PredictorType");
            }
        }

        // Peptides
        /// <summary>
        /// Maximum mods for peptide alignment
        /// </summary>
        public int MaxModsForAlignment
        {
            get
            {
                return m_maxModsForAlignment;
            }
            set
            {
                m_maxModsForAlignment = value;
                OnPropertyChanged("MaxModsForAlignment");
            }
        }

        /// <summary>
        /// Minimum observations for export
        /// </summary>
        public short MinObservationsForExport
        {
            get
            {
                return m_minObservationsForExport;
            }
            set
            {
                m_minObservationsForExport = value;
                OnPropertyChanged("MinObservationsForExport");
            }
        }

        // Sequest
        /// <summary>
        /// Minimum XCorr for Sequest Alignment
        /// </summary>
        public double MinXCorrForAlignment
        {
            get
            {
                return m_minXCorrForAlignment;
            }
            set
            {
                m_minXCorrForAlignment = value;
                OnPropertyChanged("MinXCorrForAlignment");
            }
        }

        // Xtandem
        /// <summary>
        /// Maximum LogEVal for XTandem Alignment
        /// </summary>
        public double MaxLogEValForXTandemAlignment
        {
            get
            {
                return m_maxLogEValForXTandemAlignment;
            }
            set
            {
                m_maxLogEValForXTandemAlignment = value;
                OnPropertyChanged("MaxLogEValForXTandemAlignment");
            }
        }

        // MSAlign
        /// <summary>
        /// Maximum LogEVal for MSAlign Alignment
        /// </summary>
        public double MaxLogEValForMsAlignAlignment
        {
            get
            {
                return m_maxLogEValForMsAlignAlignment;
            }
            set
            {
                m_maxLogEValForMsAlignAlignment = value;
                OnPropertyChanged("MaxLogEValForMSAlignAlignment");
            }
        }

        /// <summary>
        /// MSGF FDR
        /// </summary>
        public double MsgfQValue
        {
            get
            {
                return m_msgfQValue;
            }
            set
            {
                m_msgfQValue = value;
                OnPropertyChanged("MsgfFDR");
            }
        }

        /// <summary>
        /// Maximum MSGF SpecProb
        /// </summary>
        public double MaxMsgfSpecProb
        {
            get
            {
                return m_maxMsgfSpecProb;
            }
            set
            {
                m_maxMsgfSpecProb = value;
                OnPropertyChanged("MaxMSGFSpecProb");
            }
        }

        // Other
        /// <summary>
        /// Maximum Rank for export
        /// </summary>
        public short MaxRankForExport
        {
            get
            {
                return m_maxRankForExport;
            }
            set
            {
                m_maxRankForExport = value;
                OnPropertyChanged("MaxRankForExport");
            }
        }

        /// <summary>
        /// Minimum Observed Net for export
        /// </summary>
        public double MinimumObservedNet
        {
            get
            {
                return m_minimumObservedNet;
            }
            set
            {
                m_minimumObservedNet = value;
                OnPropertyChanged("MinimumObservedNet");
            }
        }

        /// <summary>
        /// Maximum Observed Net for export
        /// </summary>
        public double MaximumObservedNet
        {
            get
            {
                return m_maximumObservedNet;
            }
            set
            {
                m_maximumObservedNet = value;
                OnPropertyChanged("MaximumObservedNet");
            }
        }

        /// <summary>
        /// Filter type for MSGF+ analysis
        /// </summary>
        public MsgfFilterType MsgfFilter
        {
            get
            {
                return m_msfgFilterType;
            }
            set
            {
                m_msfgFilterType = value;
                OnPropertyChanged("MsgfFilter");
            }
        }

        #endregion

        /// <summary>
        /// Constructor to default values
        /// </summary>
        public Options()
        {
            DatabaseType = DatabaseType.SQLite;

            RegressionType = RegressionType.LinearEm;
            RegressionOrder = 1;

            TargetFilterType = TargetWorkflowType.BOTTOM_UP;

            PredictorType = RetentionTimePredictionType.KANGAS;

            MaxModsForAlignment = 0;
            MinObservationsForExport = 1;

            MinXCorrForAlignment = 3.0;

            MaxLogEValForXTandemAlignment = -2.0;

            MaxLogEValForMsAlignAlignment = 1E-4;

            MaxRankForExport = 2;

            MsgfQValue = .01;
            MaxMsgfSpecProb = 1E-10;
            MsgfFilter = MsgfFilterType.Q_VALUE;

            MinimumObservedNet = 0.0;
            MaximumObservedNet = 1.0;

            OptionsChanged = false;
        }

    }
}