using System.Collections.Generic;
using System.IO;
using System.Linq;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using MTDBFramework.UI;

namespace MTDBFramework.Database
{
    public class SQLiteTargetDatabaseWriter : ITargetDatabaseWriter
    {

        // TODO: Implement these (or maybe dictionaries)
        private Dictionary<string, TargetPeptideInfo> uniquePeptides = new Dictionary<string, TargetPeptideInfo>();
        private Dictionary<string, TargetDataSet> uniqueDataSets = new Dictionary<string, TargetDataSet>();
        
        // Testing adding a table for the protein references
        private Dictionary<string, ProteinInformation> uniqueProteins = new Dictionary<string, ProteinInformation>();

        public void Write(TargetDatabase database, Options options, string path)
        {

            DatabaseCreatorFactory.DbFile = path;
            var sessionFactory = DatabaseCreatorFactory.CreateSessionFactory();

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
                    int current = 0;
                    int total = database.ConsensusTargets.Count;
                    session.Save(options);
                    foreach (ConsensusTarget consensusTarget in database.ConsensusTargets)
                    {
                        consensusTarget.Id = ++current;
                        foreach (Evidence t in consensusTarget.Evidences)
                        {
                            /*foreach(ProteinInformation protein in t.Proteins)
                            {
                                if(!uniqueProteins.ContainsKey(protein.ProteinName))
                                {
                                    uniqueProteins.Add(protein.ProteinName, protein);                                    
                                }
                                Proteins.Add(protein);

                            }*/
                            if (!uniquePeptides.ContainsKey(t.PeptideInfo.Peptide/*InfoSequence*/))
                            {
                                uniquePeptides.Add(t.PeptideInfo.Peptide/*InfoSequence*/, t.PeptideInfo);
                            }
                            t.PeptideInfo = uniquePeptides[t.PeptideInfo.Peptide/*InfoSequence*/];
                            if (!uniqueDataSets.ContainsKey(t.DataSet.Path))
                            {
                                uniqueDataSets.Add(t.DataSet.Path, t.DataSet);
                            }
                            t.DataSet = uniqueDataSets[t.DataSet.Path];
                            t.Parent = consensusTarget;
                            
                        }
                        consensusTarget.Dataset = consensusTarget.Evidences[0].DataSet;
                        //foreach (ProteinInformation p in consensusTarget.Proteins)
                        //{
                        //    if (!uniqueProteins.ContainsKey(p.ProteinName))
                        //    {
                        //        p.Id = ++protNum;
                        //        uniqueProteins.Add(p.ProteinName, p);
                        //    }
                        //    else
                        //    {
                        //        p.Id = uniqueProteins[p.ProteinName].Id;
                        //    }
                        //}

                        session.SaveOrUpdate(consensusTarget);
                    }
                    current = -1;
                    total = 0;

                    OnProgressChanged(new MTDBProgressChangedEventArgs(current, total, MTDBCreationProgressType.Commit.ToString()));

                    transaction.Commit();
                    session.Close();
                }
            }
        }
        #region Events

        public event MTDBProgressChangedEventHandler ProgressChanged;

        protected void OnProgressChanged(MTDBProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        #endregion
    }
}
