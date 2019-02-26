using System;
using System.Collections.ObjectModel;
using MTDBCreator.Helpers;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TreeNodeViewModel : ObservableObject, ITreeNodeViewModel
    {
        #region Private Fields

        private bool m_isSelected;
        private bool m_isExpanded;
        private bool m_isLoaded;

        private string m_text;

        private static string m_searchFilter = "";

        private readonly TreeNodeViewModel m_Parent;

        protected ObservableCollection<TreeNodeViewModel> m_ChildNodes;

        #endregion

        #region Events

        public event EventHandler ItemSelected;

        #endregion

        #region Properties

        public string Text {
            get => m_text;
            protected set
            {
                m_text = value;
                OnPropertyChanged("Text");
            }
        }

        public ObservableCollection<TreeNodeViewModel> ChildNodes => m_ChildNodes;

        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                if (value != m_isSelected)
                {
                    m_isSelected = value;
                    OnPropertyChanged("IsSelected");

                    if (value)
                    {
                        ItemSelected?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public bool IsExpanded
        {
            get => m_isExpanded;
            set
            {
                if (value != m_isExpanded)
                {
                    m_isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (m_isExpanded && m_Parent != null)
                {
                    m_Parent.IsExpanded = true;
                }

                if (m_isExpanded && !IsLoaded)
                {
                    LoadChildNodes();
                    m_isLoaded = true;
                }

                if (m_isExpanded)
                {
                    IsSelected = true;
                }
            }
        }

        public virtual bool IsLoaded => m_isLoaded;

        #endregion

        #region Methods

        public virtual void LoadChildNodes()
        {
            m_ChildNodes.Clear();
        }

        #endregion

        public TreeNodeViewModel(string text)
        {
            Text = text;
        }

        public TreeNodeViewModel(string text, TreeNodeViewModel parent)
            : this(text)
        {
            m_Parent = parent;
        }

        public TreeNodeViewModel(string text, bool hasChildNodes)
            : this(text)
        {
            if (hasChildNodes)
            {
                m_ChildNodes = new ObservableCollection<TreeNodeViewModel> { new TreeNodeViewModel("Loading...") };
            }
        }

        public TreeNodeViewModel(string text, bool hasChildNodes, TreeNodeViewModel parent)
            : this(text, hasChildNodes)
        {
            m_Parent = parent;
        }

        public string SearchFilter
        {
            get => m_searchFilter;
            set
            {
                m_searchFilter = value;
                OnPropertyChanged("SearchFilter");
            }
        }
    }
}
