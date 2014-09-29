using System;
using System.Linq;
using MTDBFramework.Database;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ProteinDatabaseTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly TargetDatabase m_targetDatabase;
        public DelegateCommand ProteinSearchCommand { get; set; }

        public ProteinDatabaseTreeNodeViewModel(TargetDatabase database)
            : base(String.Format("Target Database ({0})", database.Proteins.Count), true)
        {
            m_targetDatabase = database;

            Action proteinSearch = ProteinSearch;
            ProteinSearchCommand = new DelegateCommand(proteinSearch);

            IsExpanded = true;
        }

        public void ProteinSearch()
        {
            base.LoadChildNodes();

            var proteins = m_targetDatabase.Proteins.Where(x => x.ProteinName.ToUpper().Contains(SearchFilter.ToUpper())).ToList();
            proteins.Sort();
            Text = string.Format("Target Database ({0})", proteins.Count);

            foreach (var prot in proteins)
            {
                m_ChildNodes.Add(new ProteinTreeNodeViewModel(prot, this));
            }

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
