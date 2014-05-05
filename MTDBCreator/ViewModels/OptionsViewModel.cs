using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MTDBCreator.Commands;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.UI;
using Regressor.Algorithms;

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
            List<string> options = ((IEnumerable)param).Cast<string>().ToList();

            // General
            Options.MaxModsForAlignment = Convert.ToInt32(options[0]);
            Options.MinObservationsForExport = Convert.ToInt16(options[1]);
            Options.MinXCorrForAlignment = Convert.ToInt32(options[2]);
            Options.MaxLogEValForXTandemAlignment = Convert.ToInt32(options[3]);

            // Regression type
            Options.RegressionType = (Convert.ToBoolean(options[4])) ? RegressionType.LinearEm : RegressionType.MixtureRegression;

            // Predictor Type
            Options.PredictorType = (Convert.ToBoolean(options[7])) ? RetentionTimePredictionType.Kangas : RetentionTimePredictionType.Krokhin;

            // Tryptic Peptides
            Options.ExportTryptic = (Convert.ToBoolean(options[9])) ? true : false;
            Options.MinXCorrForExportTrytpic[0] = Convert.ToDouble(options[10]);
            Options.MinXCorrForExportTrytpic[1] = Convert.ToDouble(options[11]);
            Options.MinXCorrForExportTrytpic[2] = Convert.ToDouble(options[12]);

            // Partially Tryptic Peptides
            Options.ExportPartiallyTryptic = (Convert.ToBoolean(options[13])) ? true : false;
            Options.MinXCorrForExportPartiallyTrytpic[0] = Convert.ToDouble(options[14]);
            Options.MinXCorrForExportPartiallyTrytpic[1] = Convert.ToDouble(options[15]);
            Options.MinXCorrForExportPartiallyTrytpic[2] = Convert.ToDouble(options[16]);

            // Non Tryptic Peptides
            Options.ExportNonTryptic = (Convert.ToBoolean(options[17])) ? true : false;
            Options.MinXCorrForExportNonTrytpic[0] = Convert.ToDouble(options[18]);
            Options.MinXCorrForExportNonTrytpic[1] = Convert.ToDouble(options[19]);
            Options.MinXCorrForExportNonTrytpic[2] = Convert.ToDouble(options[20]);

            // Sequest dlCN
            Options.UseDelCn = (Convert.ToBoolean(options[21])) ? true : false;
            Options.MaxDelCn = Convert.ToDouble(options[22]);

            // X!Tandem Export
            Options.MaxLogEValForXTandemExport = Convert.ToDouble(options[23]);
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

            PredictionText = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
                   "K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
                   "E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
                   "accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
                   "Analytical Chemistry, 75 (5) 1039-1048.";
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
