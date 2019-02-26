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
    public class AccessTargetDatabaseReader: ITargetDatabaseReader
    {

        public IEnumerable<LcmsDataSet> Read(string sourceFilePath)
        {
            // Read in the data from the access database
            // put it into a text file (?)
            // Read the data from the text file into program
            var accApplication = new ACCESS.Application();

            var outFile = new FileInfo(sourceFilePath);

            var directoryPath = outFile.DirectoryName;
            if (directoryPath == null)
            {
                throw new DirectoryNotFoundException("Unable to determine the parent directory of " + sourceFilePath);
            }

            accApplication.OpenCurrentDatabase(sourceFilePath);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT", FileName: Path.Combine(directoryPath, "outTempAMT.txt"), HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_Proteins", FileName: Path.Combine(directoryPath, "outTempAMT_Proteins.txt"), HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_to_Protein_Map", FileName: Path.Combine(directoryPath, "outTempAMT_to_Protein_Map.txt"), HasFieldNames: true);
            accApplication.CloseCurrentDatabase();
            accApplication.Quit();

            var priorDatasets = new List<LcmsDataSet>();

            return priorDatasets;
        }

        public TargetDatabase ReadDb(string dbFilePath)
        {
            // Read in the data from the access database
            // put it into a text file (?)
            // Read the data from the text file into program
            var accApplication = new ACCESS.Application();

            var dbFile = new FileInfo(dbFilePath);
            var directoryPath = dbFile.DirectoryName;
            if (directoryPath == null)
            {
                throw new DirectoryNotFoundException("Unable to determine the parent directory of " + dbFilePath);
            }

            accApplication.OpenCurrentDatabase(dbFile.FullName);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT", FileName: Path.Combine(directoryPath, "outTempAMT.txt"), HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_Proteins", FileName: Path.Combine(directoryPath, "outTempAMT_Proteins.txt"), HasFieldNames: true);
            accApplication.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acExportDelim,
                TableName: "AMT_to_Protein_Map", FileName: Path.Combine(directoryPath, "outTempAMT_to_Protein_Map.txt"), HasFieldNames: true);
            accApplication.CloseCurrentDatabase();
            accApplication.Quit();

            // Put the data into its objects
            // AMT stuff going in Consensus targets
            // NET, MonoMass, Predicted NET, Peptide (Sequence with numeric mods), ID (can be crushed later)
            // OBSERVED <-- number of times this peptide was seen in the AMT
            // for <observed> times, add an evidence with the info? would make sense and would allow the stats calculations to be accurate
            // Protein stuff going into ProteinInfo
            // Protein name only thing important for MTDB, ID (can be crushed later)
            // AMT map
            // Link Consensus and Protein (ct[ct_id].protein.add(protein[protein_id]))

            var consensusTargets    = new Dictionary<int, ConsensusTarget>();
            var proteins            = new Dictionary<int, ProteinInformation>();

            var ctReader       = new StreamReader(Path.Combine(directoryPath, "outTempAMT.txt"));
            var proteinReader  = new StreamReader(Path.Combine(directoryPath, "outTempAMT_Proteins.txt"));
            var mapReader      = new StreamReader(Path.Combine(directoryPath, "outTempAMT_to_Protein_Map.txt"));

            // Read the headers for the files
            ctReader.ReadLine();
            proteinReader.ReadLine();
            mapReader.ReadLine();

            // Read the first "Data" lines from the files
            var ctLine      = ctReader.ReadLine();
            var proteinLine = proteinReader.ReadLine();
            var mapLine     = mapReader.ReadLine();

            while (ctLine != null)
            {
                var pieces = ctLine.Split(',');

                var target = new ConsensusTarget
                {
                    Id = Convert.ToInt32(pieces[0]),
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

            while (proteinLine != null)
            {
                var pieces = proteinLine.Split(',');

                var protein = new ProteinInformation
                {
                    ProteinName = pieces[1]
                };
                proteins.Add(Convert.ToInt32(pieces[0]), protein);
                proteinLine = proteinReader.ReadLine();
            }

            while (mapLine != null)
            {
                var pieces = mapLine.Split(',');

                consensusTargets[Convert.ToInt32(pieces[0])].AddProtein(proteins[Convert.ToInt32(pieces[1])]);
                mapLine = mapReader.ReadLine();
            }

            ctReader.Close();
            proteinReader.Close();
            mapReader.Close();

            File.Delete(Path.Combine(directoryPath, "outTempAMT.txt"));
            File.Delete(Path.Combine(directoryPath, "outTempAMT_Proteins.txt"));
            File.Delete(Path.Combine(directoryPath, "outTempAMT_to_Protein_Map.txt"));

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
