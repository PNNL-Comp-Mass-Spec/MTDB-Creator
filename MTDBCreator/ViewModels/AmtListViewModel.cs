﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using MTDBCreator.DmsExporter.Data;
using MTDBCreator.DmsExporter.IO;

namespace MTDBCreator.ViewModels
{
    public class AmtListViewModel : ViewModelBase
    {
        public DmsLookupUtility Lookup { get; set; }

        public ObservableCollection<AmtInfo> MtdbList { get; set; }
        public ObservableCollection<AmtPeptideOptions> QualityStats { get; set;}

        public AmtInfo SelectedDb { get; set; }
        public AmtPeptideOptions SelectedStats { get; set; }

        public DelegateCommand RefreshMtdbsCommand { get; set; }
        public DelegateCommand GetStatsCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }

        public AmtListViewModel()
        {
            MtdbList = new ObservableCollection<AmtInfo>();
            QualityStats = new ObservableCollection<AmtPeptideOptions>();

            Lookup = new DmsLookupUtility();

            Action refreshMtdbsAction   = RefreshMtdbs;
            Action getStatsAction       = GetStats;
            Action exportAction         = Export;
            RefreshMtdbsCommand = new DelegateCommand(refreshMtdbsAction);
            GetStatsCommand     = new DelegateCommand(getStatsAction);
            GetStatsCommand.Executable = false;
            ExportCommand       = new DelegateCommand(exportAction);
            ExportCommand.Executable = false;
        }

        private void RefreshMtdbs()
        {
            var list = Lookup.GetDatabases().Values.ToList();

            list.Sort((x, y) => x.Name.CompareTo(y.Name));

            MtdbList.Clear();
            foreach (var db in list)
            {
                MtdbList.Add(db);
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
                var dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    RestoreDirectory = true,
                    Title = "Save Mass Tag Data Base",
                    Filter = "Access file (*.mdb)|*.mdb|Sql file (*.mtdb)|*.mtdb",
                    FilterIndex = 2
                };

                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;

                    Lookup.ExportToText(path, SelectedDb, SelectedStats);

                    var exporter = TextToDbConverterFactory.Create(path);
                    exporter.Convert(path);
                }
            }    
        }
    }
}