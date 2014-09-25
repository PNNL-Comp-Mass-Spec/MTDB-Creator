using System;
using System.Collections.Generic;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ConsensusTargetTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly ConsensusTarget m_consensusTarget;

        public ConsensusTargetTreeNodeViewModel(ConsensusTarget ct, TreeNodeViewModel parent)
            : base(String.Format("{0} ({1} evidences)", ct.EncodedNumericSequence, ct.Evidences.Count), true, parent)
        {
            m_consensusTarget = ct;
        }
        
        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Theoretical Monoisotopic Mass: ", m_consensusTarget.TheoreticalMonoIsotopicMass.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Average Net: ", m_consensusTarget.AverageNet.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Stdev Net: ", m_consensusTarget.StdevNet.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Predicted NET: ", m_consensusTarget.PredictedNet.ToString()), this));
            //m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Peptide Sequence: ", m_consensusTarget.Sequence), this));

            m_ChildNodes.Add(new TargetCollectionTreeNodeViewModel(m_consensusTarget.Evidences, this));
        }
    }
}
