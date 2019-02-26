using System;
using System.Linq;
using MTDBFrameworkBase.Database;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TargetDatabaseTreeNodeViewModel : TreeNodeViewModel
    {
        private readonly TargetDatabase m_targetDatabase;
        public DelegateCommand PeptideSearchCommand { get; set;}


        public TargetDatabaseTreeNodeViewModel(TargetDatabase database)
            : base(string.Format("Target Database ({0})", database.ConsensusTargets.Count), true)
        {
            m_targetDatabase = database;

            Action peptideSearch = PeptideSearch;
            PeptideSearchCommand = new DelegateCommand(peptideSearch);

            IsExpanded = true;
        }

        public void PeptideSearch()
        {
            base.LoadChildNodes();

            var targets = m_targetDatabase.ConsensusTargets.Where(x => x.EncodedNumericSequence.Contains(SearchFilter.ToUpper())).ToList();
            targets.Sort();
            Text = string.Format("Target Database ({0})", targets.Count);
            foreach (var ct in targets)
            {
                m_ChildNodes.Add(new ConsensusTargetTreeNodeViewModel(ct, this));
            }

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
