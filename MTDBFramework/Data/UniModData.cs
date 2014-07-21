using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;

namespace MTDBFramework.Data
{
    /// <summary>
    /// Class to hold all needed data read from unimod.xml (http://www.unimod.org/downloads.html)
    /// </summary>
    public static class UniModData
    {
        /// <summary>
        /// Store the UNIMOD modification data - title, formula, etc.
        /// </summary>
        public class Modification
        {
            public string _title;
            public string _fullName;
            public double _monoMass;
            public double _avgMass;
            public string _composition;

            public class Symbols
            {
                public string symbol;
                public int number;

                public override string ToString()
                {
                    return symbol + number;
                }
            }

            public List<Symbols> _formula;

            /// <summary>
            /// Get the chemical formula of the modification
            /// </summary>
            public string Formula
            {
                get
                {
                    string formula = "";
                    foreach (var symbol in _formula)
                    {
                        formula += symbol;
                    }
                    return formula;
                }
                private set { }
            }

            /// <summary>
            /// 
            /// </summary>
            public Modification()
            {
                _formula = new List<Symbols>();
            }
        }

        /// <summary>
        /// Store the element data read from unimod.xml
        /// </summary>
        public class Element
        {
            public string _title;
            public string _fullName;
            public double _avgMass;
            public double _monoMass;

            /// <summary>
            /// Populate an element object with the appropriate data
            /// </summary>
            /// <param name="title">Unique symbol - includes isotope designation</param>
            /// <param name="name">name</param>
            /// <param name="avgMass">average mass</param>
            /// <param name="monoMass">monoisotopic mass</param>
            public Element(string title, string name, double avgMass, double monoMass)
            {
                _title = title;
                _fullName = name;
                _avgMass = avgMass;
                _monoMass = monoMass;
            }
        }

        /// <summary>
        /// Store the amino acid data from unimod.xml
        /// </summary>
        public class AminoAcid
        {
            public string _title;
            public string _shortName;
            public string _fullName;
            public double _monoMass;

            /// <summary>
            /// Populate an AminoAcid object with the appropriate date
            /// </summary>
            /// <param name="title">Unique designation of Amino Acid, usually a single letter</param>
            /// <param name="shortName">3 letter name</param>
            /// <param name="fullName">full name</param>
            /// <param name="monoMass">monoisotopic mass</param>
            public AminoAcid(string title, string shortName, string fullName, double monoMass)
            {
                _title = title;
                _shortName = shortName;
                _fullName = fullName;
                _monoMass = monoMass;
            }
        }

        /// <summary>
        /// All needed modification data from unimod.xml, indexed by name
        /// </summary>
        public static Dictionary<string, Modification> ModList;

        /// <summary>
        /// All of the element data stored in unimod.xml, indexed by title (H, 2H, O, 18O, etc);
        /// </summary>
        public static Dictionary<string, Element> Elements;

        /// <summary>
        /// All of the Amino Acid data stored in unimod.xml, indexed by the letter used
        /// </summary>
        public static Dictionary<char, AminoAcid> AminoAcids;
    }
}
