using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBFramework.IO
{
	/// <summary>
	/// Writing to a SQLite MTDB
	/// </summary>
    public class SqLiteTargetDatabaseWriter : ITargetDatabaseWriter
    {

        // TODO: Implement these (or maybe dictionaries)
        private readonly Dictionary<string, TargetPeptideInfo> m_uniquePeptides = new Dictionary<string, TargetPeptideInfo>();
        private readonly Dictionary<string, TargetDataSet> m_uniqueDataSets = new Dictionary<string, TargetDataSet>();
        private readonly Dictionary<string, ProteinInformation> m_uniqueProteins = new Dictionary<string, ProteinInformation>();
        private readonly Dictionary<string, PostTranslationalModification> m_uniquePtms = new Dictionary<string, PostTranslationalModification>();

		/// <summary>
		/// Write to the SQLite database
		/// </summary>
		/// <param name="database"></param>
		/// <param name="options"></param>
		/// <param name="path"></param>
        public void Write(TargetDatabase database, Options options, string path)
        {
            DatabaseFactory.DatabaseFile = path;
			/**********************************************************************************************
			 * TODO: Get the append capability working
			 * Set to false to avoid problems. Setting it to true will append some items, but not others. 
			 **********************************************************************************************/
	        DatabaseFactory.ReadOrAppend = false;
            var sessionFactory = DatabaseFactory.CreateSessionFactory(options.DatabaseType);

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {

                    /* This section breaks up the Target object, pulling out the individual TargetDataSet,  SequenceInfo,
                     * and TargetPeptideInfo. These objects are then "reverse linked", so that each of these objects 
                     * relates to multiple evidences. This is because these objects need to know what they are related to.
                     * Additionally, these objects are saved before the Evidences are, because these objects need to already
                     * exist in order to properly generate the relation. 
                     * */
                    var current = 0;
                    var currentProt = 0;
                    var currentPtm = 0;
                    var total = database.ConsensusTargets.Count;
                    session.Save(options);
                    foreach (var consensusTarget in database.ConsensusTargets)
                    {
                        OnProgressChanged(new MtdbProgressChangedEventArgs(current, total, MtdbCreationProgressType.COMMIT.ToString()));
                        consensusTarget.Id = ++current;
                        foreach(var ptm in consensusTarget.PTMs)
                        {
                            ptm.Id = 0;
                        }
                        foreach (var evidence in consensusTarget.Evidences)
                        {
                            //if (!m_uniquePeptides.ContainsKey(evidence.PeptideInfo.Peptide))
                            //{
                            //    m_uniquePeptides.Add(evidence.PeptideInfo.Peptide, evidence.PeptideInfo);
                            //}
                            //evidence.PeptideInfo = m_uniquePeptides[evidence.PeptideInfo.Peptide];
                            if (!m_uniqueDataSets.ContainsKey(evidence.DataSet.Name))
                            {
                                evidence.DataSet.Id = 0;
                                m_uniqueDataSets.Add(evidence.DataSet.Name, evidence.DataSet);
                            }
                            evidence.DataSet = m_uniqueDataSets[evidence.DataSet.Name];
                            evidence.Parent = consensusTarget;
                            
                        }
                        
                        
                        consensusTarget.Dataset = consensusTarget.Evidences[0].DataSet;
                        consensusTarget.ModificationCount = consensusTarget.Evidences[0].ModificationCount;
                        consensusTarget.ModificationDescription = consensusTarget.Evidences[0].ModificationDescription;
                        consensusTarget.MultiProteinCount = consensusTarget.Evidences[0].MultiProteinCount;
                        session.SaveOrUpdate(consensusTarget);

                        foreach (var protein in consensusTarget.Proteins)
                        {
                            if (!m_uniqueProteins.ContainsKey(protein.ProteinName))
                            {
                                //protein.Id = ++currentProt;
                                m_uniqueProteins.Add(protein.ProteinName, protein);
                                session.SaveOrUpdate(protein);
                            }
                            protein.Id = m_uniqueProteins[protein.ProteinName].Id;
                            var cProt = m_uniqueProteins[protein.ProteinName];
                            ConsensusProteinPair cPPair = new ConsensusProteinPair();
                            cPPair.Consensus = consensusTarget;
                            cPPair.Protein = cProt;
                            cPPair.CleavageState = (short)cProt.CleavageState;
                            cPPair.TerminusState = (short)cProt.TerminusState;
                            cPPair.ResidueStart = (short)cProt.ResidueStart;
                            cPPair.ResidueEnd = (short)cProt.ResidueEnd;
                            protein.ConsensusProtein.Add(cPPair);
                            session.SaveOrUpdate(cPPair);
                            consensusTarget.ConsensusProtein.Add(cPPair);
                        }

                        foreach (var ptm in consensusTarget.PTMs)
                        {
                            if (!m_uniquePtms.ContainsKey(ptm.Name))
                            {
                                //ptm.Id = ++currentPtm;
                                m_uniquePtms.Add(ptm.Name, ptm);
                                session.SaveOrUpdate(ptm);
                            }
                            ptm.Id = m_uniquePtms[ptm.Name].Id;
                            var cPtm = m_uniquePtms[ptm.Name];
                            var cPtmPair = new ConsensusPtmPair();
                            cPtmPair.Location = ptm.Location;
                            cPtmPair.PtmId = ptm.Id;
                            cPtmPair.ConsensusId = consensusTarget.Id;
                            session.SaveOrUpdate(cPtmPair);
                        }
                    }

                    //foreach(var protein in m_uniqueProteins)
                    //{
                    //}

                    //foreach(var ptm in m_uniquePtms)
                    //{
                    //}

                    //session.SaveOrUpdate(database.ConsensusTargets);

                    OnProgressChanged(new MtdbProgressChangedEventArgs(current, total, MtdbCreationProgressType.COMMIT.ToString()));

                    transaction.Commit();
                    session.Close();
                }
            }
        }
        #region Events

		/// <summary>
		/// Progress change reporting
		/// </summary>
        public event MtdbProgressChangedEventHandler ProgressChanged;

		/// <summary>
		/// Event Handler
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
