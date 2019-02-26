using System.Globalization;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ConsensusTargetTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly ConsensusTarget m_consensusTarget;

        public ConsensusTargetTreeNodeViewModel(ConsensusTarget ct, TreeNodeViewModel parent)
            : base(string.Format("{0} ({1} evidences)", ct.EncodedNumericSequence, ct.Evidences.Count), true, parent)
        {
            m_consensusTarget = ct;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Theoretical Monoisotopic Mass: ", m_consensusTarget.TheoreticalMonoIsotopicMass.ToString(CultureInfo.InvariantCulture)), this));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Average Net: ", m_consensusTarget.AverageNet.ToString(CultureInfo.InvariantCulture)), this));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Stdev Net: ", m_consensusTarget.StdevNet.ToString(CultureInfo.InvariantCulture)), this));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Predicted NET: ", m_consensusTarget.PredictedNet.ToString(CultureInfo.InvariantCulture)), this));

            m_ChildNodes.Add(new TargetCollectionTreeNodeViewModel(m_consensusTarget.Evidences, this));
        }
    }
}
