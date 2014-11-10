using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Data;
using MTDBFramework.UI;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MTDBCreator.ViewModels
{
    public class DatasetPlotViewModel : ObservableObject
    {
        #region Private Fields

        private AnalysisJobViewModel m_AnalysisJobViewModel;
        private IList<PlotModel> m_plotModels;
        private PlotModel m_NETScanPlotModel;
        private PlotModel m_massScanPlotModel;
        private PlotModel m_observedPredictedPlotModel;

        private ICommand m_ZoomExtentsCommand;
        private ICommand m_SelectItemsCommand;

        private IEnumerable<AnalysisJobItem> m_CurrentAnalysisJobItems;
        private bool m_IsRegressionLineVisible = false;

        private Dictionary<string, Color> m_SeriesColorDictionary;

        #endregion

        #region Public Properties

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get
            {
                return m_AnalysisJobViewModel;
            }
            private set
            {
                m_AnalysisJobViewModel = value;
                OnPropertyChanged("AnalysisJobViewModel");

                CurrentAnalysisJobItems = value.AnalysisJobItems;
            }
        }

        public PlotModel NETScanPlotModel
        {
            get
            {
                return m_NETScanPlotModel;
            }
            set
            {
                m_NETScanPlotModel = value;
                OnPropertyChanged("NETScanPlotModel");
            }
        }

        public PlotModel MassScanPlotModel
        {
            get
            {
                return m_massScanPlotModel;
            }
            set
            {
                m_massScanPlotModel = value;
                OnPropertyChanged("MassScanPlotModel");
            }
        }

        public PlotModel ObservedPredictedPlotModel
        {
            get
            {
                return m_observedPredictedPlotModel;
            }
            set
            {
                m_observedPredictedPlotModel = value;
                OnPropertyChanged("ObservedPredictedPlotModel");
            }
        }

        public ICommand ZoomExtentsCommand
        {
            get
            {
                if (m_ZoomExtentsCommand == null)
                {
                    m_ZoomExtentsCommand = new RelayCommand(param => ZoomExtents());
                }

                return m_ZoomExtentsCommand;
            }
        }

        public ICommand SelectItemsCommand
        {
            get
            {
                if (m_SelectItemsCommand == null)
                {
                    m_SelectItemsCommand = new RelayCommand(items => SelectItems(items));
                }

                return m_SelectItemsCommand;
            }
        }

        public IEnumerable<AnalysisJobItem> CurrentAnalysisJobItems
        {
            get
            {
                return m_CurrentAnalysisJobItems;
            }
            set
            {
                m_CurrentAnalysisJobItems = value;
                OnPropertyChanged("CurrentAnalysisJobItems");

                if (m_CurrentAnalysisJobItems != null)
                {
                    FillAnalysisSeries(m_plotModels, CurrentAnalysisJobItems, AnalysisJobViewModel.Options);
                    FillAnalysisAnnotations(m_plotModels, CurrentAnalysisJobItems);
                }
            }
        }

        public bool IsRegressionLineVisible
        {
            get
            {
                return m_IsRegressionLineVisible;
            }
            set
            {
                m_IsRegressionLineVisible = value;
                OnPropertyChanged("IsRegressionLineVisible");

                if (CurrentAnalysisJobItems != null)
                {
                    FillAnalysisAnnotations(m_plotModels, CurrentAnalysisJobItems);
                }
            }
        }

        #endregion

        #region Public Instance Methods

        public LinearAxis MakeLinerAxis(AxisPosition position)
        {
            return new LinearAxis
            {
                Position = position,
                TitleFontSize = 14,
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                AxisTitleDistance = 12,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.Parse("#11000000"),
                Minimum = 0.0,
                AbsoluteMinimum = 0.0
            };
        }

        //public ScatterSeries MakeAnalysisScatterSeries(AnalysisJobItem analysisJobItem, Options options)
        public ScatterSeries MakeNetAnalysisScatterSeries(AnalysisJobItem analysisJobItem, Options options)
        {
            var color = m_SeriesColorDictionary[analysisJobItem.FilePath];

            var scatterSeries = new ScatterSeries
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColor.FromArgb(color.A, color.R, color.G, color.B),
                MarkerType = MarkerType.Cross,
                Title = analysisJobItem.Title,
                Tag = analysisJobItem,
            };

            foreach (var evidence in analysisJobItem.DataSet.Evidences)
            {
                var filter = AlignmentFilterFactory.Create(analysisJobItem.Format, options);

                if (!filter.ShouldFilter(evidence))
                {
                    var scatterPoint = new ScatterPoint(evidence.Scan, evidence.ObservedNet)
                    {
                        Tag = evidence
                    };

                    scatterSeries.Points.Add(scatterPoint);
                }
            }

            return scatterSeries;
        }
        
        public ScatterSeries MakeMassAnalysisScatterSeries(AnalysisJobItem analysisJobItem, Options options)
        {
            var color = m_SeriesColorDictionary[analysisJobItem.FilePath];

            var scatterSeries = new ScatterSeries
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColor.FromArgb(color.A, color.R, color.G, color.B),
                MarkerType = MarkerType.Cross,
                Title = analysisJobItem.Title,
                Tag = analysisJobItem,
            };

            foreach (var evidence in analysisJobItem.DataSet.Evidences)
            {
                var filter = AlignmentFilterFactory.Create(analysisJobItem.Format, options);

                if (!filter.ShouldFilter(evidence))
                {
                    var scatterPoint = new ScatterPoint(evidence.Scan, evidence.MonoisotopicMass)
                    {
                        Tag = evidence
                    };

                    scatterSeries.Points.Add(scatterPoint);
                }
            }

            return scatterSeries;
        }

        public ScatterSeries MakePredictedAnalysisScatterSeries(AnalysisJobItem analysisJobItem, Options options)
        {
            var color = m_SeriesColorDictionary[analysisJobItem.FilePath];

            var scatterSeries = new ScatterSeries
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColor.FromArgb(color.A, color.R, color.G, color.B),
                MarkerType = MarkerType.Cross,
                Title = analysisJobItem.Title,
                Tag = analysisJobItem,
            };

            foreach (var evidence in analysisJobItem.DataSet.Evidences)
            {
                var filter = AlignmentFilterFactory.Create(analysisJobItem.Format, options);

                if (!filter.ShouldFilter(evidence))
                {
                    var scatterPoint = new ScatterPoint(evidence.ObservedNet, evidence.PredictedNet)
                    {
                        Tag = evidence
                    };

                    scatterSeries.Points.Add(scatterPoint);
                }
            }

            return scatterSeries;
        }

        // DEGAN TESTING END

        public LineAnnotation MakeRegressionLineAnnotation(AnalysisJobItem analysisJobItem)
        {
            return new LineAnnotation
            {
                Type = LineAnnotationType.LinearEquation,
                Color = OxyColors.Black,
                Intercept = analysisJobItem.DataSet.RegressionResult.Intercept,
                Slope = analysisJobItem.DataSet.RegressionResult.Slope,
                StrokeThickness = 2,
                Layer = AnnotationLayer.AboveSeries,
                LineStyle = LineStyle.Solid,
                Tag = analysisJobItem
            };
        }

        public void FillAnalysisSeries(IList<PlotModel> plotModels, IEnumerable<AnalysisJobItem> analysisJobItems, Options options)
        {
            for (var i = 0; i < plotModels.Count; i++)
            {
                var plotModel = plotModels[i];
                foreach (var series in plotModel.Series)
                {
                    series.IsVisible = false;
                }

                foreach (var analysisJobItem in analysisJobItems)
                {
                    // Add new color for this item, if the color does not exist

                    if (!m_SeriesColorDictionary.ContainsKey(analysisJobItem.FilePath))
                    {
                        m_SeriesColorDictionary.Add(analysisJobItem.FilePath, GraphHelper.PickColor());
                    }

                    var seriesList =
                        from s in plotModel.Series
                        where s.Tag == analysisJobItem
                        select s;

                    if (seriesList.Any())
                    {
                        foreach (var series in seriesList)
                        {
                            series.IsVisible = true;
                        }
                    }
                    else
                    {
                        // Degan test stuff
                        switch (i)
                        {
                            case 0:
                                plotModel.Series.Add(MakeNetAnalysisScatterSeries(analysisJobItem, options));
                                break;
                                
                            case 1:
                                plotModel.Series.Add(MakeMassAnalysisScatterSeries(analysisJobItem, options));
                                break;

                            case 2:
                                plotModel.Series.Add(MakePredictedAnalysisScatterSeries(analysisJobItem, options));
                                break;
                        }
                    }

                }
                plotModel.InvalidatePlot(true);
            }
        }

        public void FillAnalysisAnnotations(IList<PlotModel> plotModels, IEnumerable<AnalysisJobItem> analysisJobItems)
        {
            foreach(var plotModel in plotModels)
            {
                plotModel.Annotations.Clear();

                if (IsRegressionLineVisible)
                {
                    foreach (var analysisJobItem in analysisJobItems)
                    {
                        plotModel.Annotations.Add(MakeRegressionLineAnnotation(analysisJobItem));
                    }
                }
                plotModel.InvalidatePlot(true);
            }
        }

        #endregion

        #region Command Methods

        private void ZoomExtents()
        {
            foreach (var axis in NETScanPlotModel.Axes)
            {
                axis.Reset();
            }
        }

        private void SelectItems(object param)
        {
            IEnumerable<AnalysisJobItem> selectedAnalysisJobItems = ((IEnumerable)param).Cast<AnalysisJobItem>().ToList();

            if (selectedAnalysisJobItems.Any())
            {
                CurrentAnalysisJobItems = selectedAnalysisJobItems;
            }
        }

        #endregion

        public DatasetPlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_SeriesColorDictionary = new Dictionary<string, Color>();
            m_plotModels = new List<PlotModel>();

            NETScanPlotModel = new PlotModel
            {
                Title = "Observed NET Vs. Scan",
                IsLegendVisible = true,
                LegendTitle = "LEGEND",
                LegendPosition = LegendPosition.RightMiddle,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendBackground = OxyColors.Transparent,
            };

            NETScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            NETScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            foreach (var axis in NETScanPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "Observed NET";
                        axis.AbsoluteMinimum = 0;
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "Scan";
                        axis.AbsoluteMinimum = 0;
                        break;
                }
            }
            m_plotModels.Add(NETScanPlotModel);

            MassScanPlotModel = new PlotModel
            {
                Title = "Monoisotopic Mass Vs. Scan",
                IsLegendVisible = true,
                LegendTitle = "LEGEND",
                LegendPosition = LegendPosition.RightMiddle,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendBackground = OxyColors.Transparent,
            };

            MassScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            MassScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            foreach (var axis in MassScanPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "MonoisotopicMass";
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "Scan";
                        axis.AbsoluteMinimum = 0;
                        break;
                }
            }
            m_plotModels.Add(MassScanPlotModel);

            ObservedPredictedPlotModel = new PlotModel
            {
                Title = "Predicted Net Vs. Observed Net",
                IsLegendVisible = true,
                LegendTitle = "LEGEND",
                LegendPosition = LegendPosition.RightMiddle,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendBackground = OxyColors.Transparent,
            };

            ObservedPredictedPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            ObservedPredictedPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            foreach (var axis in ObservedPredictedPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "Predicted Net";
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "Observed Net";
                        axis.AbsoluteMinimum = 0;
                        break;
                }
            }
            m_plotModels.Add(ObservedPredictedPlotModel);
            UpdatePlotViewModel(analysisJobViewModel);
        }

        public void UpdatePlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            AnalysisJobViewModel = analysisJobViewModel;
        }
    }
}
