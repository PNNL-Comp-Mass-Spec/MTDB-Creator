using System;
using System.Linq;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ProteinTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly ProteinInformation m_proteinInformation;

        public ProteinTreeNodeViewModel(ProteinInformation protein, TreeNodeViewModel parent)
            : base(String.Format("{0} ({1} peptides)", protein.ProteinName, protein.Consensus.Count), true, parent)
        {
            m_proteinInformation = protein;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            var sorted = m_proteinInformation.Consensus.ToList();

            sorted.Sort();

            foreach (var ct in sorted)
            {
                m_ChildNodes.Add(new ConsensusTargetTreeNodeViewModel(ct, this));
            }
        }
    }
}
