using System;
using MTDBFramework.Database;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetDatabaseTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly TargetDatabase m_targetDatabase;

        public TargetDatabaseTreeNodeViewModel(TargetDatabase database)
            : base(String.Format("Target Database ({0})", database.ConsensusTargets.Count), true)
        {
            m_targetDatabase = database;

            IsExpanded = true;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            foreach (var ct in m_targetDatabase.ConsensusTargets)
            {
                m_ChildNodes.Add(new ConsensusTargetTreeNodeViewModel(ct, this));
            }
        }
    }
}
