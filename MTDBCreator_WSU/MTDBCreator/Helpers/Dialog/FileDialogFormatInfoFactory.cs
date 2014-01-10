using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBCreator.Helpers.Dialog
{
    public class FileDialogFormatInfoFactory
    {
        public static FileDialogFormatInfo Create(string format)
        {
            switch (format)
            {
                case "Sequest":
                    return new FileDialogFormatInfo("Open SEQUEST analysis files", "SEQUEST Analysis Files (*_syn.txt)|*_syn.txt", LcmsIdentificationTool.Sequest);
                case "XTandem":
                    return new FileDialogFormatInfo("Open X!Tandem analysis files", "X!Tandem Analysis Files (*_xt.txt)|*_xt.txt", LcmsIdentificationTool.XTandem);
                case "MSAlign":
                    return new FileDialogFormatInfo("Open MSAlign analysis files", "MSAlign Analysis Files (*.txt)|*.txt", LcmsIdentificationTool.MSAlign);
                case "Description":
                    return new FileDialogFormatInfo("Open a dataset description file", "Dataset description files (*.txt)|*.txt|All Files (*.*)|*.*", LcmsIdentificationTool.Description);
                default:
                    return null;
            }
        }
    }
}
