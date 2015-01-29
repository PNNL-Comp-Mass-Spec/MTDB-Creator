#region Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MathNet.Numerics.Statistics;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;
using PNNLOmics.Annotations;
using PNNLOmics.Data.Features;

#endregion

namespace MTDBFramework.Algorithms
{
	/// <summary>
	/// Perform the data transform from an LCMS dataset to MTDB
	/// </summary>
    public class MtdbProcessor : IProcessor
    {
        private const int ProgressPercentStart = 0;
        private const int ProgressPercentComplete = 100;

        private int m_currentItem;
        private int m_totalItems;

        private bool m_abortRequested;

		/// <summary>
		/// Event handler
		/// </summary>
        public event EventHandler<AlignmentCompleteArgs> AlignmentComplete;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public MtdbProcessor(Options options)
        {
            ProcessorOptions = options;
        }

		/// <summary>
		/// Options accessor
		/// </summary>
        public Options ProcessorOptions { get; set; }

		/// <summary>
		/// Allow for thread cancellation
		/// </summary>
        public void AbortProcessing()
        {
            m_abortRequested = true;
        }

		/// <summary>
		/// Main work function - Transform LCMS Datasets into MTDB datasets
		/// </summary>
		/// <param name="dataSets"></param>
		/// <param name="bWorker"></param>
		/// <returns></returns>
        [UsedImplicitly]
        public TargetDatabase Process(IEnumerable<LcmsDataSet> dataSets, BackgroundWorker bWorker)
        {
            m_abortRequested = false;
            m_currentItem = 0;
            m_totalItems = 2 * dataSets.Count();

            OnPercentProgressChanged(new PercentCompleteEventArgs(0));

            // Deal with DataSetId - Auto increments - Not in this class only
            var evidenceMap = new Dictionary<int, Evidence>();
            var targetDatabase = new TargetDatabase();
            var aligner = TargetAlignmentFactory.Create(ProcessorOptions);
            var clusterer = TargetClustererFactory.Create(ProcessorOptions.TargetFilterType);
            var epicTargets = new List<Evidence>();

            dataSets = dataSets.ToList();
            foreach (var dataSet in dataSets)
            {
                float percentComplete = (float)m_currentItem / m_totalItems;
                UpdateProgress(m_currentItem, m_totalItems, percentComplete, "Determining Consensus Targets");
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
                    
                    if (t.ObservedNet >= ProcessorOptions.MinimumObservedNet && 
                        t.ObservedNet <= ProcessorOptions.MaximumObservedNet)
                    {
                        
                        // To prevent filtration of evidences which have previously passed alignment, 
                        if (dataSet.PreviouslyAnalyzed || !targetFilter.ShouldFilter(t))
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

                m_currentItem++;
            }

            //Create the database (the list of consensus targets)            
            //Convert the list of targets into a list of MassTagLights for LCMS to use as baseline

            // Cluster initially to provide a baseline for LCMSWarp
            var newTargets = clusterer.Cluster(epicTargets);
            int i = 0, j = 0;
            var tempConsensusTargets = new List<ConsensusTarget>();
		    var proteinDict = new Dictionary<string, ProteinInformation>();
		    foreach (var consensusTarget in newTargets)
		    {
		        consensusTarget.Id = ++i;

		        foreach (var target in consensusTarget.Evidences)
		        {
		            target.Id = ++j;
		        }
		        consensusTarget.CalculateStatistics();
		        tempConsensusTargets.Add(consensusTarget);
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
                    Net = evidence.PredictedNet,
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
            options.AlignType = PNNLOmics.Algorithms.Alignment.LcmsWarp.AlignmentType.NET_WARP;
            var lcmsNetAligner = new LcmsWarpAdapter(options);

            //Foreach dataset
            foreach (var dataSet in dataSets)
            {
                float percentComplete = (float)m_currentItem / m_totalItems;
                UpdateProgress(m_currentItem, m_totalItems, percentComplete, "Performing LCMSWarp Alignment");
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
                    if (evidence.ObservedNet >= ProcessorOptions.MinimumObservedNet)
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
                LcmsWarpAlignmentData alignedData;
                try
                {
                    alignedData = lcmsAligner.Align(massTagLightTargets, umcDataset);
                }
                catch
                {
                    try
                    {
                        alignedData = lcmsNetAligner.Align(massTagLightTargets, umcDataset);
                    }
                    catch
                    {
                        alignedData = null;
                    }
                }

                umcDataset.Sort((x, y) => x.ScanAligned.CompareTo(y.ScanAligned));

                var netDiffList = new List<double>();
                var numBins     = Math.Min(50, dataSet.Evidences.Count);
                var medNetDiff  = new double[numBins];
                var numPerBin   = (int)Math.Ceiling((double)dataSet.Evidences.Count / numBins);
                var binNum      = 0;

                
                //Copy the residual data back into the evidences
                for (int a = 0, b = 0; a < dataSet.Evidences.Count; a++)
                {
                    if (dataSet.Evidences[a].ObservedNet >= ProcessorOptions.MinimumObservedNet)
                    {
                        dataSet.Evidences[a].MonoisotopicMass = umcDataset[b].MassMonoisotopicAligned;
                        var netShift = umcDataset[b].NetAligned - umcDataset[b].Net;
                        netDiffList.Add(netShift);
                        dataSet.Evidences[a].NetShift = netShift;
                        dataSet.Evidences[a].ObservedNet += netShift;
                        
                        if (netDiffList.Count % numPerBin == 0)
                        {
                            medNetDiff[binNum] = netDiffList.Median();
                            netDiffList.Clear();
                            binNum++;
                        }

                        b++;
                    }
                }
                if (netDiffList.Count != 0)
                {
                    medNetDiff[binNum] = netDiffList.Median();
                    netDiffList.Clear();
                }

                
                foreach (var data in dataSet.Evidences.Where(data => !evidenceMap.ContainsKey(data.Id)))
                {
                    evidenceMap.Add(data.Id, data);
                }
                if (alignedData != null)
                {
                    dataSet.RegressionResult.Slope = alignedData.NetSlope;
                    dataSet.RegressionResult.Intercept = alignedData.NetIntercept;
                    dataSet.RegressionResult.RSquared = alignedData.NetRsquared;
                    alignmentData.Add(alignedData);
                }
                else
                {
                    dataSet.RegressionResult.Slope = 1;
                    dataSet.RegressionResult.Intercept = 0;
                    dataSet.RegressionResult.RSquared = 0;
                }
                m_currentItem++;
            }
            
            if (AlignmentComplete != null)
            {
                AlignmentComplete(this, new AlignmentCompleteArgs(alignmentData));
            }
            if (ProcessorOptions.TargetFilterType != TargetWorkflowType.TOP_DOWN)
            {
                i = j = 0;
                foreach (var consensus in tempConsensusTargets)
                {
                    for (var evNum = 0; evNum < consensus.Evidences.Count; evNum++)
                    {
                        consensus.Evidences[evNum] = evidenceMap[consensus.Evidences[evNum].Id];
                    }
                    //Recalculate the target's data from the warped values
                    consensus.Id = ++i;
                    foreach (var target in consensus.Evidences)
                    {
                        target.Id = ++j;
                    }
                    consensus.CalculateStatistics();
                    targetDatabase.AddConsensusTarget(consensus);
                    foreach (var protein in consensus.Proteins)
                    {
                        if (!proteinDict.ContainsKey(protein.ProteinName))
                        {
                            proteinDict.Add(protein.ProteinName, protein);
                            // Don't need to manually link the first consensus to the protein
                            continue;
                        }
                        proteinDict[protein.ProteinName].Consensus.Add(consensus);
                    }
                }
                targetDatabase.Proteins = proteinDict.Values.ToList();
            }
            return targetDatabase;
        }

		/// <summary>
		/// Progress handler
		/// </summary>
		/// <param name="current"></param>
		/// <param name="total"></param>
		/// <param name="percentComplete"></param>
		/// <param name="currentTask"></param>
        protected void UpdateProgress(int current, int total, float percentComplete, string currentTask)
        {
            float percentCompleteEffective = ProgressPercentStart +
                                             percentComplete * (ProgressPercentComplete - ProgressPercentStart);
            OnPercentProgressChanged(new PercentCompleteEventArgs(current, total, percentCompleteEffective, currentTask));
        }

		/// <summary>
		/// Progress event
		/// </summary>
        public PercentCompleteEventHandler ProgressChanged;

		/// <summary>
		/// Progress event handler
		/// </summary>
		/// <param name="e"></param>
        protected void OnPercentProgressChanged(PercentCompleteEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }
    }
}
