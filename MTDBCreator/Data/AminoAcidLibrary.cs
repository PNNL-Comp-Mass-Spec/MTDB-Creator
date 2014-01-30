using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public static class AminoAcidLibrary
    {
        static Dictionary<string, AminoAcid> m_aminoAcids;

        static AminoAcidLibrary()
        {
            m_aminoAcids = new Dictionary<string, AminoAcid>();

            AddAminoAcid("A", 0);
            AddAminoAcid("R", 0);
            AddAminoAcid("N", 0);
            AddAminoAcid("D", 0);
            AddAminoAcid("C", 0);
            AddAminoAcid("E", 0);
            AddAminoAcid("Q", 0);
            AddAminoAcid("G", 0);
            AddAminoAcid("H", 0);
            AddAminoAcid("I", 0);
            AddAminoAcid("L", 0);
            AddAminoAcid("K", 0);
            AddAminoAcid("M", 0);
            AddAminoAcid("F", 0);
            AddAminoAcid("P", 0);
            AddAminoAcid("S", 0);
            AddAminoAcid("T", 0);
            AddAminoAcid("W", 0);
            AddAminoAcid("Y", 0);
            AddAminoAcid("V", 0);            
        }

        private static void AddAminoAcid(string name, double mass)
        {
            m_aminoAcids.Add(name, new AminoAcid(name, mass));
        }

        /// <summary>
        /// Determines if the provided string, character is an amino acid letter.
        /// </summary>
        /// <param name="aminoAcid"></param>
        /// <returns></returns>
        public static bool IsAminoAcid(string aminoAcid)
        {
            if (aminoAcid.Length != 1)
                return false;

            return m_aminoAcids.ContainsKey(aminoAcid);
        }
    }

}
