using System;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly Evidence m_evidence;

        public TargetTreeNodeViewModel(Evidence t, TreeNodeViewModel parent)
            : base(t.Sequence, true, parent)
        {
            m_evidence = t;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Scan: ", m_evidence.Scan.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Charge: ", m_evidence.Charge.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Mass: ", m_evidence.MonoisotopicMass.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Observed Net: ", m_evidence.ObservedNet.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Originating Dataset: ", m_evidence.DataSet.Name)));
        }
    }
}
