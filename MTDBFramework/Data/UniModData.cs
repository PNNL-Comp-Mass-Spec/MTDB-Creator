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

			/// <summary>
			/// Overloaded string output
			/// </summary>
			/// <returns>Formatted chemical formula</returns>
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
				if (symbols[symbol] != 1)
				{
					output += symbols[symbol];
				}
				return output;
			}

			/// <summary>
			/// Return the count of an element from the formula
			/// </summary>
			/// <param name="element"></param>
			/// <returns></returns>
			public int GetElementCount(string element)
			{
				if (symbols.ContainsKey(element))
				{
					return symbols[element];
				}
				return 0;
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
            private string _title;
            private string _fullName;
            private double _monoMass;
            private double _avgMass;
            private string _composition;
            private int _recordId;
			private ChemFormula _formula;

			/// <summary>
			/// UNIMOD title - short name of modification
			/// </summary>
	        public string Title
	        {
				get { return _title; }
				private set { _title = value; }
	        }

			/// <summary>
			/// Full name of modification
			/// </summary>
			public string FullName
			{
				get { return _fullName; }
				private set { _fullName = value; }
			}

			/// <summary>
			/// MonoIsotopic Mass
			/// </summary>
			public double MonoIsotopicMass
			{
				get { return _monoMass; }
				private set { _monoMass = value; }
			}

			/// <summary>
			/// Average Mass
			/// </summary>
			public double AverageMass
			{
				get { return _avgMass; }
				private set { _avgMass = value; }
			}

			/// <summary>
			/// UNIMOD Composition - Includes Bricks
			/// </summary>
			public string Composition
			{
				get { return _composition; }
				private set { _composition = value; }
			}

			/// <summary>
			/// UNIMOD record Id
			/// </summary>
			public int Id
			{
				get { return _recordId; }
				private set { _recordId = value; }
			}

			/// <summary>
			/// Chemical Formula
			/// </summary>
			public ChemFormula Formula
			{
				get { return _formula; }
				private set { _formula = value; }
			}

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
            private string _title;
            private string _fullName;
            private double _avgMass;
            private double _monoMass;

			/// <summary>
			/// UNIMOD title - chemical symbol, may have isotope prefix
			/// </summary>
			public string Title
			{
				get { return _title; }
				private set { _title = value; }
			}

			/// <summary>
			/// Full name of element
			/// </summary>
			public string FullName
			{
				get { return _fullName; }
				private set { _fullName = value; }
			}

			/// <summary>
			/// MonoIsotopic Mass
			/// </summary>
			public double MonoIsotopicMass
			{
				get { return _monoMass; }
				private set { _monoMass = value; }
			}

			/// <summary>
			/// Average Mass
			/// </summary>
			public double AverageMass
			{
				get { return _avgMass; }
				private set { _avgMass = value; }
			}

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
            private string _title;
            private string _shortName;
            private string _fullName;
			private double _monoMass;
			private double _avgMass;
			private ChemFormula _formula;

			/// <summary>
			/// 1-letter abbreviation
			/// </summary>
			public string Title
			{
				get { return _title; }
				private set { _title = value; }
			}

			/// <summary>
			/// 3-letter Abbreviation
			/// </summary>
			public string ShortName
			{
				get { return _shortName; }
				private set { _shortName = value; }
			}

			/// <summary>
			/// Full name of Amino Acid
			/// </summary>
			public string FullName
			{
				get { return _fullName; }
				private set { _fullName = value; }
			}

			/// <summary>
			/// MonoIsotopic Mass
			/// </summary>
			public double MonoIsotopicMass
			{
				get { return _monoMass; }
				private set { _monoMass = value; }
			}

			/// <summary>
			/// Average Mass
			/// </summary>
			public double AverageMass
			{
				get { return _avgMass; }
				private set { _avgMass = value; }
			}

			/// <summary>
			/// Chemical Formula
			/// </summary>
			public ChemFormula Formula
			{
				get { return _formula; }
				private set { _formula = value; }
			}

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
			private string _title;
			private string _fullName;
			private double _monoMass;
			private double _avgMass;
			private ChemFormula _formula;

			/// <summary>
			/// UNIMOD title - Brick name
			/// </summary>
			public string Title
			{
				get { return _title; }
				private set { _title = value; }
			}

			/// <summary>
			/// Full name of modification brick
			/// </summary>
			public string FullName
			{
				get { return _fullName; }
				private set { _fullName = value; }
			}

			/// <summary>
			/// MonoIsotopic Mass
			/// </summary>
			public double MonoIsotopicMass
			{
				get { return _monoMass; }
				private set { _monoMass = value; }
			}

			/// <summary>
			/// Average Mass
			/// </summary>
			public double AverageMass
			{
				get { return _avgMass; }
				private set { _avgMass = value; }
			}

			/// <summary>
			/// Chemical Formula
			/// </summary>
			public ChemFormula Formula
			{
				get { return _formula; }
				private set { _formula = value; }
			}

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
