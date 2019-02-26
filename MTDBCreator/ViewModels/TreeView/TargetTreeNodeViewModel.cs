using System;
using System.Globalization;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

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

            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Scan: ", m_evidence.Scan.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Charge: ", m_evidence.Charge.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Mass: ", m_evidence.MonoisotopicMass.ToString(CultureInfo.InvariantCulture))));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Observed Net: ", m_evidence.ObservedNet.ToString(CultureInfo.InvariantCulture))));
            m_ChildNodes.Add(new TreeNodeViewModel(string.Concat("Originating Dataset: ", m_evidence.DataSet.Name)));
        }
    }
}
