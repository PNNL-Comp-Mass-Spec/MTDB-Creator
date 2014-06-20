#region Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool m_abortRequested;
        private const double CarryOverThreshold = 0.05;

        public event EventHandler<AlignmentCompleteArgs> AlignmentComplete;

        public MtdbProcessor(Options options)
        {
            ProcessorOptions = options;
        }

        public Options ProcessorOptions { get; set; }

        public void AbortProcessing()
        {
            m_abortRequested = true;
        }

        [UsedImplicitly]
        public TargetDatabase Process(IEnumerable<LcmsDataSet> dataSets, BackgroundWorker bWorker)
        {
            m_abortRequested = false;

            // Deal with DataSetId - Auto increments - Not in this class only
            var evidenceMap = new Dictionary<int, Evidence>();
            var targetDatabase = new TargetDatabase();
            var aligner = TargetAlignmentFactory.Create(ProcessorOptions);
            var clusterer = TargetClustererFactory.Create(ProcessorOptions.TargetFilterType);
            var epicTargets = new List<Evidence>();

            dataSets = dataSets.ToList();
            foreach (var dataSet in dataSets)
            {
                if (bWorker.CancellationPending || m_abortRequested)
                    return targetDatabase;

                var targetFilter = TargetFilterFactory.Create(dataSet.Tool, ProcessorOptions);
                var alignmentFilter = AlignmentFilterFactory.Create(dataSet.Tool, ProcessorOptions);

                var filteredTargets = new List<Evidence>();
                var alignedTargets = new List<Evidence>();

                foreach (var t in dataSet.Evidences)
                {
                    // Exclude carryover peptides.
                    // Would be evidenced by a sizable difference between observed net and predicted net
                    if (t.ObservedNet > CarryOverThreshold)
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
                }

                epicTargets.AddRange(filteredTargets);

                

                if (ProcessorOptions.TargetFilterType == TargetWorkflowType.TOP_DOWN)
                {
                    dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                }

                //const bool PERFORM_LINEAR_REGRESSION = false;
                //if (PERFORM_LINEAR_REGRESSION)
                //{
                //    if (ProcessorOptions.TargetFilterType != TargetWorkflowType.TOP_DOWN)
                //    {
                //        var regressor = LinearRegressorFactory.Create(ProcessorOptions.RegressionType);
                //        dataSet.RegressionResult =
                //            regressor.CalculateRegression(alignedTargets.Select(t => (double)t.Scan).ToList(),
                //                                          alignedTargets.Select(t => t.PredictedNet).ToList());
                //    }
                //    else
                //    {
                //        dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                //    }
                //}

            }

            //Create the database (the list of consensus targets)            
            //Convert the list of targets into a list of MassTagLights for LCMS to use as baseline

            // Cluster initially to provide a baseline for LCMSWarp
            var newTargets = clusterer.Cluster(epicTargets);
            int i = 0, j = 0;
            var tempConsensusTargets = new List<ConsensusTarget>();
            foreach (var consensusTarget in newTargets)
            {
                consensusTarget.Id = ++i;

                foreach (var target in consensusTarget.Evidences)
                {
                    target.Id = ++j;
                }
                consensusTarget.CalculateStatistics();
                tempConsensusTargets.Add(consensusTarget);
                targetDatabase.AddConsensusTarget(consensusTarget);
            }

            var massTagLightTargets = new List<UMCLight>();
            foreach (var evidence in tempConsensusTargets)
            {
                var driftStart = double.MaxValue;
                var driftEnd = double.MinValue;

                foreach (var member in evidence.Evidences)
                {
                    driftStart = Math.Min(member.Scan, driftStart);
                    driftEnd = Math.Max(member.Scan, driftEnd);
                }

                massTagLightTargets.AddRange(evidence.Charges.Select(charge => new UMCLight
                {
                    Net = evidence.Net,
                    ChargeState = charge,
                    Mz = (evidence.TheoreticalMonoIsotopicMass / charge),
                    MassMonoisotopic = evidence.TheoreticalMonoIsotopicMass,
                    Id = evidence.Id,
                    MassMonoisotopicAligned = evidence.TheoreticalMonoIsotopicMass,
                    DriftTime = driftEnd - driftStart,
                    Scan = (int)((driftStart + driftEnd) / 2.0),
                    ScanStart = (int)driftStart,
                    ScanEnd = (int)driftEnd,
                }));
            }

            if (bWorker.CancellationPending || m_abortRequested)
                return targetDatabase;

            var alignmentData = new List<LcmsWarpAlignmentData>();
            var options = new LcmsWarpAlignmentOptions();
            var lcmsAligner = new LcmsWarpAdapter(options);

            //For performing net warping without mass correction
            options.AlignType = LcmsWarpAlignmentOptions.AlignmentType.NET_WARP;
            var lcmsNetAligner = new LcmsWarpAdapter(options);
            

            //Foreach dataset
            foreach (var dataSet in dataSets)
            {
                if (bWorker.CancellationPending || m_abortRequested)
                    return targetDatabase;

                var umcDataset = new List<UMCLight>();
                if (dataSet.Tool == LcmsIdentificationTool.MSAlign)
                {
                    continue;
                }

                dataSet.Evidences.Sort((x, y) => x.Scan.CompareTo(y.Scan));

                var backupDataset = new List<UMCLight>();
                foreach (var evidence in dataSet.Evidences)
                {
                    if (evidence.ObservedNet > CarryOverThreshold)
                    {
                        UMCLight umc;
                        {
                            umc = new UMCLight
                            {
                                Net = evidence.ObservedNet,
                                ChargeState = evidence.Charge,
                                Mz = evidence.Mz,
                                Scan = evidence.Scan,
                                MassMonoisotopic = evidence.MonoisotopicMass,
                                MassMonoisotopicAligned = evidence.MonoisotopicMass,
                                Id = evidence.Id,
                                ScanStart = evidence.Scan,
                                ScanEnd = evidence.Scan,
                            };
                        }
                        umcDataset.Add(umc);
                        backupDataset.Add(umc);
                    }
                }
                umcDataset.Sort((x, y) => x.MassMonoisotopic.CompareTo(y.MassMonoisotopic));
                var alignedData = lcmsAligner.Align(massTagLightTargets, umcDataset);
                if (umcDataset.Max(x => x.NetAligned) < 0.6)
                {
                    umcDataset = backupDataset;
                    alignedData = lcmsNetAligner.Align(massTagLightTargets, umcDataset);
                }
                
                //var alignedData = lcmsAligner.Align(datasetMassTagLight, umcDataset);
                alignmentData.Add(alignedData);
                //var residualList = new List<UMCLight> { Capacity = alignedData.ResidualData.Mz.Length };
                ////Put the residual data into a list of UMCLights
                //for (var a = 0; a < alignedData.ResidualData.Mz.Length; a++)
                //{
                //    var residual = new UMCLight
                //    {
                //        MassMonoisotopic = alignedData.ResidualData.MassErrorCorrected[a],
                //        Net = alignedData.ResidualData.LinearCustomNet[a],
                //        Scan = Convert.ToInt32(alignedData.ResidualData.Scan[a])
                //    };
                //    //Not actually monoisotopic mass here, it's the corrected massError from the warping
                //    residualList.Add(residual);
                //}

                //residualList.Sort(UmcScanComparison);
                
                //foreach (var umc in umcDataset)
                //{
                //    firstSeenPeptide[umc.]
                //}
                umcDataset.Sort((x, y) => x.ScanAligned.CompareTo(y.ScanAligned));
                
                //Copy the residual data back into the evidences
                for (int a = 0, b = 0; a < dataSet.Evidences.Count; a++)
                {
                    if (dataSet.Evidences[a].ObservedNet > CarryOverThreshold)
                    {
                        dataSet.Evidences[a].MonoisotopicMass = umcDataset[b].MassMonoisotopicAligned;
                        dataSet.Evidences[a].ObservedNet = umcDataset[b].NetAligned;
                        b++;
                    }
                }

                //foreach (var evidence in dataSet.Evidences)
                //{
                //    UMCLight value;
                //    firstSeenPeptide.TryGetValue(evidence.Sequence, out value);
                //    if (value != null)
                //    {
                //        evidence.MonoisotopicMass = value.MassMonoisotopicAligned;
                //        evidence.ObservedNet = value.NetAligned;
                //    }
                //}

                foreach (var data in dataSet.Evidences.Where(data => !evidenceMap.ContainsKey(data.Id)))
                {
                    evidenceMap.Add(data.Id, data);
                }
                dataSet.RegressionResult.Slope = alignedData.NetSlope;
                dataSet.RegressionResult.Intercept = alignedData.NetIntercept;
                dataSet.RegressionResult.RSquared = alignedData.NetRsquared;
            }
          
            if (AlignmentComplete != null)
            {
                AlignmentComplete(this, new AlignmentCompleteArgs(alignmentData));
            }
            if (ProcessorOptions.TargetFilterType != TargetWorkflowType.TOP_DOWN)
            {
                foreach (var consensus in tempConsensusTargets)
                {
                    for (var evNum = 0; evNum < consensus.Evidences.Count; evNum++)
                    {
                        consensus.Evidences[evNum] = evidenceMap[consensus.Evidences[evNum].Id];
                    }
                }
            }

            return targetDatabase;
        }
    }
}
