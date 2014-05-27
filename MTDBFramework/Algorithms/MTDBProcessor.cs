#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;
using PNNLOmics.Algorithms.Regression;
using PNNLOmics.Annotations;
using PNNLOmics.Data.Features;
using Remotion.Linq.Collections;

#endregion

namespace MTDBFramework.Algorithms
{
    public class MtdbProcessor : IProcessor
    {
        private static int UmcScanComparison(UMCLight umc1, UMCLight umc2)
        {
            return umc1.Scan.CompareTo(umc2.Scan);
        }

        private static int EvidenceScanComparison(Evidence ev1, Evidence ev2)
        {
            return ev1.Scan.CompareTo(ev2.Scan);
        }

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
            var evidenceMap     = new Dictionary<int, Evidence>();
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
                    //var regressor = LinearRegressorFactory.Create(ProcessorOptions.RegressionType);
                    //dataSet.RegressionResult = regressor.CalculateRegression(alignedTargets.Select(t => (double)t.Scan).ToList(), alignedTargets.Select(t => t.PredictedNet).ToList());
                }
                else
                {
                    //dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                }
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
            foreach (var evidence in tempConsensusTargets) //targetDatabase.ConsensusTargets)
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
            var options = new LcmsWarpAlignmentOptions();
            var lcmsAligner = new LcmsWarpAdapter(options);
            int count = 1;

            //Foreach dataset
            foreach (var dataSet in dataSets)
            {
                dataSet.Evidences.Sort(EvidenceScanComparison);
                var umcDataset = new List<UMCLight>();
                var prePath = string.Format(@"d:\preAlign{0}.txt", count);
                var postPath = string.Format(@"d:\postAlign{0}.txt", count);
                using (var writer = new StreamWriter(prePath))
                {
                    foreach (var evidence in dataSet.Evidences)
                    {

                        var umcData = new UMCLight
                        {
                            Net = evidence.ObservedNet,
                            ChargeState = evidence.Charge,
                            Mz = evidence.Mz,
                            Scan = evidence.Scan,
                            MassMonoisotopic = evidence.MonoisotopicMass,
                            MassMonoisotopicAligned = evidence.MonoisotopicMass,
                            Id = evidence.Id,
                        };
                        writer.WriteLine(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}", "\t",
                                                        umcData.Net,
                                                        umcData.ChargeState,
                                                        umcData.Mz,
                                                        umcData.Scan,
                                                        umcData.MassMonoisotopic,
                                                        umcData.MassMonoisotopicAligned,
                                                        umcData.Id));
                        umcDataset.Add(umcData);
                    }
                }

                //Convert the list of evidences to UMCLights for LCMS to use as alignee
                //Align the LCMS                
                // alignmentData.Add(lcmsAligner.AlignFeatures(massTagLightTargets, umcDataset, options));

                //TODO: Make this a better sorter... bubble sort = booooooo...
                //for (int a = 0; a < (umcDataset.Count - 1); a ++)
                //{
                //    for (int b = 0; b < (umcDataset.Count - a - 1); b++)
                //    {
                //        if (umcDataset[b].Scan > umcDataset[b + 1].Scan)
                //        {
                //            var temp = umcDataset[b];
                //            umcDataset[b] = umcDataset[b + 1];
                //            umcDataset[b + 1] = temp;
                //        }
                //    }
                //}

                umcDataset.Sort(UmcScanComparison);

                var alignedData = lcmsAligner.Align(massTagLightTargets, umcDataset);
                alignmentData.Add(alignedData);
                var residualList = new List<UMCLight> {Capacity = alignedData.ResidualData.Mz.Length};
                //Put the residual data into a list of UMCLights
                for (int a = 0; a < alignedData.ResidualData.Mz.Length; a++)
                {
                    var residual = new UMCLight
                    {
                        MassMonoisotopic    = alignedData.ResidualData.MassErrorCorrected[a],
                        Net                 = alignedData.ResidualData.LinearCustomNet[a],
                        Scan                = Convert.ToInt32(alignedData.ResidualData.Scan[a])
                    };
                    //Not actually monoisotopic mass here, it's the corrected massError from the warping
                    residualList.Add(residual);
                }

