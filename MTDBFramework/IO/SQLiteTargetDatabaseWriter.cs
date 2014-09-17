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
        //private readonly Dictionary<string, TargetPeptideInfo> m_uniquePeptides = new Dictionary<string, TargetPeptideInfo>();
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
		            session.Save(options);
					/* This section breaks up the Target object, pulling out the individual TargetDataSet,  SequenceInfo,
                     * and TargetPeptideInfo. These objects are then "reverse linked", so that each of these objects 
                     * relates to multiple evidences. This is because these objects need to know what they are related to.
                     * Additionally, these objects are saved before the Evidences are, because these objects need to already
                     * exist in order to properly generate the relation. 
                     * */
                    var current = 0;
                    var total = database.ConsensusTargets.Count;
		            foreach (var consensusTarget in database.ConsensusTargets)
		            {
			            OnProgressChanged(new MtdbProgressChangedEventArgs(current, total,
				            MtdbCreationProgressType.COMMIT.ToString()));
			            consensusTarget.Id = 0;
			            foreach (var ptm in consensusTarget.Ptms)
			            {
				            ptm.Id = 0;
			            }
			            foreach (var evidence in consensusTarget.Evidences)
			            {
				            if (!m_uniqueDataSets.ContainsKey(evidence.DataSet.Name))
				            {
					            evidence.DataSet.Id = 0;
					            m_uniqueDataSets.Add(evidence.DataSet.Name, evidence.DataSet);
				            }
				            evidence.Id = 0;
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
				                protein.Id = 0;
					            m_uniqueProteins.Add(protein.ProteinName, protein);
					            session.SaveOrUpdate(protein);
				            }
				            var cProt = m_uniqueProteins[protein.ProteinName];
				            var cPPair = new ConsensusProteinPair
				            {
				                Consensus = consensusTarget,
				                Protein = cProt,
				                CleavageState = (short) cProt.CleavageState,
				                TerminusState = (short) cProt.TerminusState,
				                ResidueStart = (short) cProt.ResidueStart,
				                ResidueEnd = (short) cProt.ResidueEnd
				            };
			                session.SaveOrUpdate(cPPair);
				            consensusTarget.ConsensusProtein.Add(cPPair);
			            }

			            foreach (var ptm in consensusTarget.Ptms)
			            {
				            if (!m_uniquePtms.ContainsKey(ptm.Name))
				            {
					            m_uniquePtms.Add(ptm.Name, ptm);
					            session.SaveOrUpdate(ptm);
				            }
				            var cPtmPair = new ConsensusPtmPair
				            {
				                Location = ptm.Location,
                                PostTranslationalModification = m_uniquePtms[ptm.Name],
				                PtmId = m_uniquePtms[ptm.Name].Id,
                                Target = consensusTarget,
				                ConsensusId = consensusTarget.Id
				            };
			                session.SaveOrUpdate(cPtmPair);
			            }
		                current++;
		            }

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
