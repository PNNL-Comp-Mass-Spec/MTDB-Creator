using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using ACCESS = Microsoft.Office.Interop.Access;

namespace MTDBAccessIO
{
    public class AccessTargetDatabaseReader
    {

        public TargetDatabase Read(string path)
        {
            // Read in the data from the access database
            // put it into a text file (?)
            // Read the data from the text file into program
            var accApplication = new ACCESS.Application();

            var pathPieces = path.Split('\\');
            string directory = "";
            foreach (var piece in pathPieces)
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
            
            var consensusTargets    = new Dictionary<int, ConsensusTarget>();
            var proteins            = new Dictionary<int, ProteinInformation>();

            var ctReader    = new StreamReader(directory + "outTempAMT.txt");
            var protReader  = new StreamReader(directory + "outTempAMT_Proteins.txt");
            var mapReader   = new StreamReader(directory + "outTempAMT_to_Protein_Map.txt");

            // Read the headers for the files
            ctReader.ReadLine();
            protReader.ReadLine();
            mapReader.ReadLine();

            // Read the first "Data" lines from the files
            var ctLine      = ctReader.ReadLine();
            var protLine    = protReader.ReadLine();
            var mapLine     = mapReader.ReadLine();

            while (ctLine != null)
            {
                var pieces = ctLine.Split(',');

                var target = new ConsensusTarget
                {
                    Id = Convert.ToInt16(pieces[0]),
                    TheoreticalMonoIsotopicMass = Convert.ToDouble(pieces[1]),
                    AverageNet = Convert.ToDouble(pieces[2]),
                    PredictedNet = Convert.ToDouble(pieces[3]),
                    EncodedNumericSequence = pieces[6]
                };
                var totalEvidences = Convert.ToInt32(pieces[4]);
                var normScore = Convert.ToDouble(pieces[5]);
                for (var evNum = 0; evNum < totalEvidences; evNum++)
                {
                    var evidence = new Evidence
                    {
                        ObservedNet = target.AverageNet,
                        ObservedMonoisotopicMass = target.TheoreticalMonoIsotopicMass,
                        PredictedNet = target.PredictedNet,
                        NormalizedScore = normScore,
                        SeqWithNumericMods = target.EncodedNumericSequence,
                        Parent = target
                    };
                    target.Evidences.Add(evidence);
                }
                consensusTargets.Add(target.Id, target);
                ctLine = ctReader.ReadLine();
            }

            while (protLine != null)
            {
                var pieces = protLine.Split(',');

                var protein = new ProteinInformation
                {
                    ProteinName = pieces[1]
                };
                proteins.Add(Convert.ToInt32(pieces[0]), protein);
                protLine = protReader.ReadLine();
            }

            while (mapLine != null)
            {
                var pieces = mapLine.Split(',');

                consensusTargets[Convert.ToInt32(pieces[0])].AddProtein(proteins[Convert.ToInt32(pieces[1])]);
                mapLine = mapReader.ReadLine();
            }

            ctReader.Close();
            protReader.Close();
            mapReader.Close();

            File.Delete(directory + "outTempAMT.txt");
            File.Delete(directory + "outTempAMT_Proteins.txt");
            File.Delete(directory + "outTempAMT_to_Protein_Map.txt");

            var database = new TargetDatabase();
            foreach (var target in consensusTargets)
            {
                database.AddConsensusTarget(target.Value);
            }
            database.Proteins = proteins.Values.ToList();

            return database;
        }
    }
}
