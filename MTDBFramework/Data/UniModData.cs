using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using MTDBFramework.IO;

namespace MTDBFramework.Data
{
    /// <summary>
    /// Class to hold all needed data read from unimod.xml (http://www.unimod.org/downloads.html)
    /// </summary>
    public static class UniModData
    {
        /// <summary>
        /// A basic class for storing an element of a chemical formula
        /// </summary>
        public class Symbol
        {
            public string symbol;
            public int number;

            public override string ToString()
            {
                return symbol + number;
            }
        }

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
            public int _recordId;

            public List<Symbol> _formula;

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
                _formula = new List<Symbol>();
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
            public List<Symbol> _formula;

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
            /// Populate an AminoAcid object with the appropriate date
            /// </summary>
            /// <param name="title">Unique designation of Amino Acid, usually a single letter</param>
            /// <param name="shortName">3 letter name</param>
            /// <param name="fullName">full name</param>
            /// <param name="monoMass">monoisotopic mass</param>
            public AminoAcid(string title, string shortName, string fullName, double monoMass, List<Symbol> formula)
            {
                _title = title;
                _shortName = shortName;
                _fullName = fullName;
                _monoMass = monoMass;
                _formula = formula;
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
        public static Dictionary<string, AminoAcid> AminoAcids;

        /// <summary>
        /// Initializer, called by the first time access to an item in UniModData
        /// </summary>
        static UniModData()
        {
            ModList = new Dictionary<string, Modification>();
            Elements = new Dictionary<string, Element>();
            AminoAcids = new Dictionary<string, AminoAcid>();

            // This is called the first time the static object is used, which should not be from UniModReader
            var reader = new UniModReader();
            reader.Read("unimod.xml");
        }
    }
}
