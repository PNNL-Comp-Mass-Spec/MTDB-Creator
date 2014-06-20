using System.Collections.ObjectModel;
using MTDBCreator.Commands;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.UI;
using System;
using System.Collections;
using System.Windows.Input;
using PNNLOmics.Algorithms.Regression;

namespace MTDBCreator.ViewModels
{
    public class OptionsViewModel : ObservableObject
    {
        #region Private Fields

        private string m_predictionText;
        private Options m_options;


        private ICommand m_updatePredictionCommand;
        private ICommand m_saveCommand;

        #endregion

        public OptionsViewModel()
        {
            RegressionTypes = new ObservableCollection<RegressionType>
            {
                RegressionType.LinearEm,
                RegressionType.MixtureRegression
            };
            PredictionText = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
                   "K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
                   "E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
                   "accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
                   "Analytical Chemistry, 75 (5) 1039-1048.";
        }

        #region Public Properties


        public string PredictionText
        {
            get
            {
                return m_predictionText;
            }
            set
            {
                m_predictionText = value;
                OnPropertyChanged("PredictionText");
            }
        }

        public bool OptionsChanged { get; set; }

        public Options Options
        {
            get
            {
                return m_options;
            }
            set
            {
                m_options = value;
                OptionsChanged = true;
                OnPropertyChanged("Options");
            }
        }

        public RegressionType SelectedRegressionType
        {

            get { return m_options.RegressionType; }

            set
            {
                if (m_options.RegressionType == value) return;
                m_options.RegressionType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        public ObservableCollection<RegressionType> RegressionTypes { get; private set; }
        public ICommand SaveCommand
        {
            get { return m_saveCommand ?? (m_saveCommand = new RelayCommand(Save)); }
        }

        public void Save(object param)
        { 
            var parameter = param as IList;
            // General
            if (parameter != null)
            {
                Options.MaxModsForAlignment = Convert.ToInt32(parameter[0]);
                Options.MinObservationsForExport = Convert.ToInt16(parameter[1]);
                Options.MinXCorrForAlignment = Convert.ToInt32(parameter[2]);
                Options.MaxLogEValForXTandemAlignment = Convert.ToInt32(parameter[3]);
                Options.MaxMsgfSpecProb = Convert.ToDouble(parameter[4]);
                Options.MsgfFdr = Convert.ToDouble(parameter[5]);

                // Regression type
                Options.RegressionType = (Convert.ToString(parameter[6]) == "LinearEm")
                    ? RegressionType.LinearEm
                    : RegressionType.MixtureRegression;
                Options.RegressionOrder = (Convert.ToInt16(parameter[7]));

                // Predictor Type
                Options.PredictorType = (Convert.ToBoolean(parameter[8]))
                    ? RetentionTimePredictionType.KANGAS
                    : RetentionTimePredictionType.KROKHIN;

                // Tryptic Peptides
                Options.ExportTryptic = (Convert.ToBoolean(parameter[9]));
                Options.MinXCorrForExportTrytpic[0] = Convert.ToDouble(parameter[10]);
                Options.MinXCorrForExportTrytpic[1] = Convert.ToDouble(parameter[11]);
                Options.MinXCorrForExportTrytpic[2] = Convert.ToDouble(parameter[12]);

                // Partially Tryptic Peptides
                Options.ExportPartiallyTryptic = (Convert.ToBoolean(parameter[13]));
                Options.MinXCorrForExportPartiallyTrytpic[0] = Convert.ToDouble(parameter[14]);
                Options.MinXCorrForExportPartiallyTrytpic[1] = Convert.ToDouble(parameter[15]);
                Options.MinXCorrForExportPartiallyTrytpic[2] = Convert.ToDouble(parameter[16]);

                // Non Tryptic Peptides
                Options.ExportNonTryptic = (Convert.ToBoolean(parameter[17]));
                Options.MinXCorrForExportNonTrytpic[0] = Convert.ToDouble(parameter[18]);
                Options.MinXCorrForExportNonTrytpic[1] = Convert.ToDouble(parameter[19]);
                Options.MinXCorrForExportNonTrytpic[2] = Convert.ToDouble(parameter[20]);

                // Sequest dlCN
                Options.UseDelCn = (Convert.ToBoolean(parameter[21]));
                Options.MaxDelCn = Convert.ToDouble(parameter[22]);

                // X!Tandem Export
                Options.MaxLogEValForXTandemExport = Convert.ToDouble(parameter[23]);
                Options.OptionsChanged = true;
            }
        }

        public ICommand UpdatePredictionCommand
        {
            get {
                return m_updatePredictionCommand ??
                       (m_updatePredictionCommand = new RelayCommand(param => UpdatePredictionReference()));
            }
        }

        #endregion

        public OptionsViewModel(Options options)
        {
            Options = options;
            if(options.PredictorType == RetentionTimePredictionType.KANGAS)
            {
                PredictionText = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
                   "K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
                   "E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
                   "accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
                   "Analytical Chemistry, 75 (5) 1039-1048.";
            }
            else
            {
                PredictionText = "Prediction algorithm developed by Oleg Krokhin.  See: " +
                     "O.V. Krokhin, R. Craig, V. Spicer, W. Ens, K.G. Standing, R.C. Beavis, J.A. Wilkins. 2004. " +
                     "\"An improved model for prediction of retention times of tryptic peptides in ion pair reversed-phase HPLC " +
                     "- Its application to protein peptide mapping by off-line HPLC-MALDI MS\". " +
                    "Molecular & Cellular Proteomics, 3 (9) 908-919.";
            }
        }

        #region Helpers

        private void UpdatePredictionReference()
        {
            if (Options.PredictorType == RetentionTimePredictionType.KROKHIN)
            {
                PredictionText = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
                   "K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
                   "E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
                   "accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
                   "Analytical Chemistry, 75 (5) 1039-1048.";

                Options.PredictorType = RetentionTimePredictionType.KANGAS;
            }
            else
            {
                PredictionText = "Prediction algorithm developed by Oleg Krokhin.  See: " +
                     "O.V. Krokhin, R. Craig, V. Spicer, W. Ens, K.G. Standing, R.C. Beavis, J.A. Wilkins. 2004. " +
                     "\"An improved model for prediction of retention times of tryptic peptides in ion pair reversed-phase HPLC " +
                     "- Its application to protein peptide mapping by off-line HPLC-MALDI MS\". " +
                    "Molecular & Cellular Proteomics, 3 (9) 908-919.";

                Options.PredictorType = RetentionTimePredictionType.KROKHIN;
            }
        }

        #endregion
    }
}
