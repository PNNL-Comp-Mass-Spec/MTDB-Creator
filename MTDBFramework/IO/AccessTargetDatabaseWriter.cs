using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using System.Data.OleDb;
using System;
using System.IO;
using ADOX;
using ADODB;

namespace MTDBFramework.IO
{
	/// <summary>
	/// Create and output to a Microsoft Access format database
	/// </summary>
    public class AccessTargetDatabaseWriter : ITargetDatabaseWriter
    {
        private Connection CreateNewAccessDatabase(string path)
        {
            var cat = new ADOX.Catalog();
            var tables = AccessDatabaseTableCreator.CreateTables();

            ADODB.Connection con = null;

            try
            {
                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + "; Jet OLEDB:Engine Type=5");

                foreach (var member in tables)
                {
                    cat.Tables.Append(member);
                    con = cat.ActiveConnection as ADODB.Connection;
                    if (con != null)
                    {
                    }
                }
            }
            catch
            {
            }
            return con;
        }

        private readonly Dictionary<string, TargetPeptideInfo> m_uniquePeptides = new Dictionary<string, TargetPeptideInfo>();
        private readonly Dictionary<string, TargetDataSet> m_uniqueDataSets = new Dictionary<string, TargetDataSet>();
        private readonly Dictionary<string, ProteinInformation> m_uniqueProteins = new Dictionary<string, ProteinInformation>();

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

            ADODB.Connection con = CreateNewAccessDatabase(path);
            var record = new ADODB.Recordset();
            /* This section breaks up the Target object, pulling out the individual TargetDataSet,  SequenceInfo,
             * and TargetPeptideInfo. These objects are then "reverse linked", so that each of these objects 
             * relates to multiple evidences. This is because these objects need to know what they are related to.
             * Additionally, these objects are saved before the Evidences are, because these objects need to already
             * exist in order to properly generate the relation. 
             * */
            var current = 0;
            var currentProt = 0;
            var currentPair = 0;
            var currentEvidence = 0;
            var currentDatasetId = 0;
            var total = database.ConsensusTargets.Count;
            string insertString;

            foreach (var consensusTarget in database.ConsensusTargets)
            {
                OnProgressChanged(new MtdbProgressChangedEventArgs(current, total, MtdbCreationProgressType.COMMIT.ToString()));
                consensusTarget.Id = ++current;
                foreach (var evidence in consensusTarget.Evidences)
                {
                    if (!m_uniquePeptides.ContainsKey(evidence.PeptideInfo.Peptide))
                    {
                        m_uniquePeptides.Add(evidence.PeptideInfo.Peptide, evidence.PeptideInfo);
                    }
                    evidence.PeptideInfo = m_uniquePeptides[evidence.PeptideInfo.Peptide];
                    if (!m_uniqueDataSets.ContainsKey(evidence.DataSet.Path))
                    {
                        evidence.DataSet.Id = ++currentDatasetId;
                        m_uniqueDataSets.Add(evidence.DataSet.Path, evidence.DataSet);
                    }
                    evidence.DataSet = m_uniqueDataSets[evidence.DataSet.Path];
                    evidence.Parent = consensusTarget;
                    evidence.Id = ++currentEvidence;
                    con.BeginTrans();
                    insertString = string.Format("Insert into Evidence(EvidenceId,Charge,ObservedNet,PredictedNet,Mz,Scan,DelM,DelMPpm,SpecProb,DatasetId,ConsensusId) values({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                        evidence.Id,
                        evidence.Charge,
                        evidence.ObservedNet,
                        evidence.PredictedNet,
                        evidence.Mz,
                        evidence.Scan,
                        evidence.DelM,
                        evidence.DelMPpm,
                        evidence.SpecProb,
                        evidence.DataSet.Id,
                        evidence.Parent.Id
                    );
                    record.Open(insertString, con);
                    con.CommitTrans();

                }
                foreach (var protein in consensusTarget.Proteins)
                {
                    if (!m_uniqueProteins.ContainsKey(protein.ProteinName))
                    {
                        protein.Id = ++currentProt;
                        m_uniqueProteins.Add(protein.ProteinName, protein);
                    }
                    protein.Id = m_uniqueProteins[protein.ProteinName].Id;
                    var cProt = m_uniqueProteins[protein.ProteinName];
                    currentPair++;
                    ConsensusProteinPair cPPair = new ConsensusProteinPair();
                    cPPair.Id = currentPair;
                    cPPair.Consensus = consensusTarget;
                    cPPair.Protein = cProt;
                    cPPair.CleavageState = (short)cProt.CleavageState;
                    cPPair.TerminusState = (short)cProt.TerminusState;
                    cPPair.ResidueStart = (short)cProt.ResidueStart;
                    cPPair.ResidueEnd = (short)cProt.ResidueEnd;
                    protein.ConsensusProtein.Add(cPPair);
                    consensusTarget.ConsensusProtein.Add(cPPair);
                    con.BeginTrans();
                    insertString = string.Format("Insert into ConsensusProteinPair(PairId,ConsensusId,ProteinId,CleavageState,TerminusState,ResidueStart,ResidueEnd) values({0},{1},{2},{3},{4},{5},{6})",
                        cPPair.Id,
                        cPPair.Consensus.Id,
                        cPPair.Protein.Id,
                        cPPair.CleavageState,
                        cPPair.TerminusState,
                        cPPair.ResidueStart,
                        cPPair.ResidueEnd);
                    record.Open(insertString, con);
                    con.CommitTrans();
                }

                consensusTarget.Dataset = consensusTarget.Evidences[0].DataSet;
                consensusTarget.EncodedNumericSequence = consensusTarget.Evidences[0].SeqWithNumericMods;
                consensusTarget.ModificationCount = consensusTarget.Evidences[0].ModificationCount;
                consensusTarget.ModificationDescription = consensusTarget.Evidences[0].ModificationDescription;
                consensusTarget.MultiProteinCount = consensusTarget.Evidences[0].MultiProteinCount;

                con.BeginTrans();
                insertString = string.Format("Insert into ConsensusTargets(ConsensusId,Net,StdevNet,PredictedNet,TheoreticalMonoIsotopicMass,PrefixResidue,SuffixResidue,Sequence,ModificationCount,ModificationDescription,MultiProteinCount) values({0}, {1}, {2}, {3}, {4}, '{5}', '{6}', '{7}', {8}, '{9}', {10})",
                    consensusTarget.Id,
                    consensusTarget.AverageNet,
                    consensusTarget.StdevNet,
                    consensusTarget.PredictedNet,
                    consensusTarget.TheoreticalMonoIsotopicMass,
                    consensusTarget.PrefixResidue,
                    consensusTarget.SuffixResidue,
                    consensusTarget.EncodedNumericSequence,
                    consensusTarget.ModificationCount,
                    consensusTarget.ModificationDescription,
                    consensusTarget.MultiProteinCount
                );
                record.Open(insertString, con);
                con.CommitTrans();

            }

