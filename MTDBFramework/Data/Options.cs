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
        private bool m_exportTryptic;
        private bool m_exportPartiallyTryptic;
        private bool m_exportNonTryptic;
        private double[] m_minXCorrForExportTryptic;
        private double[] m_minXCorrForExportPartiallyTryptic;
        private double[] m_minXCorrForExportNonTryptic;
        private double m_minXCorrForAlignment;
        private bool m_useDelCn;
        private double m_maxDelCn;
        private double m_maxLogEValForXTandemAlignment;
        private double m_maxLogEValForXTandemExport;
        private double m_maxLogEValForMsAlignAlignment;
        private short m_maxRankForExport;
        private double m_msgfFdr;
        private double m_maxMsgfSpecProb;
	    private double m_minimumObservedNet;
	    private double m_maximumObservedNet;

        #endregion

        #region Public Properties

		/// <summary>
		/// Id
		/// </summary>
        public int Id;

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

		/// <summary>
		/// Whether to export Tryptic peptides
		/// </summary>
        public bool ExportTryptic
        {
            get
            {
                return m_exportTryptic;
            }
            set
            {
                m_exportTryptic = value;
                OnPropertyChanged("ExportTryptic");
            } 
        }

		/// <summary>
		/// Whether to export partially Tryptic peptides
		/// </summary>
        public bool ExportPartiallyTryptic
        {
            get
            {
                return m_exportPartiallyTryptic;
            }
            set
            {
                m_exportPartiallyTryptic = value;
                OnPropertyChanged("ExportPartiallyTryptic");
            } 
        }

		/// <summary>
		/// Whether to export non-Tryptic peptides
		/// </summary>
        public bool ExportNonTryptic
        {
            get
            {
                return m_exportNonTryptic;
            }
            set
            {
                m_exportNonTryptic = value;
                OnPropertyChanged("ExportNonTryptic");
            }
        }

		/// <summary>
		/// Minimum XCorr for exporting Tryptic peptides
		/// </summary>
        public double[] MinXCorrForExportTryptic
        {
            get
            {
                return m_minXCorrForExportTryptic;
            }
            set
            {
                m_minXCorrForExportTryptic = value;
                OnPropertyChanged("MinXCorrForExportTryptic");
            }
        }

		/// <summary>
		/// Minimum XCorr for exporting partially Tryptic peptides
		/// </summary>
        public double[] MinXCorrForExportPartiallyTryptic
        {
            get
            {
                return m_minXCorrForExportPartiallyTryptic;
            }
            set
            {
                m_minXCorrForExportPartiallyTryptic = value;
                OnPropertyChanged("MinXCorrForExportPartiallyTryptic");
            }
        }

		/// <summary>
		/// Minimum XCorr for exporting non-Tryptic peptides
		/// </summary>
        public double[] MinXCorrForExportNonTryptic
        {
            get
            {
                return m_minXCorrForExportNonTryptic;
            }
            set
            {
                m_minXCorrForExportNonTryptic = value;
                OnPropertyChanged("MinXCorrForExportNonTryptic");
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

		/// <summary>
		/// Use DelCn for Sequest
		/// </summary>
        public bool UseDelCn
        {
            get
            {
                return m_useDelCn;
            }
            set
            {
                m_useDelCn = value;
                OnPropertyChanged("UseDelCN");
            }
        }

		/// <summary>
		/// Maximum DelCn for Sequest
		/// </summary>
        public double MaxDelCn
        {
            get
            {
                return m_maxDelCn;
            }
            set
            {
                m_maxDelCn = value;
                OnPropertyChanged("MaxDelCN");
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

		/// <summary>
		/// Maximum LogEVal for XTandem export
		/// </summary>
        public double MaxLogEValForXTandemExport 
{
            get
            {
                return m_maxLogEValForXTandemExport;
            }
            set
            {
                m_maxLogEValForXTandemExport = value;
                OnPropertyChanged("MaxLogEValForXTandemExport");
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
        public double MsgfFdr
        {
            get
            {
                return m_msgfFdr;
            }
            set
            {
                m_msgfFdr = value;
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

            MaxModsForAlignment = 2;
            MinObservationsForExport = 2;

            ExportTryptic = true;
            ExportPartiallyTryptic = true;
            ExportNonTryptic = true;

            MinXCorrForExportTryptic = new [] { 1.5, 2.0, 2.5 };
            MinXCorrForExportPartiallyTryptic = new [] { 1.5, 2.0, 2.5 };
            MinXCorrForExportNonTryptic = new [] { 3.0, 3.5, 4.0 };

            MinXCorrForAlignment = 3.0;
            UseDelCn = false;
            MaxDelCn = 0.1;

            MaxLogEValForXTandemAlignment = -2.0;
            MaxLogEValForXTandemExport = -2.0;

            MaxLogEValForMsAlignAlignment = 1E-4;

            MaxRankForExport = 2;

            MsgfFdr = .01;
            MaxMsgfSpecProb = 1E-10;

		    MinimumObservedNet = 0.0;
		    MaximumObservedNet = 1.0;

            OptionsChanged = false;
        }

        
    }
}