using MTDBFramework.Database;
using MTDBFramework.UI;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MTDBCreator.ViewModels
{
    public class StatPlotViewModel : ObservableObject
    {
        public PlotModel StdevMassPlotModel { get; set; }
        public PlotModel StdevNetPlotModel { get; set; }

        public StatPlotViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            StdevMassPlotModel = MakePlotModel("Stdev Mass");
            StdevNetPlotModel = MakePlotModel("Stdev NET");

            foreach (var axis in StdevMassPlotModel.Axes)
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

            StdevMassPlotModel.Series.Add(MakeStdevMassScatterSeries(analysisJobViewModel.Database));

            foreach (var axis in StdevNetPlotModel.Axes)
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

            StdevMassPlotModel.Series.Add(MakeStdevMassScatterSeries(analysisJobViewModel.Database));
            StdevNetPlotModel.Series.Add(MakeStdevNetScatterSeries(analysisJobViewModel.Database));
        }

        public static PlotModel MakePlotModel(string title)
        {
            var plotModel = new PlotModel
            {
                Title = title
            };

            plotModel.Axes.Add(MakeLinerAxis(AxisPosition.Left));
            plotModel.Axes.Add(MakeLinerAxis(AxisPosition.Bottom));

            return plotModel;
        }

        public static LinearAxis MakeLinerAxis(AxisPosition position)
        {
            return new LinearAxis
            {
                Position = position,
                TitleFontSize = 14,
                TitleFontWeight = FontWeights.Bold,
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
            return new ScatterSeries
            {
                MarkerSize = 2,
                // Use Cross MarkerType and MarkerStroke (instead of MarkerFill) to improve the graphing performance
                MarkerStroke = OxyColors.DodgerBlue,
                MarkerType = MarkerType.Cross
            };
        }

        public static ScatterSeries MakeStdevMassScatterSeries(TargetDatabase targetDatabase)
        {
            var scatterSeries = MakeScatterSeries();

            foreach (var ct in targetDatabase.ConsensusTargets)
            {
                scatterSeries.Points.Add(new ScatterPoint(ct.TheoreticalMonoIsotopicMass, ct.Evidences.Count));
            }

            return scatterSeries;
        }

        public static ScatterSeries MakeStdevNetScatterSeries(TargetDatabase targetDatabase)
        {
            var scatterSeries = MakeScatterSeries();

            foreach (var ct in targetDatabase.ConsensusTargets)
            {
                scatterSeries.Points.Add(new ScatterPoint(ct.Net, ct.Evidences.Count));
            }

            return scatterSeries;
        }
    }
}
