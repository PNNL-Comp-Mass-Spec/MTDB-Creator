using System;
using MTDBFramework.Data;
using MTDBFramework.Database;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetDatabaseTreeNodeViewModel : TreeNodeViewModel
    {
        private TargetDatabase m_TargetDatabase;

        public TargetDatabaseTreeNodeViewModel(TargetDatabase database)
            : base(String.Format("Target Database ({0})", database.ConsensusTargets.Count), true)
        {
            m_TargetDatabase = database;

            IsExpanded = true;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            foreach (var ct in m_TargetDatabase.ConsensusTargets)
            {
                m_ChildNodes.Add(new ConsensusTargetTreeNodeViewModel(ct, this));
            }
        }
    }
}
