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
    public class MtdbProcessor : IProcessor
    {
        public Options ProcessorOptions { get; set; }

        public MtdbProcessor(Options options)
        {
            ProcessorOptions = options;
        }

        public TargetDatabase Process(IEnumerable<LcmsDataSet> dataSets)
        {
            // Deal with DataSetId - Auto increments - Not in this class only

            var targetDatabase = new TargetDatabase();

            ITargetAligner aligner = TargetAlignmentFactory.Create(ProcessorOptions);
            ITargetClusterer clusterer = TargetClustererFactory.Create(ProcessorOptions.TargetFilterType);

            var epicTargets = new List<Evidence>();

            foreach (LcmsDataSet dataSet in dataSets)
            {
                ITargetFilter targetFilter = TargetFilterFactory.Create(dataSet.Tool, ProcessorOptions);
                ITargetFilter alignmentFilter = AlignmentFilterFactory.Create(dataSet.Tool, ProcessorOptions);

                var filteredTargets = new List<Evidence>();
                var alignedTargets = new List<Evidence>();

                foreach (Evidence t in dataSet.Evidences)
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

                    if (ProcessorOptions.TargetFilterType != TargetWorkflowType.TOP_DOWN)
                    {
                        IRegressorAlgorithm<LinearRegressionResult> regressor = LinearRegressorFactory.Create(ProcessorOptions.RegressionType);
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

                foreach (Evidence t in consensusTarget.Evidences)
                {
                    t.Id = ++j;
                }
                consensusTarget.CalculateStatistics();
            }

            return targetDatabase;
        }
    }
}
