using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;
using System.Collections;

namespace MTDBCreator.Algorithms
{
    public class LcmsRegressor : IRegressionAlgorithm
    {
        private Regressor.clsRegressor m_regressor;

        /// <summary>
        /// constructor
        /// </summary>
        public LcmsRegressor()
        {
            m_regressor = new Regressor.clsRegressor(); 
        }

        public LcmsRegressor(RegressionTypeIdentifier identifier)
        {
            m_regressor = new Regressor.clsRegressor();
            
        }

        /// <summary>
        /// Gets or sets the type of regression to use.
        /// </summary>
        public RegressionTypeIdentifier RegressionType { get; set; }
        /// <summary>
        /// Gets or sets the regression order for this function
        /// </summary>
        public int RegressionOrder { get; set; }
        
        /// <summary>
        /// Calculates the regression function between the original and predicted or 
        /// </summary>
        /// <param name="peptideScans"></param>
        /// <param name="peptidePredictedNET"></param>
        /// <returns></returns>
        public RegressionResult CalculateRegression(List<float> observed, List<float> basis)
		{
            float[] observedArray   = new float[observed.Count];
            float[] basisArray      = new float[basis.Count];

            observed.CopyTo(observedArray);
            basis.CopyTo(basisArray);

            m_regressor.SetPoints(ref observedArray, ref basisArray);
            m_regressor.PerformRegression((Regressor.RegressionType)RegressionType); 
			
            RegressionResult result = new RegressionResult();
            result.NETSlope         = m_regressor.Slope;
            result.NETIntercept     = m_regressor.Intercept;
            result.NETRSquared      = m_regressor.RSquared;

            return result;            
		}
        /// <summary>
        /// Aligns all targets using the regression data.
        /// </summary>
        /// <param name="targets"></param>
		public void ApplyTransformation(List<Target> targets)
		{			
			foreach(Target target in targets)            
				target.NetAligned = GetTransformedNET(target.Scan); 			
		}
        
        public double GetTransformedNET(int scan)
        {
            return Convert.ToDouble(m_regressor.GetNETFromScan(Convert.ToSingle(scan)));            
        }
    }
}

//// contains first scan number each observed peptide was seen in.
//            Hashtable peptideTable  = new Hashtable(results.Sequences.Count) ; 

			



            //peptideScans            = new float [peptideTable.Count] ; 
            //peptidePredictedNET     = new float [peptideTable.Count] ; 
            //int numElementsSoFar    = 0 ; 

            //// now for each peptide calculate theoretical NET value. 
            //foreach (int massTagIndex in peptideTable.Keys)
            //{
            //    MassTag massTag                         = m_massTags[massTagIndex] ; 
            //    peptideScans[numElementsSoFar]          = Convert.ToSingle((int) peptideTable[massTagIndex]) ; 
            //    peptidePredictedNET[numElementsSoFar]   = Convert.ToSingle(massTag.GaNetAverage); 
            //    numElementsSoFar++ ; 
            //}



        //public RegressionResult AlignTargetsToTheoreticalNETs(List<Target> peptides,                                                              
        //                                                      IRetentionTimePredictor predictor)
        //{
        //    RegressionResult result = null;
        //    try
        //    {
        //        // contains first scan number each observed peptide was seen in.
        //       Dictionary<string, Target> peptideTable = new Dictionary<string, Target>();
			
        //        /// Find the latest scan...
        //        foreach(Target peptide in peptides)
        //        {
					
        //            if (peptideTable.ContainsKey(peptide.CleanSequence))
        //            {
        //                Target existing = peptideTable[peptide.CleanSequence];

        //                if (existing.Scan < existing.Scan)
        //                    peptideTable[peptide.CleanSequence] = peptide; 
        //            }
        //            else					
        //                peptideTable.Add(peptide.CleanSequence, peptide);
					
        //            if (peptide.Scan > numScans)					
        //                numScans = peptide.Scan; 
					
        //        }

        //        // now for each peptide calculate theoretical NET value. 
        //        float [] peptideScans            = new float [peptideTable.Keys.Count] ;
        //        float [] peptidePredictedNET = new float[peptideTable.Keys.Count]; 
        //        int i                   = 0; 				
        //        foreach (string cleanPeptide in peptideTable.Keys)
        //        {
        //            peptideScans[i]         = Convert.ToSingle(peptideTable[cleanPeptide].Scan);
        //            peptidePredictedNET[i]  = Convert.ToSingle(predictor.GetElutionTime(cleanPeptide));                    					
        //            i++ ; 
        //        }

        //        result = new RegressionResult();
        //        try
        //        {
        //            m_regressor.SetPoints(ref peptideScans, ref peptidePredictedNET);
        //            m_regressor.PerformRegression((Regressor.RegressionType)RegressionType);

        //            result = new RegressionResult();
        //            result.NETSlope     = m_regressor.Slope;
        //            result.NETIntercept = m_regressor.Intercept;
        //            result.NETIntercept = m_regressor.RSquared; 					
        //        }
        //        catch (Exception ex)
        //        {                    
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {								                
        //        throw ex;
        //    }
        //    return result;
        //}