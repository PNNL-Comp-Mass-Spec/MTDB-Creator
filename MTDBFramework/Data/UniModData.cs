using System;
using System.Collections.Generic;
using MTDBFramework.IO;

namespace MTDBFramework.Data
{
    /// <summary>
    /// Class to hold all needed data read from unimod.xml (http://www.unimod.org/downloads.html)
    /// </summary>
    public static class UniModData
    {
		/// <summary>
		/// Class to facilitate working with chemical formulas
		/// </summary>
	    public class ChemFormula
	    {
		    private List<string> symbolOrder;
			private Dictionary<string, int> symbols; 

			private const string SymbolOrdering = "C,13C,H,2H,O,18O,N,15N,S";
			private string[] _symbolOrder;

		    public override string ToString()
		    {
			    string formula = "";
				// Handle the special ordering first.
			    foreach (var symbol in _symbolOrder)
			    {
				    if (symbols.ContainsKey(symbol))
				    {
					    formula += SymbolOutput(symbol);
				    }
			    }
				// Output all other elements and quantities
			    foreach (var symbol in symbolOrder)
			    {
				    if (!SymbolOrdering.Contains(symbol))
				    {
					    formula += SymbolOutput(symbol);
				    }
			    }
			    return formula;
		    }

			/// <summary>
			/// Handle special formatting of chemical symbols
			/// </summary>
			/// <param name="symbol"></param>
			/// <returns></returns>
			private string SymbolOutput(string symbol)
			{
				string output = "";
				if (Char.IsDigit(symbol[0]))
				{
					output += "(" + symbol + ")";
				}
				else
				{
					output += symbol;
				}
				output += symbols[symbol];
				return output;
			}

			/// <summary>
			/// Initialize the symbol list
			/// </summary>
			public ChemFormula()
			{
				symbolOrder = new List<string>();
				symbols = new Dictionary<string, int>();
				_symbolOrder = SymbolOrdering.Split(',');
			}

			/// <summary>
			/// Add an item to the chemical formula
			/// </summary>
			/// <param name="symbol"></param>
			/// <param name="number"></param>
			public void Add(string symbol, int number)
			{
				symbols.Add(symbol, number);
				symbolOrder.Add(symbol);
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

			public ChemFormula _formula;

            /// <summary>
            /// Populate a modification object with the appropriate data
            /// </summary>
            /// <param name="title">Modification title</param>
            /// <param name="fullName">Modification full name</param>
            /// <param name="monoMass">Modification monoisotopic mass</param>
            /// <param name="avgMass">Modification average mass</param>
            /// <param name="comp">Modifcation composition (UNIMOD style)</param>
            /// <param name="id">UNIMOD record id for modification</param>
            /// <param name="formula">Chemical formula of modification</param>
            public Modification(string title, string fullName, double monoMass, double avgMass, string comp, int id, ChemFormula formula)
            {
	            _title = title;
	            _fullName = fullName;
	            _monoMass = monoMass;
	            _avgMass = avgMass;
	            _composition = comp;
	            _recordId = id;
                _formula = formula;
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
			public double _avgMass;
			public ChemFormula _formula;

			/// <summary>
			/// Populate an AminoAcid object with the appropriate date
			/// </summary>
			/// <param name="title">Unique designation of Amino Acid, usually a single letter</param>
			/// <param name="shortName">3 letter name</param>
			/// <param name="fullName">full name</param>
			/// <param name="monoMass">monoisotopic mass</param>
            /// <param name="avgMass">average mass</param>
            /// <param name="formula">Chemical formula</param>
			public AminoAcid(string title, string shortName, string fullName, double monoMass, double avgMass, ChemFormula formula)
            {
                _title = title;
                _shortName = shortName;
                _fullName = fullName;
                _monoMass = monoMass;
	            _avgMass = avgMass;
                _formula = formula;
            }
        }

		/// <summary>
		/// Store the modification brick data from unimod.xml
		/// </summary>
		public class ModBrick
		{
			public string _title;
			public string _fullName;
			public double _monoMass;
			public double _avgMass;
			public ChemFormula _formula;

			/// <summary>
			/// Populate a mod brick object with the appropriate date
			/// </summary>
			/// <param name="title">Unique designation of mod brick, usually a single letter</param>
			/// <param name="fullName">full name</param>
			/// <param name="monoMass">monoisotopic mass</param>
			/// <param name="avgMass">average mass</param>
			/// <param name="formula">Chemical formula</param>
			public ModBrick(string title, string fullName, double monoMass, double avgMass, ChemFormula formula)
			{
				_title = title;
				_fullName = fullName;
				_monoMass = monoMass;
				_avgMass = avgMass;
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
		/// All of the Modification Brick data stored in unimod.xml, indexed by title
		/// </summary>
		public static Dictionary<string, ModBrick> ModBricks;

        /// <summary>
        /// Initializer, called by the first time access to an item in UniModData
        /// </summary>
        static UniModData()
        {
            ModList = new Dictionary<string, Modification>();
            Elements = new Dictionary<string, Element>();
            AminoAcids = new Dictionary<string, AminoAcid>();
			ModBricks = new Dictionary<string, ModBrick>();

            // This is called the first time the static object is used, which should not be from UniModReader
            var reader = new UniModReader();
            reader.Read("unimod.xml");
        }
    }
}
