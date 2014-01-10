using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetTreeNodeViewModel : TreeNodeViewModel
    {
        private Target m_Target;

        public TargetTreeNodeViewModel(Target t, TreeNodeViewModel parent)
            : base(t.Sequence, true, parent)
        {
            m_Target = t;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Scan: ", m_Target.Scan.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Charge: ", m_Target.Charge.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Mass: ", m_Target.MonoisotopicMass.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Observed Net: ", m_Target.ObservedNet.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Predicted Net: ", m_Target.PredictedNet.ToString())));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Sequence: ", m_Target.Sequence)));
            m_ChildNodes.Add(new TreeNodeViewModel(String.Concat("Clean Peptide: ", m_Target.CleanPeptide)));
        }
    }
}
