﻿#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;
using PNNLOmics.Algorithms.Regression;
using PNNLOmics.Annotations;
using PNNLOmics.Data.Features;

#endregion

namespace MTDBFramework.Algorithms
{
    public class MtdbProcessor : IProcessor
    {
        public event EventHandler<AlignmentCompleteArgs> AlignmentComplete;
        
        public MtdbProcessor(Options options)
        {
            ProcessorOptions = options;
        }

        public Options ProcessorOptions { get; set; }

        [UsedImplicitly]
        public TargetDatabase Process(IEnumerable<LcmsDataSet> dataSets)
        {
            // Deal with DataSetId - Auto increments - Not in this class only

            var targetDatabase  = new TargetDatabase();
            var aligner         = TargetAlignmentFactory.Create(ProcessorOptions);
            var clusterer       = TargetClustererFactory.Create(ProcessorOptions.TargetFilterType);
            var epicTargets     = new List<Evidence>();
            
            foreach (var dataSet in dataSets)
            {
                var targetFilter = TargetFilterFactory.Create(dataSet.Tool, ProcessorOptions);
                var alignmentFilter = AlignmentFilterFactory.Create(dataSet.Tool, ProcessorOptions);

                var filteredTargets = new List<Evidence>();
                var alignedTargets = new List<Evidence>();

                foreach (var t in dataSet.Evidences)
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
                    var regressor = LinearRegressorFactory.Create(ProcessorOptions.RegressionType);
                    dataSet.RegressionResult = regressor.CalculateRegression(alignedTargets.Select(t => (double)t.Scan).ToList(), alignedTargets.Select(t => t.PredictedNet).ToList());
                }
                else
                {
                    dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                }
            }

            //Create the database (the list of consensus targets)            
            //Convert the list of targets into a list of MassTagLights for LCMS to use as baseline
            var newTargets = clusterer.Cluster(epicTargets);                        
            int i = 0, j = 0;

            foreach (var consensusTarget in newTargets)
            {
                consensusTarget.Id = ++i;

                foreach (var target in consensusTarget.Evidences)
                {
                    target.Id = ++j;
                }
                consensusTarget.CalculateStatistics();
                targetDatabase.AddConsensusTarget(consensusTarget);
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
                        NetAligned = evidence.Net,
                        Net = evidence.Net,
                        ChargeState = charge,
                        MassMonoisotopic = evidence.TheoreticalMonoIsotopicMass,
                        Id = evidence.Id,
                        MassMonoisotopicAligned = evidence.TheoreticalMonoIsotopicMass,
                        DriftTime = driftEnd - driftStart
                    };
                    massTagLightTargets.Add(consensus);
                }
            }

            var alignmentData = new List<LcmsWarpAlignmentData>();
            var options = new LcmsWarpAlignmentOptions { AlignType = LcmsWarpAlignmentOptions.AlignmentType.NET_WARP };
            var lcmsAligner = new LcmsWarpAdapter(options);

            //Foreach dataset
            foreach (var dataSet in dataSets)
            {
                var umcDataset = new List<UMCLight>();
                foreach (var evidence in dataSet.Evidences)
                {
                    var umcData = new UMCLight
                    {
                        Net = evidence.ObservedNet,
                        ChargeState = evidence.Charge,
                        Mz = evidence.Mz,
                        Scan = evidence.Scan,
                        MassMonoisotopic = evidence.MonoisotopicMass,
                        Id = evidence.Id,
                    };
                    umcDataset.Add(umcData);

                }
                //Convert the list of evidences to UMCLights for LCMS to use as alignee
                //Align the LCMS                
                // alignmentData.Add(lcmsAligner.AlignFeatures(massTagLightTargets, umcDataset, options));
                
                var alignedData = lcmsAligner.Align(massTagLightTargets, umcDataset);
                alignmentData.Add(alignedData);

                //alignmentData.Add(lcmsAligner.AlignFeatures(massTagLightTargets, umcDataset, options));
            }

            if (AlignmentComplete != null)                
            {
                AlignmentComplete(this, new AlignmentCompleteArgs(alignmentData));
            }
          
            return targetDatabase;
        }
    }
}