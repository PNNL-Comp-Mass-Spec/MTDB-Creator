using System;
using MTDBCreator.Data;
using System.Collections.Generic;

namespace MTDBCreator
{
	/// <summary>
	/// Averaged observance of a peptide / protein (top down).  Previously called mass tag.
	/// </summary>
	public class ConsensusTarget
	{
        public const double DEFAULT_XCORR = 100;
        public const double DEFAULT_MIN_LOG_EVAL = 100;

        /// <summary>
        /// Manages list of proteins
        /// </summary>
        private Dictionary<string, Protein> m_proteinMap;

        public ConsensusTarget()
        {
            HighNormalizedScore     = DEFAULT_XCORR;
            LogEValue               = DEFAULT_MIN_LOG_EVAL;
            FScore_CS_Sum           = new float [SequestResult.MAX_CHARGE_FOR_FSCORE] ;
            FScore_CS_Count         = new short[SequestResult.MAX_CHARGE_FOR_FSCORE];

            for (int chargeNum = 0; chargeNum < SequestResult.MAX_CHARGE_FOR_FSCORE; chargeNum++)
            {
                FScore_CS_Count[chargeNum]  = 0;
                FScore_CS_Sum[chargeNum]    = 0;
            }

            Targets = new List<Target>();


            m_proteinMap = new Dictionary<string, Protein>();
        }

        /// <summary>
        /// Gets a list of the proteins available.
        /// </summary>
        public List<Protein> GetProteins()
        {            
            List<Protein> proteins = new List<Protein>();
            foreach (Protein protein in m_proteinMap.Values)
            {
                proteins.Add(protein);
            }
            return proteins;            
        }
        /// <summary>
        /// Adds the protein to the target, and maps the target onto the protein
        /// </summary>
        /// <param name="p"></param>
        public void AddProtein(Protein p)
        {
            if (!m_proteinMap.ContainsKey(p.Reference))
            {
                m_proteinMap.Add(p.Reference, p);
            }
            else
            {
                m_proteinMap[p.Reference] = p;
            }
        }

        
        public List<Target> Targets { get; set; }
		public int      Id {get;set;} 
		public string   Sequence {get;set;}
		public string   CleanSequence {get;set;}
		public double   MonoisotopicMass {get;set;} 
		public short    MultipleProteins {get;set;} 
		public int      NumberOfObservations{get;set;} 
		public int      PeptideObservationCountPassingFilter{get;set;} 
		public double   HighNormalizedScore {get;set;} 
		public double   HighPeptideProphetProbability {get;set;} 
		public double   LogEValue {get;set;} 
		public short    ModificationCount{get;set;} 
		public string   ModificationDescription{get;set;} 
		public short    PmtQualityScore {get;set;} 

		public double   GaNetMinium  {get;set;} 
		public double   GaNetMax     {get;set;} 
		public double   GaNetAverage {get;set;} 
		public double   GaNetCount   {get;set;} 
		public double   GaNetStdev   {get;set;} 
		public double   GaNetStderr  {get;set;} 
		public double   NetPredicted {get;set;} 

		/// <summary>
		/// For peptides with charge greater than this charge, the discriminant
		/// scores are rolled into the distribution for this charge. 
		/// </summary>
		public float [] FScore_CS_Sum   {get;set;}
		public short [] FScore_CS_Count {get;set;}

        /// <summary>
        /// Calculates the statistics using the targets identified for this mass tag.
        /// </summary>
        public void CalculateStatistics()
        {
            double sumSquare    = 0 ; 
			double sum          = 0 ; 
			double min_ganet    = double.MaxValue ; 
			double max_ganet    = double.MinValue ;
            int numObservations = Targets.Count;
			foreach(Target target in Targets)
			{
                float val = (float)target.NetAligned;
				if (val > max_ganet)
					max_ganet = val ; 
				if (val < min_ganet)
					min_ganet = val ; 
				sum += val ; 
				sumSquare += val * val ; 
			}

			double std = 0 ; 
			if (numObservations > 1)
                std = Math.Sqrt((numObservations * sumSquare - sum * sum) / (numObservations * numObservations - 1));

            int massTagId   = Id;
			GaNetCount      = numObservations;
            GaNetAverage    = sum / numObservations;
			GaNetStdev      = std;
			GaNetStderr     = std / Math.Sqrt(numObservations); 
			GaNetMinium     = min_ganet;
			GaNetMax        = max_ganet;
        }
        public int ConformerCharge { get; set; } 
        public int ConformerId { get; set; } 

        public double DriftTime { get; set; }
    }
}
