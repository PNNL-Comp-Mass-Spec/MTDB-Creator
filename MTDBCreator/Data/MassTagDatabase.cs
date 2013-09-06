using System.Collections.Generic;
using MTDBCreator.Algorithms;
using MTDBCreator.Data;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsMTDB.
	/// </summary>
    public class MassTagDatabase : IEnumerable<Protein>
	{
		public const float MISSING_F_SCORE = -100 ;

        /// <summary>
        /// Maps the mass tags ID to a mass tag.
        /// </summary>
        private Dictionary<string, ConsensusTarget> m_massTagMap;
        private List<ConsensusTarget>               m_massTags;

        /// <summary>
		/// Constructor
		/// </summary>
		public MassTagDatabase()
		{									
			m_massTags   = new List<ConsensusTarget>();			
            m_massTagMap = new Dictionary<string, ConsensusTarget>(); 
            Proteins     = new List<Protein>();
        }
        /// <summary>
        /// Copy constructor based on an existing database and a filtering mechanism.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filter"></param>
        public MassTagDatabase(MassTagDatabase database, ITargetFilter filter)
        {
            m_massTags   = new List<ConsensusTarget>();
            m_massTagMap = new Dictionary<string, ConsensusTarget>();            
            Proteins     = new List<Protein>();
            Filter(database, filter);
        }
        /// <summary>
        /// Gets the list of proteins
        /// </summary>
        public List<Protein> Proteins { get; set; }        
        /// <summary>
        /// List of the mass tags / consensus targets
        /// </summary>
        public List<ConsensusTarget> ConsensusTargets
        {
            get
            {
                return m_massTags;
            }
        }


		#region Insert Methods               
        /// <summary>
        /// Adds a list of mass tags to the database.
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="predictor"></param>
		private void AddMassTags(   List<Target> targets, 
                                    IRetentionTimePredictor predictor)
		{					
			foreach(Target target in targets)
            {                				                
				string peptideWithMod   = target.CleanSequence + target.SequenceData.ModificationDescription ; 				
				double highNorm         = target.HighNormalizedScore; 
				
                // Here we see if the mass tag has already been added to the database
                // If it has then update the tag's data if appropriate for the max values.
				if (m_massTagMap.ContainsKey(peptideWithMod))
				{
                    ConsensusTarget consensusTarget = m_massTagMap[peptideWithMod];
                    if (consensusTarget.HighNormalizedScore < highNorm)
					{
                        consensusTarget.HighNormalizedScore = highNorm; 
					}
                    if (consensusTarget.LogEValue > target.LogPeptideEValue)
					{
                        consensusTarget.LogEValue = target.LogPeptideEValue; 
					}

                    // Map the proteins
                    target.Proteins.ForEach(x => consensusTarget.AddProtein(x));
                    target.ParentTarget = consensusTarget;
                    consensusTarget.Targets.Add(target);

                    // Increment the number of observations
                    consensusTarget.NumberOfObservations++;                        					
				}
				else
				{
					// Otherwise we create a new mass tag entry into the database.
					ConsensusTarget consensusTarget                     = new ConsensusTarget() ; 
					consensusTarget.MonoisotopicMass            = target.MonoisotopicMass; 
					consensusTarget.Id                          = m_massTags.Count ; 
					consensusTarget.LogEValue                   = target.LogPeptideEValue ; 
					consensusTarget.HighNormalizedScore         = highNorm ; 
					consensusTarget.NumberOfObservations        = 1 ;
					consensusTarget.ModificationCount           = target.SequenceData.ModificationCount ;
                    consensusTarget.ModificationDescription     = target.SequenceData.ModificationDescription; 
					consensusTarget.MultipleProteins            = target.MultiProteinCount ; 
					consensusTarget.PmtQualityScore             = 0 ; 
					consensusTarget.Sequence                    = peptideWithMod; 
					consensusTarget.CleanSequence               = Target.CleanPeptide(consensusTarget.Sequence);                    
                    consensusTarget.NetPredicted                = predictor.GetElutionTime(target.CleanSequence);                    					
                    consensusTarget.PeptideObservationCountPassingFilter = 0;                    
                    consensusTarget.Targets.Add(target);
                    target.Proteins.ForEach(x => consensusTarget.AddProtein(x));
                    

                    // Then link the child target to these parents
                    target.ParentTarget = consensusTarget;

					m_massTags.Add(consensusTarget) ; 	
				    m_massTagMap.Add(peptideWithMod, consensusTarget);
				}				
			}			
		}
        /// <summary>
        /// Adds a list of peptide results to the database.
        /// </summary>
        /// <param name="peptides"></param>
        /// <param name="type"></param>
        public void AddResults( List<Target> targets,
                                RegressionTypeIdentifier type,
                                IRetentionTimePredictor predictor)
        {            
            AddMassTags(targets, predictor);	                   
        }
		#endregion

		/// <summary>
        /// Calculates the NET statistics for a mass tag
        /// </summary>
		public void FinalizeDatabase()
		{
            m_massTags.ForEach(x => x.CalculateStatistics());

            AggregateProteins();
		}
        /// <summary>
        /// Aggregate the proteins based on the data from the mass tags
        /// </summary>
        private void AggregateProteins()
        {
            Dictionary<string, Protein> proteinMap = new Dictionary<string, Protein>();

            foreach(ConsensusTarget tag in m_massTags)
            {
                List<Protein> proteins = tag.GetProteins();
                foreach (Protein p in proteins)
                {
                    if (!proteinMap.ContainsKey(p.Reference))
                    {
                        proteinMap.Add(p.Reference, p);
                        p.AddConsensusTarget(tag);                        
                    }
                    else
                    {
                        Protein existingProtein = proteinMap[p.Reference];
                        existingProtein.AddConsensusTarget(tag);        // Add the mass tag to the protein
                        tag.AddProtein(existingProtein);                // Add the protein to the mass tag...
                    }
                }
            }

            int globalCount = 0;
            foreach (Protein p in proteinMap.Values)
            {
                p.Id = globalCount++;                
                Proteins.Add(p);
            }
        }
        /// <summary>
        /// Copies the exsting database.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private void Filter(MassTagDatabase database, ITargetFilter filter)
        {            
            Dictionary<int, bool> targets   = new Dictionary<int, bool>();

            foreach (ConsensusTarget averageTarget in database.ConsensusTargets)
            {
                /// Perform any filtering now...we allow the mass tag to pass if one of the targets is allowed to pass through...
                bool shouldFilter = true;
                foreach (Target target in averageTarget.Targets)
                {
                    if (!filter.ShouldFilter(target))
                    {
                        shouldFilter = false;
                        break;
                    }
                }

                // Here we update the map, so that if it was filtered above, then we mark it As such so we dont add the protein data
                if (!targets.ContainsKey(averageTarget.Id))
                {
                    targets.Add(averageTarget.Id, shouldFilter);
                }
                targets[averageTarget.Id] = shouldFilter;
                if (shouldFilter) 
                    continue;

                m_massTags.Add(averageTarget);
            }                        
            AggregateProteins();
        }

        #region IEnumerable<Protein> Members

        public IEnumerator<Protein> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}