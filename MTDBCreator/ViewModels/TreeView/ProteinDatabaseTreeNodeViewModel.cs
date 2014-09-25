using System;
using System.Linq;
using MTDBFramework.Database;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ProteinDatabaseTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly TargetDatabase m_targetDatabase;

        public ProteinDatabaseTreeNodeViewModel(TargetDatabase database)
            : base(String.Format("Target Database ({0})", database.Proteins.Count), true)
        {
            m_targetDatabase = database;

            IsExpanded = true;
        }

        public override void LoadChildNodes()
        {
            base.LoadChildNodes();

            var proteins = m_targetDatabase.Proteins.ToList();
            proteins.Sort();

            foreach (var prot in proteins)
            {
                m_ChildNodes.Add(new ProteinTreeNodeViewModel(prot, this));
            }
        }
    }
}
