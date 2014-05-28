using System.Collections.ObjectModel;
using MTDBCreator.Commands;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.UI;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using PNNLOmics.Algorithms.Regression;

namespace MTDBCreator.ViewModels
{
    public class OptionsViewModel : ObservableObject
    {
        #region Private Fields

        private string m_PredictionText;
        private Options m_Options;

        private ICommand m_UpdatePredictionCommand;
        private ICommand m_SaveCommand;

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
                return m_PredictionText;
            }
            set
            {
                m_PredictionText = value;
                OnPropertyChanged("PredictionText");
            }
        }

        public Options Options
        {
            get
            {
                return m_Options;
            }
            set
            {
                m_Options = value;
                OnPropertyChanged("Options");
            }
        }

        public RegressionType SelectedRegressionType
        {

            get { return m_Options.RegressionType; }

            set
            {
                if (m_Options.RegressionType == value) return;
                m_Options.RegressionType = value;
                OnPropertyChanged("RegressionType");
            }
        }

        public ObservableCollection<RegressionType> RegressionTypes { get; private set; }
        public ICommand SaveCommand
        {
            get
            {
                if (m_SaveCommand == null)
                {
                    m_SaveCommand = new RelayCommand(options => Save(options));
                }

                return m_SaveCommand;
            }
        }

        public void Save(object param)
        { 
            // Remove this: This is just so I can see what the param object looks like
            var parameter = param as IList;
            // General
            Options.MaxModsForAlignment = Convert.ToInt32(parameter[0]);
            Options.MinObservationsForExport = Convert.ToInt16(parameter[1]);
            Options.MinXCorrForAlignment = Convert.ToInt32(parameter[2]);
            Options.MaxLogEValForXTandemAlignment = Convert.ToInt32(parameter[3]);

            // Regression type
            Options.RegressionType = (Convert.ToString(parameter[4]) == "LinearEm") ? RegressionType.LinearEm : RegressionType.MixtureRegression;

            // Predictor Type
            Options.PredictorType = (Convert.ToBoolean(parameter[6])) ? RetentionTimePredictionType.Kangas : RetentionTimePredictionType.Krokhin;

            // Tryptic Peptides
            Options.ExportTryptic = (Convert.ToBoolean(parameter[7])) ? true : false;
            Options.MinXCorrForExportTrytpic[0] = Convert.ToDouble(parameter[8]);
            Options.MinXCorrForExportTrytpic[1] = Convert.ToDouble(parameter[9]);
            Options.MinXCorrForExportTrytpic[2] = Convert.ToDouble(parameter[10]);

            // Partially Tryptic Peptides
            Options.ExportPartiallyTryptic = (Convert.ToBoolean(parameter[11])) ? true : false;
            Options.MinXCorrForExportPartiallyTrytpic[0] = Convert.ToDouble(parameter[12]);
            Options.MinXCorrForExportPartiallyTrytpic[1] = Convert.ToDouble(parameter[13]);
            Options.MinXCorrForExportPartiallyTrytpic[2] = Convert.ToDouble(parameter[14]);

            // Non Tryptic Peptides
            Options.ExportNonTryptic = (Convert.ToBoolean(parameter[15])) ? true : false;
            Options.MinXCorrForExportNonTrytpic[0] = Convert.ToDouble(parameter[16]);
            Options.MinXCorrForExportNonTrytpic[1] = Convert.ToDouble(parameter[17]);
            Options.MinXCorrForExportNonTrytpic[2] = Convert.ToDouble(parameter[18]);

            // Sequest dlCN
            Options.UseDelCn = (Convert.ToBoolean(parameter[19])) ? true : false;
            Options.MaxDelCn = Convert.ToDouble(parameter[20]);                
            
            // X!Tandem Export
            Options.MaxLogEValForXTandemExport = Convert.ToDouble(parameter[21]);
        }

        public ICommand UpdatePredictionCommand
        {
            get
            {
                if (m_UpdatePredictionCommand == null)
                {
                    m_UpdatePredictionCommand = new RelayCommand(param => UpdatePredictionReference());
                }
                return m_UpdatePredictionCommand;
            }
        }

        #endregion

        public OptionsViewModel(Options options)
        {
            Options = options;
            if(options.PredictorType == RetentionTimePredictionType.Kangas)
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
            if (Options.PredictorType == RetentionTimePredictionType.Krokhin)
            {
                PredictionText = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
                   "K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
                   "E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
                   "accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
                   "Analytical Chemistry, 75 (5) 1039-1048.";

                Options.PredictorType = RetentionTimePredictionType.Kangas;
            }
            else
            {
                PredictionText = "Prediction algorithm developed by Oleg Krokhin.  See: " +
                     "O.V. Krokhin, R. Craig, V. Spicer, W. Ens, K.G. Standing, R.C. Beavis, J.A. Wilkins. 2004. " +
                     "\"An improved model for prediction of retention times of tryptic peptides in ion pair reversed-phase HPLC " +
                     "- Its application to protein peptide mapping by off-line HPLC-MALDI MS\". " +
                    "Molecular & Cellular Proteomics, 3 (9) 908-919.";

                Options.PredictorType = RetentionTimePredictionType.Krokhin;
            }
        }

        #endregion
    }
}
