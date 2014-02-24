#region Namespaces

using System;
using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.UI;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Data
{
    public class Options : ObservableObject
    {
        #region Private Fields

        private RegressionType m_RegressionType ;
        private short m_RegressionOrder;
        private TargetWorkflowType m_TargetFilterType;
        private RetentionTimePredictionType m_PredictorType;
        private int m_MaxModsForAlignment;
        private short m_MinObservationsForExport;
        private bool m_ExportTryptic;
        private bool m_ExportPartiallyTryptic;
        private bool m_ExportNonTryptic;
        private double[] m_MinXCorrForExportTrytpic;
        private double[] m_MinXCorrForExportPartiallyTrytpic;
        private double[] m_MinXCorrForExportNonTrytpic;
        private double m_MinXCorrForAlignment;
        private bool m_UseDelCN;
        private double m_MaxDelCN;
        private double m_MaxLogEValForXTandemAlignment;
        private double m_MaxLogEValForXTandemExport;
        private double m_MaxLogEValForMSAlignAlignment;
        private short m_MaxRankForExport;
        private double m_MsgfFDR;
        private double m_MsgfSpectralEValue;

        #endregion

        #region Public Properties

        public int Id;
        // Regression
        public RegressionType RegressionType
        {
            get
            {
                return m_RegressionType;
            }
            set
            {
                m_RegressionType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        public short RegressionOrder
        {
            get
            {
                return m_RegressionOrder;
            }
            set
            {
                m_RegressionOrder = value;
                OnPropertyChanged("RegressionOrder");
            }
        }
        
        // General
        public TargetWorkflowType TargetFilterType
        {
            get
            {
                return m_TargetFilterType;
            }
            set
            {
                m_TargetFilterType = value;
                OnPropertyChanged("TargetFilterType");
            }
        }

        public RetentionTimePredictionType PredictorType
        {
            get
            {
                return m_PredictorType;
            }
            set
            {
                m_PredictorType = value;
                OnPropertyChanged("PredictorType");
            } 
        }

        // Peptides
        public int MaxModsForAlignment
        {
            get
            {
                return m_MaxModsForAlignment;
            }
            set
            {
                m_MaxModsForAlignment = value;
                OnPropertyChanged("MaxModsForAlignment");
            }  
        }

        public short MinObservationsForExport
        {
            get
            {
                return m_MinObservationsForExport;
            }
            set
            {
                m_MinObservationsForExport = value;
                OnPropertyChanged("MinObservationsForExport");
            }
        }

        public bool ExportTryptic
        {
            get
            {
                return m_ExportTryptic;
            }
            set
            {
                m_ExportTryptic = value;
                OnPropertyChanged("ExportTryptic");
            } 
        }

        public bool ExportPartiallyTryptic
        {
            get
            {
                return m_ExportPartiallyTryptic;
            }
            set
            {
                m_ExportPartiallyTryptic = value;
                OnPropertyChanged("ExportPartiallyTryptic");
            } 
        }

        public bool ExportNonTryptic
        {
            get
            {
                return m_ExportNonTryptic;
            }
            set
            {
                m_ExportNonTryptic = value;
                OnPropertyChanged("ExportNonTryptic");
            }
        }

        public double[] MinXCorrForExportTrytpic
        {
            get
            {
                return m_MinXCorrForExportTrytpic;
            }
            set
            {
                m_MinXCorrForExportTrytpic = value;
                OnPropertyChanged("MinXCorrForExportTrytpic");
            }
        }

        public double[] MinXCorrForExportPartiallyTrytpic
        {
            get
            {
                return m_MinXCorrForExportPartiallyTrytpic;
            }
            set
            {
                m_MinXCorrForExportPartiallyTrytpic = value;
                OnPropertyChanged("MinXCorrForExportPartiallyTrytpic");
            }
        }

        public double[] MinXCorrForExportNonTrytpic
        {
            get
            {
                return m_MinXCorrForExportNonTrytpic;
            }
            set
            {
                m_MinXCorrForExportNonTrytpic = value;
                OnPropertyChanged("MinXCorrForExportNonTrytpic");
            }
        }

        // Sequest
        public double MinXCorrForAlignment
        {
            get
            {
                return m_MinXCorrForAlignment;
            }
            set
            {
                m_MinXCorrForAlignment = value;
                OnPropertyChanged("MinXCorrForAlignment");
            }
        }

        public bool UseDelCN
        {
            get
            {
                return m_UseDelCN;
            }
            set
            {
                m_UseDelCN = value;
                OnPropertyChanged("UseDelCN");
            }
        }

        public double MaxDelCN
        {
            get
            {
                return m_MaxDelCN;
            }
            set
            {
                m_MaxDelCN = value;
                OnPropertyChanged("MaxDelCN");
            }
        }

        // Xtandem
        public double MaxLogEValForXTandemAlignment
        {
            get
            {
                return m_MaxLogEValForXTandemAlignment;
            }
            set
            {
                m_MaxLogEValForXTandemAlignment = value;
                OnPropertyChanged("MaxLogEValForXTandemAlignment");
            }
        }

        public double MaxLogEValForXTandemExport 
{
            get
            {
                return m_MaxLogEValForXTandemExport;
            }
            set
            {
                m_MaxLogEValForXTandemExport = value;
                OnPropertyChanged("MaxLogEValForXTandemExport");
            }
        }

        // MSAlign
        public double MaxLogEValForMSAlignAlignment
        {
            get
            {
                return m_MaxLogEValForMSAlignAlignment;
            }
            set
            {
                m_MaxLogEValForMSAlignAlignment = value;
                OnPropertyChanged("MaxLogEValForMSAlignAlignment");
            }
        }

        // MSGF+
        public double MsgfSpectralEValue
        {
            get
            {
                return m_MsgfSpectralEValue;
            }
            set
            {
                m_MsgfSpectralEValue = value;
                OnPropertyChanged("MsgfSpectralEValue");
            }
        }

        public double MsgfFDR
        {
            get
            {
                return m_MsgfFDR;
            }
            set
            {
                m_MsgfFDR = value;
                OnPropertyChanged("MsgfFDR");
            }
        }

        // Other
        public short MaxRankForExport
        {
            get
            {
                return m_MaxRankForExport;
            }
            set
            {
                m_MaxRankForExport = value;
                OnPropertyChanged("MaxRankForExport");
            }
        }

        #endregion

        public Options()
        {
            RegressionType = RegressionType.LinearEm;
            RegressionOrder = 1;

            TargetFilterType = TargetWorkflowType.BottomUp;

            PredictorType = RetentionTimePredictionType.Kangas;

            MaxModsForAlignment = 2;
            MinObservationsForExport = 2;

            ExportTryptic = true;
            ExportPartiallyTryptic = true;
            ExportNonTryptic = true;

            MinXCorrForExportTrytpic = new double[] { 1.5, 2.0, 2.5 };
            MinXCorrForExportPartiallyTrytpic = new double[] { 1.5, 2.0, 2.5 };
            MinXCorrForExportNonTrytpic = new double[] { 3.0, 3.5, 4.0 };

            MinXCorrForAlignment = 3.0;
            UseDelCN = true;
            MaxDelCN = 0.1;

            MaxLogEValForXTandemAlignment = -2.0;
            MaxLogEValForXTandemExport = -2.0;

            MaxLogEValForMSAlignAlignment = 1E-4;

            MaxRankForExport = 2;

            MsgfFDR = .01;
            MsgfSpectralEValue = .05;
        }
    }
}