using MTDBFrameworkBase.Data;

namespace MTDBCreator.Helpers.Dialog
{
    public class FileDialogFormatInfo
    {
        public string Title { get; }
        public string Filter { get; }
        public LcmsIdentificationTool Format { get; set; }

        public FileDialogFormatInfo(string title, string filter, LcmsIdentificationTool format)
        {
            Title = title;
            Filter = filter;
            Format = format;
        }
    }
}
