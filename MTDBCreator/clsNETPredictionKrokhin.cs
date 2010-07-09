using System;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsNETPredictionKrokhin.
	/// </summary>
	public class clsNETPredictionKrokhin 
	{ 
		private const string PROGRAM_DATE = "September 21, 2005"; 
		private const float SLOPE_CORRECTION = 97.2712003F ; 
		private const float INTERCEPT_CORRECTION = 0.18064639848F ; 
		private const int ALKYLATION_INDEX = 26; 
		private string mAlkylationSymbol; 
		private bool mShowMessages; 
		private struct udtResidueCoefficients 
		{ 
			public char Residue; 
			public float Coefficient; 
			public float NTCoefficient; 
		} 
		private udtResidueCoefficients[] mResidueCoefficients; 
		private float[] mNTerminusWeightings; 

		public string AlkylationSymbol 
		{ 
			get 
			{ 
				return mAlkylationSymbol; 
			} 
			set 
			{ 
				mAlkylationSymbol = value; 
			} 
		} 

		public bool ShowMessages 
		{ 
			get 
			{ 
				return mShowMessages; 
			} 
			set 
			{ 
				mShowMessages = value; 
			} 
		} 

		public string ProgramDescription 
		{ 
			get 
			{ 
				string description = "Algorithm developed by Oleg Krokhin et. al. " ; 
				description += " VB.NET implementation written by Matthew Monroe at Battelle (PNNL). " ; 
				description += "C# implementation ported by Navdeep Jaitly.Ported from Adam Rauch's java implementation (FHCRC)." ;
				description += "Algorithm details are in: O.V. Krokhin, R. Craig, V. Spicer, W. Ens, K.G. Standing, R.C. Beavis, J.A. Wilkins. 2004. " ; 
				description += "\"An improved model for prediction of retention times of tryptic peptides " ; 
				description += "in ion pair reversed-phase HPLC - Its application to protein peptide mapping by off-line HPLC-MALDI MS" ;
				description +=  "\". Molecular & Cellular Proteomics, 3 (9) 908-919."; 
				return description ; 
			} 
		} 

		public string Version 
		{ 
			get 
			{ 
				return PROGRAM_DATE; 
			} 
		} 

		public float GetElutionTime(string strPeptide) 
		{ 
			int intTargetIndex; 
			int intPeptideLength; 
			float kl; 
			float sngRetentionTime; 
			if (!(ValidateAndCleanPeptide(ref strPeptide))) 
			{ 
				return 0; 
			} 
			intPeptideLength = strPeptide.Length; 
			if (intPeptideLength < 10) 
			{ 
				kl = ((float)(1 - 0.027 * (10 - intPeptideLength))); 
			} 
			else if (intPeptideLength > 20) 
			{ 
				kl = ((float)(1 / (1 + 0.015 * (intPeptideLength - 20)))); 
			} 
			else 
			{ 
				kl = 1; 
			} 
			sngRetentionTime = 0; 
			char [] strPeptideChars = strPeptide.ToCharArray() ; 
			for (int intIndex = 0; intIndex <= intPeptideLength - 1; intIndex++) 
			{ 
				if (char.IsUpper(strPeptideChars[intIndex])) 
				{ 
					intTargetIndex = strPeptideChars[intIndex] - 65; 
				} 
				else if (strPeptideChars[intIndex] == 'c') 
				{ 
					intTargetIndex = ALKYLATION_INDEX; 
				} 
				else 
				{ 
					intTargetIndex = -1; 
				} 
				if (intTargetIndex >= 0) 
				{ 
					sngRetentionTime += mResidueCoefficients[intTargetIndex].Coefficient; 
					if ((intIndex < mNTerminusWeightings.Length)) 
					{ 
						sngRetentionTime += mNTerminusWeightings[intIndex] * mResidueCoefficients[intTargetIndex].NTCoefficient; 
					} 
				} 
			} 
			sngRetentionTime *= kl; 
			if ((sngRetentionTime < 38)) 
			{ 
			} 
			else 
			{ 
				sngRetentionTime = ((float)(sngRetentionTime - 0.3 * (sngRetentionTime - 38))); 
			} 
			return (sngRetentionTime - INTERCEPT_CORRECTION) / SLOPE_CORRECTION; 
		} 

		private void InitializeVariables() 
		{ 
			mAlkylationSymbol = "*"; 
			mShowMessages = false; 
			mResidueCoefficients = new udtResidueCoefficients[27] ; 
			mNTerminusWeightings = new float[3] ; 

			// TODO: NotImplemented statement: ICSharpCode.SharpRefactory.Parser.AST.VB.ReDimStatement 
			for (int intIndex = 0; intIndex <= 25; intIndex++) 
			{
				mResidueCoefficients[intIndex] = new udtResidueCoefficients() ; 
				mResidueCoefficients[intIndex].Residue = Convert.ToChar(65 + intIndex) ; 
			} 
			mResidueCoefficients[ALKYLATION_INDEX].Residue = 'c'; 
			mResidueCoefficients[0].Coefficient = 0.8F; 
			mResidueCoefficients[1].Coefficient = -0.5F; 
			mResidueCoefficients[2].Coefficient = -0.8F; 
			mResidueCoefficients[3].Coefficient = -0.5F; 
			mResidueCoefficients[4].Coefficient = 0F; 
			mResidueCoefficients[5].Coefficient = 10.5F; 
			mResidueCoefficients[6].Coefficient = -0.9F; 
			mResidueCoefficients[7].Coefficient = -1.3F; 
			mResidueCoefficients[8].Coefficient = 8.4F; 
			mResidueCoefficients[9].Coefficient = 0F; 
			mResidueCoefficients[10].Coefficient = -1.9F; 
			mResidueCoefficients[11].Coefficient = 9.6F; 
			mResidueCoefficients[12].Coefficient = 5.8F; 
			mResidueCoefficients[13].Coefficient = -1.2F; 
			mResidueCoefficients[14].Coefficient = 0F; 
			mResidueCoefficients[15].Coefficient = 0.2F; 
			mResidueCoefficients[16].Coefficient = -0.9F; 
			mResidueCoefficients[17].Coefficient = -1.3F; 
			mResidueCoefficients[18].Coefficient = -0.8F; 
			mResidueCoefficients[19].Coefficient = 0.4F; 
			mResidueCoefficients[20].Coefficient = 0F; 
			mResidueCoefficients[21].Coefficient = 5F; 
			mResidueCoefficients[22].Coefficient = 11F; 
			mResidueCoefficients[23].Coefficient = 0F; 
			mResidueCoefficients[24].Coefficient = 4F; 
			mResidueCoefficients[25].Coefficient = 0F; 
			mResidueCoefficients[ALKYLATION_INDEX].Coefficient = -0.8F; 
			mResidueCoefficients[0].NTCoefficient = -1.5F; 
			mResidueCoefficients[1].NTCoefficient = 9F; 
			mResidueCoefficients[2].NTCoefficient = 4F; 
			mResidueCoefficients[3].NTCoefficient = 9F; 
			mResidueCoefficients[4].NTCoefficient = 7F; 
			mResidueCoefficients[5].NTCoefficient = -7F; 
			mResidueCoefficients[6].NTCoefficient = 5F; 
			mResidueCoefficients[7].NTCoefficient = 4F; 
			mResidueCoefficients[8].NTCoefficient = -8F; 
			mResidueCoefficients[9].NTCoefficient = 0F; 
			mResidueCoefficients[10].NTCoefficient = 4.6F; 
			mResidueCoefficients[11].NTCoefficient = -9F; 
			mResidueCoefficients[12].NTCoefficient = -5.5F; 
			mResidueCoefficients[13].NTCoefficient = 5F; 
			mResidueCoefficients[14].NTCoefficient = 0F; 
			mResidueCoefficients[15].NTCoefficient = 4F; 
			mResidueCoefficients[16].NTCoefficient = 1F; 
			mResidueCoefficients[17].NTCoefficient = 8F; 
			mResidueCoefficients[18].NTCoefficient = 5F; 
			mResidueCoefficients[19].NTCoefficient = 5F; 
			mResidueCoefficients[20].NTCoefficient = 0F; 
			mResidueCoefficients[21].NTCoefficient = -5.5F; 
			mResidueCoefficients[22].NTCoefficient = -4F; 
			mResidueCoefficients[23].NTCoefficient = 0F; 
			mResidueCoefficients[24].NTCoefficient = -3F; 
			mResidueCoefficients[25].NTCoefficient = 7F; 
			mResidueCoefficients[ALKYLATION_INDEX].NTCoefficient = 4F; 

			// TODO: NotImplemented statement: ICSharpCode.SharpRefactory.Parser.AST.VB.ReDimStatement 
			mNTerminusWeightings[0] = 0.42F; 
			mNTerminusWeightings[1] = 0.22F; 
			mNTerminusWeightings[2] = 0.05F; 
		} 

		private bool ValidateAndCleanPeptide(ref string strPeptide) 
		{ 
			if (strPeptide == null || strPeptide.Length == 0) 
			{ 
				return false; 
			} 
			strPeptide = strPeptide.Trim(); 
			if (mAlkylationSymbol == null || mAlkylationSymbol.Length == 0) 
			{ 
			} 
			else 
			{ 
				strPeptide = strPeptide.Replace("C" + mAlkylationSymbol, "c"); 
			} 
			if (strPeptide.Length > 4) 
			{ 
				if (strPeptide.Substring(1, 1) == "." & strPeptide.Substring(strPeptide.Length - 2, 1) == ".") 
				{ 
					strPeptide = strPeptide.Substring(2, strPeptide.Length - 4); 
				} 
			} 
			return true; 
		} 

		public clsNETPredictionKrokhin() 
		{ 
			InitializeVariables(); 
		} 
	}
}
