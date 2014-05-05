using System;
using Regressor;
using MTDBCreator.Data;
using MTDBCreator.Algorithms;
using NETPrediction;

namespace MTDBCreator.Data
{
	/// <summary>
	/// Maintains list of options for application
	/// </summary>
	public class Options: ITargetFilter
	{

		private double [] marrXCorrForExportTryptic = new double [] {1.5,2.0,2.5} ; 
		private double [] marrXCorrForExportPartiallyTryptic = new double [] {1.5,2.0,2.5} ; 
		private double [] marrXCorrForExportNonTryptic = new double [] {3.0,3.5,4.0} ; 
				
		public Options()
		{
            PredictorType                   = RetentionTimePredictionType.Kangas;
            UseDelCN                        = true ; 
		    MaxDelCN                        = 0.1 ; 
		    ExportTryptic                   = true ; 
		    ExportPartiallyTryptic          = true ; 
		    ExportNonTryptic                = true ; 
		    MaxLogEValForXTandemExport      = -2.0 ;
		    MaxLogEValForXTandemAlignment   = -2.0 ;
		    MaxModsForAlignment             = 2 ; 
		    MaxRankForExport                = 2 ; 
		    MinObservationsForExport        = 2 ; 
		    MinXCorrForAlignment            = 3.0 ;
            MsgfFDR                         = .01;
            MsgfSpectralEValue              = .05;
            Regression          = RegressionTypeIdentifier.MIXTURE_REGRESSION;
            TargetFilterType    = FilterType.BottomUp; 
		}

        

        /// <summary>
        /// Gets or sets the target filter type.
        /// </summary>
        public FilterType TargetFilterType
        {
            get;
            set;
        }

		public double  [] MinXCorrForExportTrytpic
		{
			get
			{
				return (double []) marrXCorrForExportTryptic.Clone() ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportTrytpic") ; 
				marrXCorrForExportTryptic = (double [])value.Clone() ; 
			}
		}

        public int MaxModsForAlignment
        {
            get;
            set;
        }
		public double  [] MinXCorrForExportPartiallyTrytpic
		{
			get
			{
				return (double []) marrXCorrForExportPartiallyTryptic.Clone() ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportPartiallyTrytpic") ; 
				marrXCorrForExportPartiallyTryptic = (double [])value.Clone() ; 
			}
		}
		public double  [] MinXCorrForExportNonTrytpic
		{
			get
			{
				return (double []) marrXCorrForExportNonTryptic.Clone() ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportNonTrytpic") ; 
				marrXCorrForExportNonTryptic = (double [])value.Clone() ; 
			}
		}
        /// <summary>
        /// Gets or sets the prediction algorithm
        /// </summary>
        public iPeptideElutionTime PredictionAlgorithm
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the retention time predictor
        /// </summary>
        public RetentionTimePredictionType PredictorType
        {
            get;
            set;
        }

        public double MinDelCNForExport
        {
            get;
            set;
        }
		public double MinXCorrForAlignment
        {
            get;
            set;
		}

		public bool UseDelCN
        {
            get;
            set;
		}

		public double MaxDelCN
        {
            get;
            set;
		}

		public bool ExportTryptic
        {
            get;
            set;
		}

        public bool ExportPartiallyTryptic
        {
            get;
            set;
        }

		public bool ExportNonTryptic
        {
            get;
            set;
		}

        public double MaxLogEValForXTandemExport
        {
            get;
            set;
        }

        public double MaxLogEValForXTandemAlignment
        {
            get;
            set;
        }

		public short MaxModificationsForAlignment
        {
            get;
            set;
		}

        public short MaxRankForExport
        {
            get;
            set;
        }

        public short MinObservationsForExport
        {
            get;
            set;
        }

        public RegressionTypeIdentifier Regression
        {
            get;
            set;
        }

        public short RegressionOrder
        {
            get;
            set;
        }
        public double MsgfSpectralEValue
        {
            get;
            set;
        }
        public double MsgfFDR
        {
            get;
            set;
        }

        public bool IsToBeExported(MsgfPlusResult result)
        {

            if (result.Fdr > MsgfFDR)
            {
                return false;
            }
            if (result.SpectralProbability > MsgfSpectralEValue)
            {
                return false;
            }


            if (result.NumTrypticEnds == 2)
            {
                if (!ExportTryptic)
                    return false;
                else
                    return true;
            }
            if (result.NumTrypticEnds == 1)
            {
                if (!ExportPartiallyTryptic)
                    return false;
                else
                    return true;
            }
            if (result.NumTrypticEnds == 0)
            {
                if (!ExportNonTryptic)
                    return false;
                else
                    return true;
            }

            return true;
        }

