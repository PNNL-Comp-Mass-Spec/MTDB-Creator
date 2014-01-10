namespace MTDBCreator.ViewModels.TreeView
{
    public interface ITreeNodeViewModel
    {
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        bool IsLoaded { get; }

        void LoadChildNodes();
    }
}