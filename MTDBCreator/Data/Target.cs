using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{

    public class Target
    {
        public Target()
        {
            Proteins        = new List<Protein>();
            SequenceData    = new Sequence();
            ParentTarget    = null;
            IsPredicted     = false;
        }

        /// <summary>
        /// Gets or sets the ID of the target
        /// </summary>
        public int      Id               { get; set; }
        public string   Sequence         { get; set; }
        public string   CleanSequence    { get; set; }
        public int      Scan             { get; set; }
        public short    Charge           { get; set; }
        public double   NetAligned      { get; set; }
        public double   NetPredicted     {get;set;}
        public double   MonoisotopicMass {get; set;}
        public double   HighNormalizedScore { get; set; }

        public double        LogPeptideEValue { get; set; }
        public short         MultiProteinCount   { get; set; } 		
        public Sequence      SequenceData     { get; set; }
        public bool IsPredicted { get; set; }
        public static short  CalculateTrypticState(string peptide)
        {
            short trypticState = 0;
            char[] peptideChar = peptide.ToCharArray();
            int startIndex = 2;
            int stopIndex = peptideChar.Length - 3;

            if (peptideChar[1] != '.')
            {
                startIndex = 0;
                throw new ApplicationException("Peptide " + peptide + " does not have a . in the second position");
            }

            if (peptideChar[stopIndex + 1] != '.')
            {
                stopIndex = peptideChar.Length - 1;
                throw new ApplicationException("Peptide " + peptide + " does not have a . in the second last position");
            }

            if (peptideChar[stopIndex] == 'R' || peptideChar[stopIndex] == 'K')
            {
                trypticState++;
                if (peptideChar[peptideChar.Length - 1] == 'P')
                {
                    trypticState--;
                }
            }
            else if (!Char.IsLetter(peptideChar[stopIndex]))
            {
                if (peptideChar[stopIndex - 1] == 'R' || peptideChar[stopIndex - 1] == 'K')
                {
                    trypticState++;
                    if (peptideChar[peptideChar.Length - 1] == 'P')
                    {
                        trypticState--;
                    }
                }
            }

            if (peptideChar[peptideChar.Length - 1] == '-' && trypticState == 0)
            {
                trypticState++;
            }

            if (peptideChar[0] == 'R' || peptideChar[0] == 'K')
            {
                trypticState++;
                if (peptideChar[startIndex] == 'P')
                {
                    trypticState--;
                }
            }
            else if (peptideChar[0] == '-')
            {
                trypticState++;
            }

            return trypticState;
        }        
        public static string CleanPeptide(string peptide)
        {
            char[] peptideChar = peptide.ToCharArray();
            int startIndex = 2;
            int stopIndex = peptideChar.Length - 3;
            if (peptideChar[1] != '.')
            {
                startIndex = 0;
            }
            if (peptideChar[stopIndex + 1] != '.')
            {
                stopIndex = peptideChar.Length - 1;
            }

            int copyToIndex = startIndex;
            int copyFromIndex = startIndex;

            while (copyFromIndex <= stopIndex)
            {
                while (copyFromIndex <= stopIndex && (peptideChar[copyFromIndex] == '&' || peptideChar[copyFromIndex] == '*'))
                {
                    copyFromIndex++;
                }
                if (copyFromIndex > stopIndex)
                    break;
                peptideChar[copyToIndex++] = peptideChar[copyFromIndex++];
            }

            string cleanPeptide = new string(peptideChar, startIndex, copyToIndex - startIndex);
            return cleanPeptide;
        }
        /// <summary>
        /// Gets or sets the consensus target for this result.
        /// </summary>
        public ConsensusTarget ParentTarget { get; set; }
        /// <summary>
        /// Gets or sets the list of proteins this target maps onto.
        /// </summary>
        public List<Protein> Proteins { get; set; }
        public override string ToString()
        {
            return Sequence;
        }
    }
}
