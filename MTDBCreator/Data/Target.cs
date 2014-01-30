using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{

    public class Target
    {
        /// <summary>
        /// Holds onto the peptide string id for this.  Using a flyweight pattern here to minimize the 
        /// use of the same peptide string over and over foreach result.
        /// </summary>
        private int m_sequenceId;
        private int m_cleanSequenceId;


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
        
        public string Sequence
        {
            get
            {
                return StringCache.Cache.UniquePeptides.GetString(m_sequenceId);
            }
            set
            {
                m_sequenceId = StringCache.Cache.UniquePeptides.AddString(value);
            }
        }
        public string CleanSequence
        {
            get
            {
                return StringCache.Cache.UniqueCleanPeptides.GetString(m_cleanSequenceId);
            }
            set
            {
                m_cleanSequenceId = StringCache.Cache.UniqueCleanPeptides.AddString(value);
            }
        }
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
        public static string CleanPeptideOld(string peptide)
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
        /// Cleans the peptide string of PTM's and tryptic terminii characters.
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public static string CleanPeptide(string peptide)
        {                        
            int startIndex  = peptide.IndexOf('.');
            int stopIndex   = peptide.LastIndexOf('.');
                        
            if (startIndex < 0) startIndex = 0;
            if (stopIndex < 0)  stopIndex = peptide.Length - 1;

            string cleanPeptide = peptide.Substring(startIndex, stopIndex);
            StringBuilder builder = new StringBuilder();
            
            foreach(char c in cleanPeptide.ToCharArray())
            {
                if (AminoAcidLibrary.IsAminoAcid(c.ToString()))
                {
                    builder.Append(c);
                }
            }

            cleanPeptide        = builder.ToString();            
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
        /// <summary>
        /// Gets or sets the spectral probability.
        /// </summary>
        public double SpectralProbability { get; set; }
        public override string ToString()
        {
            return Sequence;
        }
                
        private static PeptideFlyweightMapper peptideMapper = new PeptideFlyweightMapper();

    }

    /// <summary>
    /// Flyweight mapper for holding only the minimal number of peptide strings in memory.
    /// </summary>
    public class PeptideFlyweightMapper
    {
        /// <summary>
        /// The static count of number of unique (overall) peptide strings 
        /// </summary>
        private int m_peptideCount;

        /// <summary>
        /// Maps the id to a peptide string
        /// </summary>
        private Dictionary<int, string> m_peptideMap;
        /// <summary>
        /// Lookup for all peptides
        /// </summary>
        private Dictionary<string, int> m_peptideLookup;

        public PeptideFlyweightMapper()
        {
            m_peptideCount = 0;
            m_peptideLookup = new Dictionary<string, int>();
            m_peptideMap = new Dictionary<int, string>();
        }

        /// <summary>
        /// Gets a peptide string from the dictionary of strings.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  string GetString(int id)
        {
            if (m_peptideMap.ContainsKey(id))
                return m_peptideMap[id];
            else
                return null;
        }
        /// <summary>
        /// Adds the peptide to the flyweight manager
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public int AddString(string peptide)
        {
            if (m_peptideLookup.ContainsKey(peptide))
            {
                return m_peptideLookup[peptide];
            }
            int key = m_peptideCount;

            m_peptideLookup.Add(peptide, key);
            m_peptideMap.Add(key, peptide);
            m_peptideCount++;

            return key;
        }
    }

    public class Trie<T>
    {

    }

    public class Node<T>
    {
        public T Value { get; set; }
        public bool IsTerminal { get { return Value == null; } }
    }
}
