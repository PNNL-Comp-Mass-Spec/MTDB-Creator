using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Algorithms.Clustering;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ConsensusTargetTreeNodeViewModel : TreeNodeViewModel
    {
        private ConsensusTarget m_ConsensusTarget;

        public ConsensusTargetTreeNodeViewModel(ConsensusTarget ct, TreeNodeViewModel parent)
            : base(String.Format("Consensus Target ({0})", ct.Evidences.Count), true, parent)
        {
            m_ConsensusTarget = ct;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Theoretical Monoisotopic Mass: ", m_ConsensusTarget.TheoreticalMonoIsotopicMass.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Average Net: ", m_ConsensusTarget.Net.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Stdev Net: ", m_ConsensusTarget.StdevNet.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Predicted NET: ", m_ConsensusTarget.PredictedNet.ToString()), this));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Peptide Sequence: ", m_ConsensusTarget.Sequence), this));

            m_ChildNodes.Add(new TargetCollectionTreeNodeViewModel(m_ConsensusTarget.Evidences, this));
        }
    }
}
