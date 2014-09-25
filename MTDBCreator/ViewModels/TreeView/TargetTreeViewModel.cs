using System.Collections.Generic;
using System.Windows.Documents;
using MTDBCreator.ViewModels.TreeView;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class TargetTreeViewModel : ObservableObject
    {
        private IEnumerable<TargetDatabaseTreeNodeViewModel> m_targetDatabaseTreeNodeViewModels;
        private IEnumerable<ProteinDatabaseTreeNodeViewModel> m_proteinDatabaseTreeNodeViewModels; 

        public TargetTreeViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_targetDatabaseTreeNodeViewModels = new List<TargetDatabaseTreeNodeViewModel> { new TargetDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
            m_proteinDatabaseTreeNodeViewModels = new List<ProteinDatabaseTreeNodeViewModel> { new ProteinDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
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

        public IEnumerable<ProteinDatabaseTreeNodeViewModel> ProteinDatabaseTreeNodeViewModels
        {
            get
            {
                return m_proteinDatabaseTreeNodeViewModels;
            }
            set
            {
                m_proteinDatabaseTreeNodeViewModels = value;
                OnPropertyChanged("ProteinDatabaseTreeNodeViewModels");
            }
        }
    }
}
