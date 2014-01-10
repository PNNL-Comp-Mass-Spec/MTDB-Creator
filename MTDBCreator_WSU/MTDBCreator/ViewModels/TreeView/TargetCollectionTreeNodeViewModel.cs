using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetCollectionTreeNodeViewModel : TreeNodeViewModel
    {
        private IEnumerable<Target> m_Targets;

        public TargetCollectionTreeNodeViewModel(IEnumerable<Target> targets, TreeNodeViewModel parent)
            : base(String.Format("Targets ({0})", targets.Count().ToString()), true, parent)
        {
            m_Targets = targets;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            foreach (Target t in m_Targets)
            {
                m_ChildNodes.Add(new TargetTreeNodeViewModel(t, this));
            }
        }
    }
}
