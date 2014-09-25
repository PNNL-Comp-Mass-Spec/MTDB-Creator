using System;
using System.Linq;
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

            var targets = m_targetDatabase.ConsensusTargets.ToList();
            targets.Sort();

            foreach (var ct in targets)
            {
                m_ChildNodes.Add(new ConsensusTargetTreeNodeViewModel(ct, this));
            }
        }
    }
}
