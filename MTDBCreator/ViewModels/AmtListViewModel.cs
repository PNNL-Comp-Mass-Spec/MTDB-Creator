using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using MTDBCreator.DmsExporter.Data;
using MTDBCreator.DmsExporter.IO;
using MTDBCreator.Helpers;

namespace MTDBCreator.ViewModels
{
    public class AmtListViewModel : ObservableObject
    {
        public DmsLookupUtility Lookup { get; set; }

        public ObservableCollection<AmtInfo> MTDBList { get; set; }
        public ObservableCollection<AmtPeptideOptions> QualityStats { get; set;}

        public AmtInfo SelectedDb { get; set; }
        public AmtPeptideOptions SelectedStats { get; set; }

        public DelegateCommand RefreshMTDBsCommand { get; set; }
        public DelegateCommand GetStatsCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }

        public bool IsSaving
        {
            get => m_isSaving;
            private set
            {
                m_isSaving = value;
                OnPropertyChanged("IsSaving");
            }
        }

        public string SavingString
        {
            get => m_savingString;
            private set
            {
                m_savingString = value;
                OnPropertyChanged("SavingString");
            }
        }

        private bool m_isSaving;
        private string m_savingString;

        public AmtListViewModel()
        {
            MTDBList = new ObservableCollection<AmtInfo>();
            QualityStats = new ObservableCollection<AmtPeptideOptions>();
            IsSaving = false;

            Lookup = new DmsLookupUtility();

            Action refreshMTDBsAction   = RefreshMTDBs;
            Action getStatsAction       = GetStats;
            Action exportAction         = Export;
            RefreshMTDBsCommand = new DelegateCommand(refreshMTDBsAction);
            GetStatsCommand =     new DelegateCommand(getStatsAction) {Executable = false};
            ExportCommand =       new DelegateCommand(exportAction) {Executable = false};
        }

        private void RefreshMTDBs()
        {
            var list = Lookup.GetDatabases().Values.ToList();

            list.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

            MTDBList.Clear();
            foreach (var db in list)
            {
                MTDBList.Add(db);
            }

            if (list.Count != 0)
            {
                GetStatsCommand.Executable = true;
            }
        }

        void GetStats()
        {
            if (SelectedDb == null)
            {
                var button = MessageBoxButton.OK;
                var image = MessageBoxImage.Warning;
                var text = "Please select a Database to get stats of first";
                MessageBox.Show(text, "", button, image);
            }
            else
            {
                var list = Lookup.GetStats(SelectedDb).Values.ToList();

                QualityStats.Clear();
                foreach (var stat in list)
                {
                    QualityStats.Add(stat);
                }

                if (list.Count != 0)
                {
                    ExportCommand.Executable = true;
                }
            }
        }

        void Export()
        {
            if (SelectedStats == null)
            {
                var button = MessageBoxButton.OK;
                var image = MessageBoxImage.Warning;
                var text = "Please first select a filter for the mass tags to export";
                MessageBox.Show(text, "", button, image);
            }
            // Run the sql commands to get the database.
            else
            {
                IsSaving = true;

                string[] loadingStrings =
            {
                "Saving\nPlease Wait",
                "Saving.\nPlease Wait",
                "Saving..\nPlease Wait",
                "Saving...\nPlease Wait"
            };
                Task.Factory.StartNew(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        var index = 0;
                        while (IsSaving)
                        {
                            Thread.Sleep(750);
                            SavingString = loadingStrings[index%4];
                            index++;
                        }
                    });

                    var dialog = new SaveFileDialog
                    {
                        AddExtension = true,
                        RestoreDirectory = true,
                        Title = "Save Mass Tag Data Base",
                        Filter = "Access file (*.mdb)|*.mdb|Sql file (*.mtdb)|*.mtdb",
                        FilterIndex = 2,
                        FileName = SelectedDb.Name + ".mtdb"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        var path = dialog.FileName;

                        if (path.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase))
                        {
                            Lookup.Separator = ",";
                        }
                        else
                        {
                            Lookup.Separator = DmsLookupUtility.DEFAULT_SEPARATOR;
                        }

                        var success = Lookup.ExportToText(path, SelectedDb, SelectedStats);

                        if (success)
                        {
                            var exporter = TextToDbConverterFactory.Create(path);
                            var success2 = exporter.ConvertToDbFormat(path);

                            if (success2)
                                MessageBox.Show("Process complete", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show("Error with ConvertToDbFormat", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error with ExportToText", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }


                    }

                    IsSaving = false;
                });
            }
        }
    }
}