using System.Collections.Generic;
using System.IO;
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

        private readonly Dictionary<string, StreamWriter> m_alignmentWriters = new Dictionary<string, StreamWriter>();

        /// <summary>
        /// Write to the SQLite database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="options"></param>
        /// <param name="path"></param>
        public void Write(TargetDatabase database, Options options, string path)
        {
            DatabaseFactory.DatabaseFile = path;
            var databaseDirectory = Path.GetDirectoryName(path);
            /**********************************************************************************************
             * TODO: Get the append capability working
             * Set to false to avoid problems. Setting it to true will append some items, but not others.
             **********************************************************************************************/
            DatabaseFactory.ReadOrAppend = false;
            var sessionFactory = DatabaseFactory.CreateSessionFactory(options.DatabaseType);

            using (var session = sessionFactory.OpenStatelessSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    session.Insert(options);
                    /* This section breaks up the Target object, pulling out the individual TargetDataSet,  SequenceInfo,
                     * and TargetPeptideInfo. These objects are then "reverse linked", so that each of these objects
                     * relates to multiple evidences. This is because these objects need to know what they are related to.
                     * Additionally, these objects are saved before the Evidences are, because these objects need to already
                     * exist in order to properly generate the relation.
                     * */
                    var currentTarget   = 0;
                    var currentEv       = 0;
                    var datasetCount    = 0;
                    var total = database.ConsensusTargets.Count;
                    foreach (var consensusTarget in database.ConsensusTargets)
                    {
                        OnProgressChanged(new MtdbProgressChangedEventArgs(currentTarget, total,
                            MtdbCreationProgressType.COMMIT.ToString()));
                        consensusTarget.Id = ++currentTarget;
                        foreach (var ptm in consensusTarget.Ptms)
                        {
                            ptm.Id = 0;
                        }

                        consensusTarget.Dataset = consensusTarget.Evidences[0].DataSet;
                        consensusTarget.ModificationCount = consensusTarget.Evidences[0].ModificationCount;
                        consensusTarget.ModificationDescription = consensusTarget.Evidences[0].ModificationDescription;
                        consensusTarget.MultiProteinCount = consensusTarget.Evidences[0].MultiProteinCount;
                        session.Insert(consensusTarget);
                        foreach (var evidence in consensusTarget.Evidences)
                        {
                            if (!m_uniqueDataSets.ContainsKey(evidence.DataSet.Name))
                            {
                                evidence.DataSet.Id = ++datasetCount;
                                m_uniqueDataSets.Add(evidence.DataSet.Name, evidence.DataSet);
                                var outputPath = databaseDirectory + evidence.DataSet.Name + "Alignment.tsv";
                                var datasetWriter = new StreamWriter(databaseDirectory + "\\" + evidence.DataSet.Name + "Alignment.tsv");
                                datasetWriter.WriteLine("GANET_Obs\tScan_Number");
                                m_alignmentWriters.Add(evidence.DataSet.Name, datasetWriter);
                                session.Insert(evidence.DataSet);
                            }
                            var writtenEvidence = new Evidence
                            {
                                Id = ++currentEv,
                                Charge = evidence.Charge,
                                ObservedNet = evidence.ObservedNet,
                                NetShift = evidence.NetShift,
                                Mz = evidence.Mz,
                                Scan = evidence.Scan,
                                DelM = evidence.DelM,
                                DelMPpm = evidence.DelMPpm,
                                DiscriminantValue = evidence.DiscriminantValue,
                                SpecProb = evidence.SpecProb,
                                DataSet = m_uniqueDataSets[evidence.DataSet.Name],
                                Parent = consensusTarget
                            };
                            m_alignmentWriters[evidence.DataSet.Name].WriteLine("{0}\t{1}", writtenEvidence.ObservedNet, writtenEvidence.Scan);
                            if (writtenEvidence.DiscriminantValue > 0.0)
                            {
                                writtenEvidence.DiscriminantValue += 0.0;
                            }
                            session.Insert(writtenEvidence);
                        }

                        foreach (var protein in consensusTarget.Proteins)
                        {
                            if (!m_uniqueProteins.ContainsKey(protein.ProteinName))
                            {
                                protein.Id = 0;
                                m_uniqueProteins.Add(protein.ProteinName, protein);
                                session.Insert(protein);
                            }
                            var consensusProtein = m_uniqueProteins[protein.ProteinName];
                            var cPPair = new ConsensusProteinPair
                            {
                                Consensus = consensusTarget,
                                Protein = consensusProtein,
                                CleavageState = (short)consensusProtein.CleavageState,
                                TerminusState = (short)consensusProtein.TerminusState,
                                ResidueStart = (short)consensusProtein.ResidueStart,
                                ResidueEnd = (short)consensusProtein.ResidueEnd
                            };
                            session.Insert(cPPair);

                            consensusTarget.ConsensusProtein.Add(cPPair);
                        }

                        foreach (var ptm in consensusTarget.Ptms)
                        {
                            if (!m_uniquePtms.ContainsKey(ptm.Name))
                            {
                                m_uniquePtms.Add(ptm.Name, ptm);
                                session.Insert(ptm);
                            }
                            var cPtmPair = new ConsensusPtmPair
                            {
                                Location = ptm.Location,
                                PostTranslationalModification = m_uniquePtms[ptm.Name],
                                PtmId = m_uniquePtms[ptm.Name].Id,
                                Target = consensusTarget,
                                ConsensusId = consensusTarget.Id
                            };
                            session.Insert(cPtmPair);
                        }
                    }

                    OnProgressChanged(new MtdbProgressChangedEventArgs(currentTarget, total, MtdbCreationProgressType.COMMIT.ToString()));

                    transaction.Commit();
                    session.Close();
                }
            }
            foreach (var writer in m_alignmentWriters)
            {
                writer.Value.Close();
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
            ProgressChanged?.Invoke(this, e);
        }

        #endregion
    }
}
