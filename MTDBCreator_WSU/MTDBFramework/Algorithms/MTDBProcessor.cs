#region Namespaces

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Algorithms
{
    public class MTDBProcessor : IProcessor
    {
        public Options ProcessorOptions { get; set; }

        public MTDBProcessor(Options options)
        {
            this.ProcessorOptions = options;
        }

        public TargetDatabase Process(IEnumerable<LcmsDataSet> dataSets)
        {
            // Deal with DataSetId - Auto increments - Not in this class only

            TargetDatabase targetDatabase = new TargetDatabase();

            ITargetAligner aligner = TargetAlignmentFactory.Create(this.ProcessorOptions);
            ITargetClusterer clusterer = TargetClustererFactory.Create(this.ProcessorOptions.TargetFilterType);

            List<Target> epicTargets = new List<Target>();

            foreach (LcmsDataSet dataSet in dataSets)
            {
                ITargetFilter targetFilter = TargetFilterFactory.Create(dataSet.Tool, this.ProcessorOptions);
                ITargetFilter alignmentFilter = AlignmentFilterFactory.Create(dataSet.Tool, this.ProcessorOptions);

                List<Target> filteredTargets = new List<Target>();
                List<Target> alignedTargets = new List<Target>();

                foreach (Target t in dataSet.Targets)
                {
                    if (!targetFilter.ShouldFilter(t))
                    {
                        filteredTargets.Add(t);

                        if (!alignmentFilter.ShouldFilter(t))
                        {
                            alignedTargets.Add(t);
                        }
                    }
                }

                epicTargets.AddRange(filteredTargets);

                if (this.ProcessorOptions.TargetFilterType != TargetWorkflowType.TopDown)
                {
                    IRegressorAlgorithm<LinearRegressionResult> regressor = LinearRegressorFactory.Create(this.ProcessorOptions.RegressionType);
                    dataSet.RegressionResult = regressor.CalculateRegression(alignedTargets.Select(t => (double)t.Scan).ToList(), alignedTargets.Select(t => t.PredictedNet).ToList());
                }
                else
                {
                    dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                }
            }

            targetDatabase.ConsensusTargets = new ObservableCollection<ConsensusTarget>(clusterer.Cluster(epicTargets));

            int i = 0, j = 0;

            foreach (ConsensusTarget consensusTarget in targetDatabase.ConsensusTargets)
            {
                consensusTarget.Id = ++i;

                foreach (Target t in consensusTarget.Targets)
                {
                    t.Id = ++j;
                }

                consensusTarget.CalculateStatistics();
            }

            return targetDatabase;
        }
    }
}
