using System.Collections.Generic;
using System.Linq;
using MTDBCreator.ViewModels.TreeView;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class TargetTreeViewModel : ObservableObject
    {
        private IEnumerable<TargetDatabaseTreeNodeViewModel> m_targetDatabaseTreeNodeViewModels;
        private IEnumerable<ProteinDatabaseTreeNodeViewModel> m_proteinDatabaseTreeNodeViewModels;
        private string m_searchFilter;
        private int m_selectedTabIndex;

        public TargetTreeViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            m_targetDatabaseTreeNodeViewModels = new List<TargetDatabaseTreeNodeViewModel> { new TargetDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
            m_proteinDatabaseTreeNodeViewModels = new List<ProteinDatabaseTreeNodeViewModel> { new ProteinDatabaseTreeNodeViewModel(analysisJobViewModel.Database) };
            m_searchFilter = "";
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

        public void EnterHandler()
        {
            if (SelectedTabIndex == 0)
            {
                TargetDatabaseTreeNodeViewModels.First().PeptideSearch();
            }
            else
            {
                ProteinDatabaseTreeNodeViewModels.First().ProteinSearch();
            }
            
        }

        public string SearchFilter
        {
            get
            {
                return m_searchFilter;
            }
            set
            {
                m_searchFilter = value;
                foreach (var model in m_proteinDatabaseTreeNodeViewModels)
                {
                    model.SearchFilter = value;
                }
                foreach (var model in m_targetDatabaseTreeNodeViewModels)
                {
                    model.SearchFilter = value;
                }
                OnPropertyChanged("SearchFilter");
            }
        }

        public int SelectedTabIndex
        {
            get
            {
                return m_selectedTabIndex;
            }
            set
            {
                m_selectedTabIndex = value;
                OnPropertyChanged("SelectedTab");
            }
        }
    }
}