		public bool IsToBeExported(SequestResult seqResult)
		{
			short charge = seqResult.Charge ;
            if (charge > SequestResult.MAX_CHARGE_FOR_FSCORE)
				charge = SequestResult.MAX_CHARGE_FOR_FSCORE ; 
			if (seqResult.NumTrypticEnds == 2)
			{
				if (!ExportTryptic)
					return false ; 
				if (UseDelCN && seqResult.DelCn > MaxDelCN)
					return false ; 
				if (seqResult.XCorr >= marrXCorrForExportTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			if (seqResult.NumTrypticEnds == 1)
			{
				if (!ExportPartiallyTryptic)
					return false ; 
				if (UseDelCN && seqResult.DelCn > MaxDelCN)
					return false ; 
				if (seqResult.XCorr >= marrXCorrForExportPartiallyTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			if (seqResult.NumTrypticEnds == 0)
			{
				if (!ExportNonTryptic)
					return false ; 
				if (UseDelCN && seqResult.DelCn > MaxDelCN)
					return false ; 
				if (seqResult.XCorr >= marrXCorrForExportNonTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			return false ; 
		}
		public bool IsToBeExported(XTandemResult xtResult)
		{
			if (xtResult.LogPeptideEValue > MaxLogEValForXTandemExport)
				return false ; 
			if (xtResult.TrypticState == 2)
			{
				if (!ExportTryptic)
					return false ; 
				else
					return true ; 
			}
			if (xtResult.TrypticState == 1)
			{
				if (!ExportPartiallyTryptic)
					return false ; 
				else
					return true ; 
			}
			if (xtResult.TrypticState == 0)
			{
				if (!ExportNonTryptic)
					return false ; 
				else
					return true ; 
			}
			return false ; 
		}

        #region ITargetFilter Members
        /// <summary>
        /// Determines if the target should be filtered or not.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ShouldFilter(Target t)
        {
            
            // This is a bad way to do this...but
            // I don't want to put the immediate effort into redesigning this...more work
            // is needed to segment out the options class out of a general options class and into
            // a concrete class or classes
            SequestResult sequest = t as SequestResult;
            if (sequest != null)
            {
                // If it is to be exported, then we should not filter it
                return !IsToBeExported(sequest);
                
            }
            XTandemResult xtandem = t as XTandemResult;
            if (xtandem != null)
            {
                return !IsToBeExported(xtandem);
            }

            MsgfPlusResult msgfResult = t as MsgfPlusResult;
            if (msgfResult != null)
            {
                return !IsToBeExported(msgfResult);
            }

            return true;
        }
        #endregion

		public override int GetHashCode()
		{
			int hash = 13;

			hash = (hash * 7) + ExportNonTryptic.GetHashCode();
			hash = (hash * 7) + ExportPartiallyTryptic.GetHashCode();
			hash = (hash * 7) + ExportTryptic.GetHashCode();
			hash = (hash * 7) + MaxDelCN.GetHashCode();
			hash = (hash * 7) + MaxLogEValForXTandemAlignment.GetHashCode();
			hash = (hash * 7) + MaxLogEValForXTandemExport.GetHashCode();
			hash = (hash * 7) + MaxModificationsForAlignment.GetHashCode();
			hash = (hash * 7) + MaxModsForAlignment.GetHashCode();
			hash = (hash * 7) + MaxRankForExport.GetHashCode();
			hash = (hash * 7) + MinDelCNForExport.GetHashCode();
			hash = (hash * 7) + MinObservationsForExport.GetHashCode();
			hash = (hash * 7) + MinXCorrForAlignment.GetHashCode();
			hash = (hash * 7) + MinXCorrForExportNonTrytpic.GetHashCode();
			hash = (hash * 7) + MinXCorrForExportPartiallyTrytpic.GetHashCode();
			hash = (hash * 7) + MinXCorrForExportTrytpic.GetHashCode();
			if (PredictionAlgorithm != null)
				hash = (hash * 7) + PredictionAlgorithm.GetHashCode();
			hash = (hash * 7) + PredictorType.GetHashCode();
			hash = (hash * 7) + Regression.GetHashCode();
			hash = (hash * 7) + RegressionOrder.GetHashCode();
			hash = (hash * 7) + TargetFilterType.GetHashCode();
			hash = (hash * 7) + UseDelCN.GetHashCode();

			return hash;
		}

        public override bool Equals(object obj)
        {
            Options other = obj as Options;
            if (other == null)
                return false;

            bool compares = true;
            compares = compares && (other.ExportNonTryptic == ExportNonTryptic);
            compares = compares && (other.ExportPartiallyTryptic == ExportNonTryptic);
            compares = compares && (other.ExportTryptic == ExportNonTryptic);
            compares = compares && (other.MaxDelCN == MaxDelCN);
            compares = compares && (other.MaxLogEValForXTandemAlignment == MaxLogEValForXTandemAlignment);
            compares = compares && (other.MaxLogEValForXTandemExport == MaxLogEValForXTandemExport);
            compares = compares && (other.MaxModificationsForAlignment == MaxModificationsForAlignment);
            compares = compares && (other.MaxModsForAlignment == MaxModsForAlignment);
            compares = compares && (other.MaxRankForExport == MaxRankForExport);
            compares = compares && (other.MinDelCNForExport == MinDelCNForExport);
            compares = compares && (other.MinObservationsForExport == MinObservationsForExport);
            compares = compares && (other.MinXCorrForAlignment == MinXCorrForAlignment);
            compares = compares && (other.MinXCorrForExportNonTrytpic == MinXCorrForExportNonTrytpic);
            compares = compares && (other.MinXCorrForExportPartiallyTrytpic == MinXCorrForExportPartiallyTrytpic);
            compares = compares && (other.MinXCorrForExportTrytpic == MinXCorrForExportTrytpic);
            compares = compares && (other.PredictionAlgorithm == PredictionAlgorithm);
            compares = compares && (other.PredictorType == PredictorType);
            compares = compares && (other.Regression == Regression);
            compares = compares && (other.RegressionOrder == RegressionOrder);
            compares = compares && (other.TargetFilterType == TargetFilterType);
            compares = compares && (other.UseDelCN == UseDelCN);

            return compares;
        }
    }
}
