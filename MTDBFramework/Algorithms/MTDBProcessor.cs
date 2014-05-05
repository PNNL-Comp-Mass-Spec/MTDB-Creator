#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;
using PNNLOmics.Data.Features;
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

            //Create the database (the list of consensus targets)
            //var clustered = clusterer.Cluster(epicTargets);
            //Convert the list of targets into a list of MassTagLights for LCMS to use as baseline

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


            

            var massTagLightTargets = new List<UMCLight>();
            foreach (var evidence in targetDatabase.ConsensusTargets)
            {
                var driftStart = double.MaxValue;
                var driftEnd = double.MinValue;

                foreach (var member in evidence.Evidences)
                {
                    driftStart = Math.Min(member.Scan, driftStart);
                    driftEnd = Math.Max(member.Scan, driftEnd);
                }

                foreach (var charge in evidence.Charges)
                {

                    var consensus = new UMCLight
                    {
                        NETAligned = evidence.Net,
                        NET = evidence.Net,
                        ChargeState = charge,
                        MassMonoisotopic = evidence.TheoreticalMonoIsotopicMass,
                        ID = evidence.Id,
                        MassMonoisotopicAligned = evidence.TheoreticalMonoIsotopicMass,
                        DriftTime = driftEnd - driftStart
                    };
                    massTagLightTargets.Add(consensus);
                }
            }

            var alignmentData = new List<LcmsWarpAlignmentData>();
            var lcmsAligner = new LcmsWarpAdapter();
            //Foreach dataset
            foreach (var dataSet in dataSets)
            {
                var umcDataset = new List<UMCLight>();
                foreach (var evidence in dataSet.Evidences)
                {
                    var umcData = new UMCLight
                    {
                        NET = evidence.ObservedNet,
                        ChargeState = evidence.Charge,
                        Mz = evidence.Mz,
                        Scan = evidence.Scan,
                        MassMonoisotopic = evidence.MonoisotopicMass,
                        ID = evidence.Id,
                    };
                    umcDataset.Add(umcData);

                }
                //Convert the list of evidences to UMCLights for LCMS to use as alignee
                //Align the LCMS
                var options = new LcmsWarpAlignmentOptions{AlignType = LcmsWarpAlignmentOptions.AlignmentType.NET_WARP};
                alignmentData.Add(lcmsAligner.AlignFeatures(massTagLightTargets, umcDataset, options));
            }
            int alignmentNum = 1;
            foreach (var alignment in alignmentData)
            {
                
                string filePath = string.Format("C:\\alignmentResults\\results{0}.csv", alignmentNum);

                using (var write = new StreamWriter(filePath))
                {
                    write.WriteLine("Linear Net Rsquared, Alignment Time Scan, Alignment Scan Output, Alignment Net Output");
                    write.WriteLine(string.Format("{0}, {1}, {2}, {3}", alignment.NetRsquared,
                        alignment.AlignmentFunction.NetFuncTimeInput[0],
                        alignment.AlignmentFunction.NetFuncTimeOutput[0], 
                        alignment.AlignmentFunction.NetFuncNetOutput[0]));
                    for (int line = 1; line < alignment.AlignmentFunction.NetFuncTimeInput.Count; line++)
                    {
                        write.WriteLine(string.Format(", {0}, {1}, {2}", 
                         alignment.AlignmentFunction.NetFuncTimeInput[line],
                         alignment.AlignmentFunction.NetFuncTimeOutput[line],
                         alignment.AlignmentFunction.NetFuncNetOutput[line])); 
                    }
                }
            }

            return targetDatabase;
        }
    }
}
