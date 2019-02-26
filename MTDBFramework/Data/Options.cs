#region Namespaces

using System.ComponentModel;
using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using FeatureAlignment.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
    /// <summary>
    /// MTDBFramework configuration options
    /// </summary>
    public class Options : INotifyPropertyChanged
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

        // ReSharper disable once IdentifierTypo
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
            get => m_databaseType;
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
            get => m_regressionType;
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
            get => m_regressionOrder;
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
            get => m_targetFilterType;
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
            get => m_predictorType;
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
            get => m_maxModsForAlignment;
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
            get => m_minObservationsForExport;
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
            get => m_minXCorrForAlignment;
            set
            {
                m_minXCorrForAlignment = value;
                OnPropertyChanged("MinXCorrForAlignment");
            }
        }

        // X!Tandem
        /// <summary>
        /// Maximum LogEVal for X!Tandem Alignment
        /// </summary>
        public double MaxLogEValForXTandemAlignment
        {
            get => m_maxLogEValForXTandemAlignment;
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
            get => m_maxLogEValForMsAlignAlignment;
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
            get => m_msgfQValue;
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
            get => m_maxMsgfSpecProb;
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
            get => m_maxRankForExport;
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
            get => m_minimumObservedNet;
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
            get => m_maximumObservedNet;
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
            get => m_msfgFilterType;
            set
            {
                m_msfgFilterType = value;
                OnPropertyChanged("MsgfFilter");
            }
        }

        /// <summary>
        /// When true, show exceptions at the console
        /// When false, use a MessageBox to inform the user of an exception
        /// </summary>
        public bool ConsoleMode { get; set; }

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

            ConsoleMode = false;

            OptionsChanged = false;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}