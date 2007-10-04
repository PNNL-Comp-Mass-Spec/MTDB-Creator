using System;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsOptions.
	/// </summary>
	public class clsOptions
	{
		private double [] marrXCorrForExportTryptic = new double [] {1.5,2.0,2.5} ; 
		private double [] marrXCorrForExportPartiallyTryptic = new double [] {1.5,2.0,2.5} ; 
		private double [] marrXCorrForExportNonTryptic = new double [] {3.0,3.5,4.0} ; 

		private bool mblnUseDelCN = true ; 
		private double mdblMaxDelCN = 0.1 ; 

		private bool mblnExportTryptic = true ; 
		private bool mblnExportPartiallyTryptic = true ; 
		private bool mblnExportNonTryptic = true ; 

		private double mdblMaxLogEValForXTandemExport = -2.0 ;
		private double mdblMaxLogEValForXTandemAlignment = -2.0 ;

		private short mshortMaxModsForAlignment = 2 ; 
		private short mshortMaxRankForExport = 2 ; 
		private short mshortMinObservationsForExport = 2 ; 

		private double mdblMinXCorrForAlignment = 3.0 ; 

		private bool mblnUseKrokhinNET = false ; 
		private Regressor.clsRegressor.RegressionType menmRegressionType = Regressor.clsRegressor.RegressionType.MIXTURE_REGRESSION ; 
		private short mshortRegressionOrder = 1 ; 

		public clsOptions()
		{
			//
			// TODO: Add constructor logic here
			//
#if BASIC
			mblnUseKrokhinNET = true ; 
#endif
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

		public double MinDelCNForExport
		{
			get
			{
				return mdblMaxDelCN; 
			}
			set
			{
				mdblMaxDelCN = value ; 
			}
		}
		public double MinXCorrForAlignment
		{
			get
			{
				return mdblMinXCorrForAlignment ; 
			}
			set
			{
				mdblMinXCorrForAlignment = value ; 
			}
		}

		public bool UseDelCN
		{
			get
			{
				return mblnUseDelCN ; 
			}
			set
			{
				mblnUseDelCN = value ; 
			}
		}

		public double MaxDelCN 
		{
			get
			{
				return mdblMaxDelCN ; 
			}
			set
			{
				mdblMaxDelCN = value ; 
			}
		}

		public bool ExportTryptic
		{
			get
			{
				return mblnExportTryptic ; 
			}
			set
			{
				mblnExportTryptic = value ; 
			}
		}

		public bool ExportPartiallyTryptic
		{
			get
			{
				return mblnExportPartiallyTryptic ; 
			}
			set
			{
				mblnExportPartiallyTryptic = value ; 
			}
		}

		public bool ExportNonTryptic
		{
			get
			{
				return mblnExportNonTryptic ; 
			}
			set
			{
				mblnExportNonTryptic = value ; 
			}
		}

		public double MaxLogEValForXTandemExport
		{
			get
			{
				return mdblMaxLogEValForXTandemExport ; 
			}
			set
			{
				mdblMaxLogEValForXTandemExport = value ; 
			}
		}

		public double MaxLogEValForXTandemAlignment
		{
			get
			{
				return mdblMaxLogEValForXTandemAlignment ; 
			}
			set
			{
				mdblMaxLogEValForXTandemAlignment = value ; 
			}
		}

		public short MaxModificationsForAlignment
		{
			get
			{
				return mshortMaxModsForAlignment ; 
			}
			set
			{
				mshortMaxModsForAlignment = value ; 
			}
		}

		public short MaxRankForExport
		{
			get
			{
				return mshortMaxRankForExport ; 
			}
			set
			{
				mshortMaxRankForExport = value ; 
			}
		}

		public short MinObservationsForExport
		{
			get
			{
				return mshortMinObservationsForExport ;
			}
			set
			{
				mshortMinObservationsForExport = value ; 
			}
		}

		public bool UseKrokhinNET
		{
			get
			{
				return mblnUseKrokhinNET ; 
			}
			set
			{
				mblnUseKrokhinNET = value ; 
			}
		}

		public Regressor.clsRegressor.RegressionType RegressionType
		{
			get
			{
				return menmRegressionType ; 
			}
			set
			{
				menmRegressionType = value ; 
			}
		}

		public short RegressionOrder
		{
			get
			{
				return mshortRegressionOrder ;
			}
			set
			{
				mshortRegressionOrder = value ;
			}
		}
	
		public bool IsToBeExported(clsSequestResults seqResult)
		{
			short charge = seqResult.mshort_ChargeState ; 
			if (charge > clsSequestResults.MAX_CHARGE_FOR_FSCORE)
				charge = clsSequestResults.MAX_CHARGE_FOR_FSCORE ; 
			if (seqResult.mshort_NumTrypticEnds == 2)
			{
				if (!mblnExportTryptic)
					return false ; 
				if (mblnUseDelCN && seqResult.mdbl_DelCn > mdblMaxDelCN)
					return false ; 
				if (seqResult.mdbl_XCorr >= marrXCorrForExportTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			if (seqResult.mshort_NumTrypticEnds == 1)
			{
				if (!mblnExportPartiallyTryptic)
					return false ; 
				if (mblnUseDelCN && seqResult.mdbl_DelCn > mdblMaxDelCN)
					return false ; 
				if (seqResult.mdbl_XCorr >= marrXCorrForExportPartiallyTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			if (seqResult.mshort_NumTrypticEnds == 0)
			{
				if (!mblnExportNonTryptic)
					return false ; 
				if (mblnUseDelCN && seqResult.mdbl_DelCn > mdblMaxDelCN)
					return false ; 
				if (seqResult.mdbl_XCorr >= marrXCorrForExportNonTryptic[charge-1])
					return true ; 
				else
					return false ; 
			}
			return false ; 
		}
		public bool IsToBeExported(clsXTandemResults xtResult)
		{
			if (xtResult.mdbl_log_peptide_e_value > mdblMaxLogEValForXTandemExport)
				return false ; 
			if (xtResult.mshort_tryptic_state == 2)
			{
				if (!mblnExportTryptic)
					return false ; 
				else
					return true ; 
			}
			if (xtResult.mshort_tryptic_state == 1)
			{
				if (!mblnExportPartiallyTryptic)
					return false ; 
				else
					return true ; 
			}
			if (xtResult.mshort_tryptic_state == 0)
			{
				if (!mblnExportNonTryptic)
					return false ; 
				else
					return true ; 
			}
			return false ; 
		}
	}
}