            foreach (var protein in m_uniqueProteins)
            {
                con.BeginTrans();
                insertString = string.Format("Insert into ProteinInformation(ProteinId,ProteinName) values({0}, '{1}')",
                    protein.Value.Id,
                    protein.Value.ProteinName
                );
                record.Open(insertString, con);
                con.CommitTrans();
            }

            foreach (var dataset in m_uniqueDataSets)
            {
                con.BeginTrans();
                insertString = string.Format("Insert into TargetDataSet(DatasetId,Name,SearchTool) values({0}, '{1}', '{2}')",
                    dataset.Value.Id,
                    dataset.Value.Name,
                    dataset.Value.Tool
                );
                record.Open(insertString, con);
                con.CommitTrans();
            }

            con.BeginTrans();

            string exportTryptic = "";
            string exportPartially = "";
            string exportNon = "";
            bool first = true;
            foreach (var piece in options.MinXCorrForExportTryptic)
            {
                if (!first)
                    exportTryptic += ", ";
                exportTryptic += piece.ToString();
                first = false;
            }
            first = true;
            foreach (var piece in options.MinXCorrForExportPartiallyTryptic)
            {
                if (!first)
                    exportPartially += ", ";
                exportPartially += piece.ToString();
                first = false;
            }
            first = true;
            foreach (var piece in options.MinXCorrForExportNonTryptic)
            {
                if (!first)
                    exportNon += ", ";
                exportNon += piece.ToString();
                first = false;
            }
            insertString = string.Format("Insert into Options(OptionsId,RegressionType,RegressionOrder,TargetFilterType,PredictorType,MaxModsForAlignment,MinObservationsForExport,ExportTryptic,ExportPartiallyTryptic,ExportNonTryptic,MinXCorrForExportTryptic,MinXCorrForExportPartiallyTryptic,MinXCorrForExportNonTryptic,MinXCorrForAlignment,UseDelCn,MaxDelCn,MaxLogEValForXTandemAlignment,MaxLogEvalForXTandemExport,MaxRankForExport) values(1, '{0}', {1}, '{2}', '{3}', {4}, {5}, '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}')",
                options.RegressionType,
                options.RegressionOrder,
                options.TargetFilterType,
                options.PredictorType,
                options.MaxModsForAlignment,
                options.MinObservationsForExport,
                options.ExportTryptic,
                options.ExportPartiallyTryptic,
                options.ExportNonTryptic,
                exportTryptic,
                exportPartially,
                exportNon,
                options.MinXCorrForAlignment,
                options.UseDelCn,
                options.MaxDelCn,
                options.MaxLogEValForXTandemAlignment,
                options.MaxLogEValForXTandemExport,
                options.MaxRankForExport
                );
            record.Open(insertString, con);
            con.CommitTrans();

            OnProgressChanged(new MtdbProgressChangedEventArgs(current, total, MtdbCreationProgressType.COMMIT.ToString()));
            con.Close();
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
