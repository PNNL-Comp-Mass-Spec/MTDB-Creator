using System.Collections.Generic;
using MTDBCreator.ViewModels.TreeView;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class TargetTreeViewModel : ObservableObject
    {
        private IEnumerable<TargetDatabaseTreeNodeViewModel> m_TargetDatabaseTreeNodeViewModels;

        public TargetTreeViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_TargetDatabaseTreeNodeViewModels = new List<TargetDatabaseTreeNodeViewModel> { new TargetDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
        }

        public IEnumerable<TargetDatabaseTreeNodeViewModel> TargetDatabaseTreeNodeViewModels
        {
            get
            {
                return m_TargetDatabaseTreeNodeViewModels;
            }
            set
            {
                m_TargetDatabaseTreeNodeViewModels = value;
                OnPropertyChanged("TargetDatabaseTreeNodeViewModels");
            }
        }
    }
}
