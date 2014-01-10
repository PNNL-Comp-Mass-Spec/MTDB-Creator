using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.Title = title;
            this.Filter = filter;
            this.Format = format;
        }
    }
}
