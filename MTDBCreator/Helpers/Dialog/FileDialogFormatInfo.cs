using MTDBFramework.Data;

namespace MTDBCreator.Helpers.Dialog
{
    public class FileDialogFormatInfo
    {
        public string Title { get; private set; }
        public string Filter { get; private set; }
        public LcmsIdentificationTool Format { get; private set; }

        public FileDialogFormatInfo(string title, string filter, LcmsIdentificationTool format)
        {
            Title = title;
            Filter = filter;
            Format = format;
        }
    }
}