                //Sort the residual data by scan (so it's sorted the same way as initial list)
                //TODO: Make this a better sorter... bubble sort = booooooo...
                //for (int a = 0; a < (residualList.Count - 1); a++)
                //{
                //    for (int b = 0; b < (residualList.Count - a - 1); b++)
                //    {
                //        if (residualList[b].Scan > residualList[b+1].Scan)
                //        {
                //            var temp = residualList[b + 1];
                //            residualList[b + 1] = residualList[b];
                //            residualList[b] = temp;
                //        }
                //    }
                //}

                residualList.Sort(UmcScanComparison);

                //TODO: Make this a better sorter... bubble sort = booooooo...
                //for (int a = 0; a < (umcDataset.Count - 1); a++)
                //{
                //    for (int b = 0; b < (umcDataset.Count - a - 1); b++)
                //    {
                //        if (umcDataset[b].Scan > umcDataset[b + 1].Scan)
                //        {
                //            var temp = umcDataset[b + 1];
                //            umcDataset[b + 1] = umcDataset[b];
                //            umcDataset[b] = temp;
                //        }
                //    }
                //}

                umcDataset.Sort(UmcScanComparison);

                //Copy the residual data back into the umcDataset
                for (int a = 0; a < residualList.Count; a++)
                {
                    umcDataset[a].MassMonoisotopicAligned   = umcDataset[a].MassMonoisotopic -
                                                                residualList[a].MassMonoisotopic;
                    umcDataset[a].Net                       = residualList[a].Net;

                    dataSet.Evidences[a].MonoisotopicMass   = dataSet.Evidences[a].MonoisotopicMass -
                                                                residualList[a].MassMonoisotopic;
                    dataSet.Evidences[a].ObservedNet        = residualList[a].Net;
                }

                foreach(var data in dataSet.Evidences)
                {
                    if(!evidenceMap.ContainsKey(data.Id))
                    {
                        evidenceMap.Add(data.Id, data);
                    }
                }

                using (var writer = new StreamWriter(postPath))
                {
                    foreach (var umcData in umcDataset)
                    {
                        writer.WriteLine(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}", "\t",
                                                        umcData.Net,
                                                        umcData.ChargeState,
                                                        umcData.Mz,
                                                        umcData.Scan,
                                                        umcData.MassMonoisotopic,
                                                        umcData.MassMonoisotopicAligned,
                                                        umcData.Id));
                    }
                }
                //dataSet.RegressionResult.Intercept  = alignedData.NetIntercept;
                //dataSet.RegressionResult.Slope      = alignedData.NetSlope;
                //dataSet.RegressionResult.RSquared   = alignedData.NetRsquared;
                //alignmentData.Add(lcmsAligner.AlignFeatures(massTagLightTargets, umcDataset, options));
                count++;
            }

            if (AlignmentComplete != null)
            {
                AlignmentComplete(this, new AlignmentCompleteArgs(alignmentData));
            }

            foreach(var consensus in tempConsensusTargets)
            {
                for (int evNum = 0; evNum < consensus.Evidences.Count; evNum++)
                {
                    consensus.Evidences[evNum] = evidenceMap[consensus.Evidences[evNum].Id];
                }
            }

            return targetDatabase;
            //TODO: See if this fixes, if so, remove following code

            clusterer = TargetClustererFactory.Create(ProcessorOptions.TargetFilterType);
            targetDatabase = new TargetDatabase();
            epicTargets.Clear();
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
                    //var regressor = LinearRegressorFactory.Create(ProcessorOptions.RegressionType);
                    //dataSet.RegressionResult = regressor.CalculateRegression(alignedTargets.Select(t => (double)t.Scan).ToList(), alignedTargets.Select(t => t.PredictedNet).ToList());
                }
                else
                {
                    //dataSet.RegressionResult = aligner.AlignTargets(filteredTargets, alignedTargets);
                }
            }



            //TODO: See if this fixes, if so, remove following code

            // Cluster again to provide accurate cluster data
            var finalTargets = clusterer.Cluster(epicTargets);
            i = 0;
            //j = 0;



            foreach (var consensusTarget in finalTargets)
            {
                consensusTarget.Id = ++i;

                //foreach (var target in consensusTarget.Evidences)
                //{
                //    target.Id = ++j;
                //}
                consensusTarget.CalculateStatistics();
                targetDatabase.AddConsensusTarget(consensusTarget);
            } 

            if (AlignmentComplete != null)                
            {
                AlignmentComplete(this, new AlignmentCompleteArgs(alignmentData));
            }
          
            return targetDatabase;
        }
    }
}
