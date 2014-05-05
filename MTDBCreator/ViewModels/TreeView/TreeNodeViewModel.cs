using System;
using System.Collections.ObjectModel;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels.TreeView
{
    public class TreeNodeViewModel : ObservableObject, ITreeNodeViewModel
    {
        #region Private Fields

        private bool m_IsSelected;
        private bool m_IsExpanded;
        private bool m_IsLoaded;

        protected TreeNodeViewModel m_Parent;

        protected ObservableCollection<TreeNodeViewModel> m_ChildNodes;

        #endregion

        #region Events

        public event EventHandler ItemSelected;

        #endregion

        #region Properties

        public virtual string Text { get; protected set; }

        public ObservableCollection<TreeNodeViewModel> ChildNodes
        {
            get
            {
                return m_ChildNodes;
            }
        }

        public virtual bool IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                if (value != m_IsSelected)
                {
                    m_IsSelected = value;
                    OnPropertyChanged("IsSelected");

                    if (value)
                    {
                        if (ItemSelected != null)
                        {
                            ItemSelected(this, new EventArgs());
                        }
                    }
                }
            }
        }

        public virtual bool IsExpanded
        {
            get { return m_IsExpanded; }
            set
            {
                if (value != m_IsExpanded)
                {
                    m_IsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (m_IsExpanded && m_Parent != null)
                {
                    m_Parent.IsExpanded = true;
                }

                if (m_IsExpanded && !IsLoaded)
                {
                    LoadChildNodes();
                    m_IsLoaded = true;
                }

                if (m_IsExpanded)
                {
                    IsSelected = true;
                }
            }
        }

        public virtual bool IsLoaded
        {
            get { return m_IsLoaded; }
        }

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
    }
}
