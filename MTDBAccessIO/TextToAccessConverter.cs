using System.IO;
using MTDBFramework.IO;
using ACCESS = Microsoft.Office.Interop.Access;

namespace MTDBAccessIO
{
    public class TextToAccessConverter : ITextToDbConverter
    {
        public bool ConvertToDbFormat(string outFilePath)
        {

            var accApplication = new ACCESS.Application();

            var outFile = new FileInfo(outFilePath);

            var directoryPath = outFile.DirectoryName;
            if (directoryPath == null)
            {
                throw new DirectoryNotFoundException("Unable to determine the parent directory of " + outFilePath);
            }

            if (outFile.Exists)
            {
                outFile.Delete();
            }

            accApplication.NewCurrentDatabase(outFile.FullName);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "T_Mass_Tags", FileName: Path.Combine(directoryPath, "tempMassTags.txt"), HasFieldNames: true);

            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "T_Mass_Tags_NET", FileName: Path.Combine(directoryPath, "tempMassTagsNet.txt"), HasFieldNames: true);

            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "T_Proteins", FileName: Path.Combine(directoryPath, "tempProteins.txt"), HasFieldNames: true);

            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "T_Mass_Tags_to_Protein_Map", FileName: Path.Combine(directoryPath, "tempMassTagToProteins.txt"), HasFieldNames: true);

            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "T_Analysis_Description", FileName: Path.Combine(directoryPath, "tempAnalysisDescription.txt"), HasFieldNames: true);

            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "V_Filter_Set_Overview_Ex", FileName: Path.Combine(directoryPath, "tempFilterSet.txt"), HasFieldNames: true);

            accApplication.CloseCurrentDatabase();
            accApplication.Quit();

            File.Delete(Path.Combine(directoryPath, "tempMassTags.txt"));
            File.Delete(Path.Combine(directoryPath, "tempPeptides.txt"));
            File.Delete(Path.Combine(directoryPath, "tempModInfo.txt"));
            File.Delete(Path.Combine(directoryPath, "tempMassTagsNet.txt"));
            File.Delete(Path.Combine(directoryPath, "tempProteins.txt"));
            File.Delete(Path.Combine(directoryPath, "tempMassTagToProteins.txt"));
            File.Delete(Path.Combine(directoryPath, "tempAnalysisDescription.txt"));
            File.Delete(Path.Combine(directoryPath, "tempFilterSet.txt"));

            return true;
        }
    }
}
