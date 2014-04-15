using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetCollectionTreeNodeViewModel : TreeNodeViewModel
    {
        private IEnumerable<Evidence> m_Evidences;

        public TargetCollectionTreeNodeViewModel(IEnumerable<Evidence> evidences, TreeNodeViewModel parent)
            : base(String.Format("Evidence ({0})", evidences.Count().ToString()), true, parent)
        {
            m_Evidences = evidences;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            foreach (Evidence t in m_Evidences)
            {
                m_ChildNodes.Add(new TargetTreeNodeViewModel(t, this));
            }
        }
    }
}
