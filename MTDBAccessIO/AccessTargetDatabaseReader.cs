using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using ACCESS = Microsoft.Office.Interop.Access;

namespace MTDBAccessIO
{
    class AccessTargetDatabaseReader : ITargetDatabaseReader
    {
        public IEnumerable<MTDBFramework.Data.LcmsDataSet> Read(string path)
        {
            // Read in the data from the access database
            // put it into a text file (?)
            // Read the data from the text file into program
            var accApplication = new ACCESS.Application();

            var pieces = path.Split('\\');
            string directory = "";
            foreach (var piece in pieces)
            {
                if (piece.Contains("."))
                {
                    continue;
                }
                directory += piece;
                directory += "\\";
            }

            accApplication.OpenCurrentDatabase(path);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT", FileName: directory + "outTempAMT.txt", HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_Proteins", FileName: directory + "outTempAMT_Proteins.txt", HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_to_Protein_Map", FileName: directory + "outTempAMT_to_Protein_Map.txt", HasFieldNames: true);
            accApplication.CloseCurrentDatabase();
            accApplication.Quit();

            // Put the data into its objects
            // AMT stuff going in Consensus targets
            // NET, MonoMass, Pred. Net, Peptide (Sequence with numeric mods), ID (can be crushed later)
            // OBSERVED <-- number of times this peptide was seen in the AMT
            // for <observed> times, add an evidence with the info? would make sense and would allow the stats calcs to be accurate
            // Prot stuff going into ProteinInfo
            // Prot name only thing important for MTDB, ID (can be crushed later)
            // AMT map
            // Link Consensus and Protein (ct[ct_id].protein.add(protein[prot_id]))

            //TODO: Finish implementation here after iPRG
            
            var consensusTargets    = new List<ConsensusTarget>();
            var proteins            = new List<ProteinInformation>();

            var ctReader    = new StreamReader(directory + "outTempAMT.txt");
            var protReader  = new StreamReader(directory + "outTempAMT_Proteins.txt");
            var mapReader   = new StreamReader(directory + "outTempAMT_to_Protein_Map.txt");

            //var

            while (ctReader.ReadLine() != null)
            {
                var line = ctReader.ReadLine();
                //var pieces
            }

            
            return new List<LcmsDataSet>();
        }
    }
}
