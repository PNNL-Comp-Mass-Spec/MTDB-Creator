using System.Collections.Generic;
using MTDBCreator.ViewModels.TreeView;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class TargetTreeViewModel : ObservableObject
    {
        private IEnumerable<TargetDatabaseTreeNodeViewModel> m_targetDatabaseTreeNodeViewModels;

        public TargetTreeViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_targetDatabaseTreeNodeViewModels = new List<TargetDatabaseTreeNodeViewModel> { new TargetDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
        }

        public IEnumerable<TargetDatabaseTreeNodeViewModel> TargetDatabaseTreeNodeViewModels
        {
            get
            {
                return m_targetDatabaseTreeNodeViewModels;
            }
            set
            {
                m_targetDatabaseTreeNodeViewModels = value;
                OnPropertyChanged("TargetDatabaseTreeNodeViewModels");
            }
        }
    }
}
