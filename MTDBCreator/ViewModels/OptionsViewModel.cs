using System.Collections.ObjectModel;
using System;
using MTDBCreator.Commands;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.UI;
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
        private bool m_enabled;

        #endregion

        public OptionsViewModel()
        {
            RegressionTypes = new ObservableCollection<RegressionType>
            {
                RegressionType.LinearEm,
                RegressionType.MixtureRegression
            };
            DatabaseTypes = new ObservableCollection<DatabaseType>
            {
                DatabaseType.Access,
                DatabaseType.SQLite,
                DatabaseType.NotSaved
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
            get => m_predictionText;
            set
            {
                m_predictionText = value;
                OnPropertyChanged("PredictionText");
            }
        }

        public bool OptionsChanged { get; set; }

        public Options Options
        {
            get => m_options;
            set
            {
                m_options = value;
                OptionsChanged = true;
                OnPropertyChanged("Options");
            }
        }

        public RegressionType SelectedRegressionType
        {

            get => m_options.RegressionType;

            set
            {
                OrderEnabled = (value != RegressionType.LinearEm);
                if (m_options.RegressionType == value) return;
                m_options.RegressionType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        public bool OrderEnabled
        {
            get => m_enabled;
            private set
            {
                m_enabled = value;
                OnPropertyChanged("OrderEnabled");
            }
        }

        public DatabaseType SelectedDatabaseType
        {

            get => m_options.DatabaseType;

            set
            {
                if (m_options.DatabaseType == value) return;
                m_options.DatabaseType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        public ObservableCollection<RegressionType> RegressionTypes { get; }
        public ObservableCollection<DatabaseType> DatabaseTypes { get; }
        public ICommand SaveCommand => m_saveCommand ?? (m_saveCommand = new RelayCommand(Save));

        public void Save(object param)
        {
            // General
            if (param is IList parameter)
            {
                Options.MaxMsgfSpecProb = Convert.ToDouble(parameter[0]);
                Options.MsgfQValue = Convert.ToDouble(parameter[1]);

                // Regression type
                Options.RegressionType = (Convert.ToString(parameter[2]) == "LinearEm")
                    ? RegressionType.LinearEm
                    : RegressionType.MixtureRegression;

                Options.RegressionOrder = (Convert.ToInt16(parameter[3]));

                // Predictor Type
                Options.PredictorType = (Convert.ToBoolean(parameter[4]))
                    ? RetentionTimePredictionType.KANGAS
                    : RetentionTimePredictionType.KROKHIN;

                Options.MsgfFilter = (Convert.ToBoolean(parameter[5]))
                    ? MsgfFilterType.SPECTRAL_PROBABILITY
                    : MsgfFilterType.Q_VALUE;

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
