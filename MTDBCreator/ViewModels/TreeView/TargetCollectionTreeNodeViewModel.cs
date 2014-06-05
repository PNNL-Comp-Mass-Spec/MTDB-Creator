using System;
using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetCollectionTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly IEnumerable<Evidence> m_evidences;

        public TargetCollectionTreeNodeViewModel(IEnumerable<Evidence> evidences, TreeNodeViewModel parent)
            : base(String.Format("Evidence ({0})", evidences.Count()), true, parent)
        {
            m_evidences = evidences;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            foreach (var t in m_evidences)
            {
                m_ChildNodes.Add(new TargetTreeNodeViewModel(t, this));
            }
        }
    }
}
