using System.Collections.Generic;
using System.IO;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using MTDBFramework.UI;
using ACCESS = Microsoft.Office.Interop.Access;

namespace MTDBAccessIO
{
	/// <summary>
	/// Create and output to a Microsoft Access format database
	/// </summary>
    public class AccessTargetDatabaseWriter : ITargetDatabaseWriter
    {

	    private void ExportToText(string path, TargetDatabase inputData)
	    {
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

	        var currentProt = 0;

	        var targetWriter    = new StreamWriter(directory + "tempAMT.txt");
            var proteinWriter   = new StreamWriter(directory + "tempAMT_Proteins.txt");
            var mapWriter       = new StreamWriter(directory + "tempAMT_to_Protein_Map.txt");

            var targetHeader = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                "AMT_ID",
                "AMTMonoisotopicMass",
                "NET",
                "PNET",
                "MSMS_Obs_Count",
                "High_Normalized_Score",
                "Peptide"
                );
            targetWriter.WriteLine(targetHeader);

            var proteinHeader = string.Format("{0},{1},{2}",
                                            "Protein_ID",
                                            "Protein_Name",
                                            "Protein_Description"
                                            );
            proteinWriter.WriteLine(proteinHeader);
            var mapHeader = string.Format("{0},{1}",
                                            "AMT_ID",
                                            "Protein_ID"
                                            );
            mapWriter.WriteLine(mapHeader);

            foreach (var target in inputData.ConsensusTargets)
            {
                var msmsObsCount = target.Evidences.Count;
                var highestNormalized = target.Evidences.Max(x => x.NormalizedScore);
                var seqPieces = target.CleanSequence.Split('.');

                var test = target.EncodedNumericSequence.Split('.');
                var numSeq = "";
                if (test.Count() != 1)
                {
                    bool first = true;
                    for (var i = 1; i < test.Count() - 1; i++)
                    {
                        if (!first)
                        {
                            numSeq += ".";
                        }
                        numSeq += test[i];
                        first = false;
                    }
                }
                else
                {
                    numSeq = test[0];
                }


                var cleanPeptide = (seqPieces.ToList().Count == 1) ? seqPieces[0] : seqPieces[1];
                var targetLine = string.Format("{0},{1},{2},{3},{4},{5},\"{6}\"",
                    target.Id,
                    target.TheoreticalMonoIsotopicMass,
                    target.AverageNet,
                    target.PredictedNet,
                    msmsObsCount,
                    highestNormalized,
                    numSeq
                    );
                targetWriter.WriteLine(targetLine);

                m_amtToProteinMap.Add(target.Id, new List<int>());

                foreach (var protein in target.Proteins)
                {
                    if (!m_uniqueProteins.ContainsKey(protein.ProteinName))
                    {
                        protein.Id = ++currentProt;
                        m_uniqueProteins.Add(protein.ProteinName, protein);
                        var proteinLine =
                            string.Format("{0},{1},{2}",
                                protein.Id,
                                protein.ProteinName,
                                ""
                                );
                        proteinWriter.WriteLine(proteinLine);
                    }
                    protein.Id = m_uniqueProteins[protein.ProteinName].Id;
                    m_amtToProteinMap[target.Id].Add(protein.Id);

                    var mapLine = string.Format("{0},{1}",
                        target.Id,
                        protein.Id
                        );
                    mapWriter.WriteLine(mapLine);
                }
            }
            targetWriter.Close();
            proteinWriter.Close();
            mapWriter.Close();
        }

        private readonly Dictionary<string, TargetPeptideInfo> m_uniquePeptides = new Dictionary<string, TargetPeptideInfo>();
        private readonly Dictionary<string, TargetDataSet> m_uniqueDataSets = new Dictionary<string, TargetDataSet>();
        private readonly Dictionary<string, ProteinInformation> m_uniqueProteins = new Dictionary<string, ProteinInformation>();

        //Might not need this
        private readonly Dictionary<int, List<int>> m_amtToProteinMap = new Dictionary<int, List<int>>(); 

		/// <summary>
		/// Write the data to the database
		/// </summary>
		/// <param name="database"></param>
		/// <param name="options"></param>
		/// <param name="path"></param>
        public void Write(TargetDatabase database, Options options, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

		    ExportToText(path, database);

		    TextToAccessConvert(path);
        }

        private void TextToAccessConvert(string path)
        {
            var accApplicaiton = new ACCESS.Application();

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

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            accApplicaiton.NewCurrentDatabase(path);
            accApplicaiton.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "AMT", FileName: directory + "tempAMT.txt", HasFieldNames: true);
            accApplicaiton.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "AMT_Proteins", FileName: directory + "tempAMT_Proteins.txt", HasFieldNames: true);
            accApplicaiton.DoCmd.TransferText(TransferType: ACCESS.AcTextTransferType.acImportDelim,
                TableName: "AMT_to_Protein_Map", FileName: directory + "tempAMT_to_Protein_Map.txt", HasFieldNames: true);
            accApplicaiton.CloseCurrentDatabase();
            accApplicaiton.Quit();
            File.Delete(directory + "tempAMT.txt");
            File.Delete(directory + "tempAMT_Proteins.txt");
            File.Delete(directory + "tempAMT_to_Protein_Map.txt");
        }



        #region Events

		/// <summary>
		/// Progress update reporting
		/// </summary>
        public event MtdbProgressChangedEventHandler ProgressChanged;

		/// <summary>
		/// Event handler
		/// </summary>
		/// <param name="e"></param>
        protected void OnProgressChanged(MtdbProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        #endregion
    }
}
