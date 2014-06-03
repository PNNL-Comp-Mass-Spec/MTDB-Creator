#region Namespaces

using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.UI;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
    public class Options : ObservableObject
    {
        #region Private Fields

        private RegressionType m_regressionType ;
        private short m_regressionOrder;
        private TargetWorkflowType m_targetFilterType;
        private RetentionTimePredictionType m_predictorType;
        private int m_maxModsForAlignment;
        private short m_minObservationsForExport;
        private bool m_exportTryptic;
        private bool m_exportPartiallyTryptic;
        private bool m_exportNonTryptic;
        private double[] m_minXCorrForExportTrytpic;
        private double[] m_minXCorrForExportPartiallyTrytpic;
        private double[] m_minXCorrForExportNonTrytpic;
        private double m_minXCorrForAlignment;
        private bool m_useDelCn;
        private double m_maxDelCn;
        private double m_maxLogEValForXTandemAlignment;
        private double m_maxLogEValForXTandemExport;
        private double m_maxLogEValForMsAlignAlignment;
        private short m_maxRankForExport;
        private double m_msgfFdr;
        private double m_msgfSpectralEValue;

        #endregion

        #region Public Properties

        public int Id;
        // Regression
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

        public bool OptionsChanged { get; set; }

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

        public double[] MinXCorrForExportTrytpic
        {
            get
            {
                return m_minXCorrForExportTrytpic;
            }
            set
            {
                m_minXCorrForExportTrytpic = value;
                OnPropertyChanged("MinXCorrForExportTrytpic");
            }
        }

        public double[] MinXCorrForExportPartiallyTrytpic
        {
            get
            {
                return m_minXCorrForExportPartiallyTrytpic;
            }
            set
            {
                m_minXCorrForExportPartiallyTrytpic = value;
                OnPropertyChanged("MinXCorrForExportPartiallyTrytpic");
            }
        }

        public double[] MinXCorrForExportNonTrytpic
        {
            get
            {
                return m_minXCorrForExportNonTrytpic;
            }
            set
            {
                m_minXCorrForExportNonTrytpic = value;
                OnPropertyChanged("MinXCorrForExportNonTrytpic");
            }
        }

        // Sequest
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

        // MSGF+
        public double MsgfSpectralEValue
        {
            get
            {
                return m_msgfSpectralEValue;
            }
            set
            {
                m_msgfSpectralEValue = value;
                OnPropertyChanged("MsgfSpectralEValue");
            }
        }

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

        // Other
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

        #endregion

        public Options()
        {
            RegressionType = RegressionType.LinearEm;
            RegressionOrder = 1;

            TargetFilterType = TargetWorkflowType.BOTTOM_UP;

            PredictorType = RetentionTimePredictionType.Kangas;

            MaxModsForAlignment = 2;
            MinObservationsForExport = 2;

            ExportTryptic = true;
            ExportPartiallyTryptic = true;
            ExportNonTryptic = true;

            MinXCorrForExportTrytpic = new [] { 1.5, 2.0, 2.5 };
            MinXCorrForExportPartiallyTrytpic = new [] { 1.5, 2.0, 2.5 };
            MinXCorrForExportNonTrytpic = new [] { 3.0, 3.5, 4.0 };

            MinXCorrForAlignment = 3.0;
            UseDelCn = false;//true;
            MaxDelCn = 0.1;

            MaxLogEValForXTandemAlignment = -2.0;
            MaxLogEValForXTandemExport = -2.0;

            MaxLogEValForMsAlignAlignment = 1E-4;

            MaxRankForExport = 2;

            MsgfFdr = .01;
            MsgfSpectralEValue = .05;

            OptionsChanged = false;
        }
    }
}