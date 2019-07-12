using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

namespace MTDBCreator.Helpers.Dialog
{
    public class FileDialogFormatInfoFactory
    {
        public static FileDialogFormatInfo Create(string format)
        {
            switch (format)
            {
                case "Sequest":
                    return new FileDialogFormatInfo("Open SEQUEST analysis files", "SEQUEST Analysis Files |*_syn.txt", LcmsIdentificationTool.Sequest);
                case "XTandem":
                    return new FileDialogFormatInfo("Open X!Tandem analysis files", "X!Tandem Analysis Files |*_xt.txt", LcmsIdentificationTool.XTandem);
                case "MSGFPlus":
                    return new FileDialogFormatInfo("Open MSGF+ analysis files", "MSGF+ Analysis Files |*msgfplus_syn.txt;*msgfdb_syn.txt", LcmsIdentificationTool.MsgfPlus);
                case "MZIdentML":
                    return new FileDialogFormatInfo("Open MZIdent analysis files", "MZIdentML Analysis Files |*.mzid;*.mzid.gz", LcmsIdentificationTool.MZIdentML);
                case "MSAlign":
                    return new FileDialogFormatInfo("Open MSAlign analysis files", "MSAlign Analysis Files |*msalign_syn.txt", LcmsIdentificationTool.MSAlign);
                case "Analysis Files":
                    return new FileDialogFormatInfo("Open old analysis file type", "Analysis Files |*_syn.txt;*msalign_syn.txt;*msgfplus_syn.txt;*msgfdb_syn.txt;*_xt.txt", LcmsIdentificationTool.Sequest);
                case "Description":
                    return new FileDialogFormatInfo("Open a dataset description file", "Dataset description files |*.txt |All Files |*.*", LcmsIdentificationTool.Description);
                default:
                    return null;
            }
        }
    }
}
