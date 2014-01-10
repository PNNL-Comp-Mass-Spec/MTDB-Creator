using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MTDBCreator.Helpers;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using OxyPlot.Wpf;
using Axis = OxyPlot.Axes.Axis;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using ScatterSeries = OxyPlot.Series.ScatterSeries;

namespace MTDBCreator.ViewModels
{
    public class StatPlotViewModel : ObservableObject
    {
        public PlotModel StdevMassPlotModel { get; set; }
        public PlotModel StdevNETPlotModel { get; set; }

        public StatPlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            this.StdevMassPlotModel = MakePlotModel("Stdev Mass");
            this.StdevNETPlotModel = MakePlotModel("Stdev NET");

            foreach (Axis axis in this.StdevMassPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "Count";
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "Mass";
                        break;
                }
            }

            this.StdevMassPlotModel.Series.Add(MakeStdevMassScatterSeries(analysisJobViewModel.Database));

            foreach (Axis axis in this.StdevNETPlotModel.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.Title = "Count";
                        break;
                    case AxisPosition.Bottom:
                        axis.Title = "NET";
                        break;
                }
            }

            this.StdevMassPlotModel.Series.Add(MakeStdevMassScatterSeries(analysisJobViewModel.Database));
            this.StdevNETPlotModel.Series.Add(MakeStdevNETScatterSeries(analysisJobViewModel.Database));
        }

        public static PlotModel MakePlotModel(string title)
        {
            PlotModel plotModel = new PlotModel(title);

            plotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            plotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            return plotModel;
        }

        public static LinearAxis MakeLinerAxis(AxisPosition position)
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

        public static ScatterSeries MakeScatterSeries()
        {
            return new ScatterSeries()
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColors.DodgerBlue,
                MarkerType = MarkerType.Cross
            };
        }

        public static ScatterSeries MakeStdevMassScatterSeries(TargetDatabase targetDatabase)
        {
            ScatterSeries scatterSeries = MakeScatterSeries();

            foreach (ConsensusTarget ct in targetDatabase.ConsensusTargets)
            {
                scatterSeries.Points.Add(new ScatterPoint(ct.Mass, ct.Targets.Count));
            }

            return scatterSeries;
        }

        public static ScatterSeries MakeStdevNETScatterSeries(TargetDatabase targetDatabase)
        {
            ScatterSeries scatterSeries = MakeScatterSeries();

            foreach (ConsensusTarget ct in targetDatabase.ConsensusTargets)
            {
                scatterSeries.Points.Add(new ScatterPoint(ct.Net, ct.Targets.Count));
            }

            return scatterSeries;
        }
    }
}
