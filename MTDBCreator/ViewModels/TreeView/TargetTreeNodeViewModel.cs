using System;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetTreeNodeViewModel : TreeNodeViewModel
    {
        private Evidence m_Evidence;

        public TargetTreeNodeViewModel(Evidence t, TreeNodeViewModel parent)
            : base(t.Sequence, true, parent)
        {
            m_Evidence = t;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Scan: ", m_Evidence.Scan.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Charge: ", m_Evidence.Charge.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Mass: ", m_Evidence.MonoisotopicMass.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Observed Net: ", m_Evidence.ObservedNet.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Predicted Net: ", m_Evidence.PredictedNet.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Sequence: ", m_Evidence.Sequence)));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Clean Peptide: ", m_Evidence.CleanPeptide)));
        }
    }
}
