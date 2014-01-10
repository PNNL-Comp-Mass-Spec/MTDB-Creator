using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Layout;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Data;
using MTDBFramework.UI;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace MTDBCreator.ViewModels
{
    public class DatasetPlotViewModel : ObservableObject
    {
        #region Private Fields

        private AnalysisJobViewModel m_AnalysisJobViewModel;
        private PlotModel m_NETScanPlotModel;

        private ICommand m_ZoomExtentsCommand;
        private ICommand m_SelectItemsCommand;

        private IEnumerable<AnalysisJobItem> m_CurrentAnalysisJobItems;
        private bool m_IsRegressionLineVisible = true;

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

                this.CurrentAnalysisJobItems = value.AnalysisJobItems;
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
                    FillAnalysisSeries(this.NETScanPlotModel, this.CurrentAnalysisJobItems, this.AnalysisJobViewModel.Options);
                    FillAnalysisAnnotations(this.NETScanPlotModel, this.CurrentAnalysisJobItems);
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

                if (this.CurrentAnalysisJobItems != null)
                {
                    FillAnalysisAnnotations(this.NETScanPlotModel, this.CurrentAnalysisJobItems);
                }
            }
        }

        #endregion

        #region Public Instance Methods

        public LinearAxis MakeLinerAxis(AxisPosition position)
        {
            return new LinearAxis()
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

        public ScatterSeries MakeAnalysisScatterSeries(AnalysisJobItem analysisJobItem, Options options)
        {
            Color color = this.m_SeriesColorDictionary[analysisJobItem.FilePath];

            ScatterSeries scatterSeries = new ScatterSeries()
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColor.FromArgb(color.A, color.R, color.G, color.B),
                MarkerType = MarkerType.Cross,
                Title = analysisJobItem.Title,
                Tag = analysisJobItem,
            };

            foreach (Target target in analysisJobItem.DataSet.Targets)
            {
                ITargetFilter filter = AlignmentFilterFactory.Create(analysisJobItem.Format, options);

                if (!filter.ShouldFilter(target))
                {
                    ScatterPoint scatterPoint = new ScatterPoint(target.Scan, target.PredictedNet)
                    {
                        Tag = target
                    };

                    scatterSeries.Points.Add(scatterPoint);
                }
            }

            return scatterSeries;
        }

        public LineAnnotation MakeRegressionLineAnnotation(AnalysisJobItem analysisJobItem)
        {
            return new LineAnnotation()
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

        public void FillAnalysisSeries(PlotModel plotModel, IEnumerable<AnalysisJobItem> analysisJobItems, Options options)
        {
            foreach (Series series in plotModel.Series)
            {
                series.IsVisible = false;
            }

            foreach (AnalysisJobItem analysisJobItem in analysisJobItems)
            {
                // Add new color for this item, if the color does not exist

                if (!m_SeriesColorDictionary.ContainsKey(analysisJobItem.FilePath))
                {
                    m_SeriesColorDictionary.Add(analysisJobItem.FilePath, GraphHelper.PickColor());
                }

                IEnumerable<Series> seriesList =
                    from s in plotModel.Series
                    where s.Tag == analysisJobItem
                    select s;

                if (seriesList.Any())
                {
                    foreach (Series series in seriesList)
                    {
                        series.IsVisible = true;
                    }
                }
                else
                {
                    plotModel.Series.Add(MakeAnalysisScatterSeries(analysisJobItem, options));
                }
            }

            plotModel.RefreshPlot(true);
        }

        public void FillAnalysisAnnotations(PlotModel plotModel, IEnumerable<AnalysisJobItem> analysisJobItems)
        {
            plotModel.Annotations.Clear();

            if (this.IsRegressionLineVisible)
            {
                foreach (AnalysisJobItem analysisJobItem in analysisJobItems)
                {
                    plotModel.Annotations.Add(MakeRegressionLineAnnotation(analysisJobItem));
                }
            }

            plotModel.RefreshPlot(true);
        }

        #endregion

        #region Command Methods

        private void ZoomExtents()
        {
            foreach (Axis axis in this.NETScanPlotModel.Axes)
            {
                axis.Reset();
            }
        }

        private void SelectItems(object param)
        {
            IEnumerable<AnalysisJobItem> selectedAnalysisJobItems = ((IEnumerable)param).Cast<AnalysisJobItem>().ToList();

            if (selectedAnalysisJobItems.Any())
            {
                this.CurrentAnalysisJobItems = selectedAnalysisJobItems;
            }
        }

        #endregion

        public DatasetPlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_SeriesColorDictionary = new Dictionary<string, Color>();

            this.NETScanPlotModel = new PlotModel()
            {
                Title = "Predicted NET Vs. Scan",
                IsLegendVisible = true,
                LegendTitle = "LEGEND",
                LegendPosition = LegendPosition.RightMiddle,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendBackground = OxyColors.Transparent,
            };

            this.NETScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            this.NETScanPlotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            foreach (Axis axis in this.NETScanPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "Predicted NET";
                        axis.AbsoluteMinimum = 0;
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "Scan";
                        axis.AbsoluteMinimum = 0;
                        break;
                }
            }

            UpdatePlotViewModel(analysisJobViewModel);
        }

        public void UpdatePlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            this.AnalysisJobViewModel = analysisJobViewModel;
        }
    }
}
