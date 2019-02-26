using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

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
            private readonly List<string> symbolOrder;
            private readonly Dictionary<string, int> symbols;

            private const string SymbolOrdering = "C,13C,H,2H,O,18O,N,15N,S";
            private readonly string[] _symbolOrder;

            /// <summary>
            /// Overloaded string output
            /// </summary>
            /// <returns>Formatted chemical formula</returns>
            public override string ToString()
            {
                var formula = "";
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
                var output = "";
                if (symbols[symbol] != 0)
                {
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
                }
                return output;
            }

            /// <summary>
            /// Return the count of an element from the formula
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            // ReSharper disable once UnusedMember.Global
            public int GetElementCount(string element)
            {
                if (symbols.ContainsKey(element))
                {
                    return symbols[element];
                }
                return 0;
            }

            /// <summary>
            /// Return the count of an element (including isotopes) from the formula
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            // ReSharper disable once UnusedMember.Global
            public int GetElementCountWithIsotopes(string element)
            {
                var count = 0;
                foreach (var symbol in symbols)
                {
                    if (symbol.Key.EndsWith(element))
                    {
                        count += symbol.Value;
                    }
                }
                return count;
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
            /// <summary>
            /// UNIMOD title - short name of modification
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// Full name of modification
            /// </summary>
            public string FullName { get; }

            /// <summary>
            /// MonoIsotopic Mass
            /// </summary>
            public double MonoIsotopicMass { get; }

            /// <summary>
            /// Average Mass
            /// </summary>
            public double AverageMass { get; }

            /// <summary>
            /// UNIMOD Composition - Includes Bricks
            /// </summary>
            public string Composition { get; }

            /// <summary>
            /// UNIMOD record Id
            /// </summary>
            public int Id { get; }

            /// <summary>
            /// Chemical Formula
            /// </summary>
            public ChemFormula Formula { get; }

            /// <summary>
            /// Populate a modification object with the appropriate data
            /// </summary>
            /// <param name="title">Modification title</param>
            /// <param name="fullName">Modification full name</param>
            /// <param name="monoMass">Modification monoisotopic mass</param>
            /// <param name="avgMass">Modification average mass</param>
            /// <param name="comp">Modification composition (UNIMOD style)</param>
            /// <param name="id">UNIMOD record id for modification</param>
            /// <param name="formula">Chemical formula of modification</param>
            public Modification(string title, string fullName, double monoMass, double avgMass, string comp, int id, ChemFormula formula)
            {
                Title = title;
                FullName = fullName;
                MonoIsotopicMass = monoMass;
                AverageMass = avgMass;
                Composition = comp;
                Id = id;
                Formula = formula;
            }
        }

        /// <summary>
        /// Store the element data read from unimod.xml
        /// </summary>
        public class Element
        {
            /// <summary>
            /// UNIMOD title - chemical symbol, may have isotope prefix
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// Full name of element
            /// </summary>
            public string FullName { get; }

            /// <summary>
            /// MonoIsotopic Mass
            /// </summary>
            public double MonoIsotopicMass { get; }

            /// <summary>
            /// Average Mass
            /// </summary>
            public double AverageMass { get; }

            /// <summary>
            /// Populate an element object with the appropriate data
            /// </summary>
            /// <param name="title">Unique symbol - includes isotope designation</param>
            /// <param name="name">name</param>
            /// <param name="avgMass">average mass</param>
            /// <param name="monoMass">monoisotopic mass</param>
            public Element(string title, string name, double avgMass, double monoMass)
            {
                Title = title;
                FullName = name;
                AverageMass = avgMass;
                MonoIsotopicMass = monoMass;
            }
        }

        /// <summary>
        /// Store the amino acid data from unimod.xml
        /// </summary>
        public class AminoAcid
        {
            /// <summary>
            /// 1-letter abbreviation
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// 3-letter Abbreviation
            /// </summary>
            public string ShortName { get; }

            /// <summary>
            /// Full name of Amino Acid
            /// </summary>
            public string FullName { get; }

            /// <summary>
            /// MonoIsotopic Mass
            /// </summary>
            public double MonoIsotopicMass { get; }

            /// <summary>
            /// Average Mass
            /// </summary>
            public double AverageMass { get; }

            /// <summary>
            /// Chemical Formula
            /// </summary>
            public ChemFormula Formula { get; }

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
                Title = title;
                ShortName = shortName;
                FullName = fullName;
                MonoIsotopicMass = monoMass;
                AverageMass = avgMass;
                Formula = formula;
            }
        }

        /// <summary>
        /// Store the modification brick data from unimod.xml
        /// </summary>
        public class ModBrick
        {
            /// <summary>
            /// UNIMOD title - Brick name
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// Full name of modification brick
            /// </summary>
            public string FullName { get; }

            /// <summary>
            /// MonoIsotopic Mass
            /// </summary>
            public double MonoIsotopicMass { get; }

            /// <summary>
            /// Average Mass
            /// </summary>
            public double AverageMass { get; }

            /// <summary>
            /// Chemical Formula
            /// </summary>
            public ChemFormula Formula { get; }

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
                Title = title;
                FullName = fullName;
                MonoIsotopicMass = monoMass;
                AverageMass = avgMass;
                Formula = formula;
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
            PopulateData();
        }

        #region Unimod.xml parsing
        /// <summary>
        /// Read and parse a unimod format XML file from project resources
        /// </summary>
        public static void PopulateData()
        {
            var xSettings = new XmlReaderSettings { IgnoreWhitespace = true };
            var memStream = new MemoryStream();
            var streamWrite = new StreamWriter(memStream);

            // ReSharper disable once RedundantNameQualifier
            streamWrite.Write(MTDBFramework.Properties.Resources.unimod);
            streamWrite.Flush();

            memStream.Position = 0;
            var sr = new StreamReader(memStream);

            // Handle disposal of allocated object correctly
            using (var reader = XmlReader.Create(sr, xSettings))
            {
                // Guarantee a move to the root node
                reader.MoveToContent();
                // Consume the umod:unimod root tag
                // Throws exception if we are not at the "umod:unimod" tag.
                // This is a critical error; we want to stop processing for this file if we encounter this error
                reader.ReadStartElement("umod:unimod");
                // Read the next node - should be the first child node
                while (reader.ReadState == ReadState.Interactive)
                {
                    // Handle exiting out properly at EndElement tags
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        reader.Read();
                        continue;
                    }
                    // Handle each 1st level as a chunk
                    switch (reader.Name)
                    {
                        case "umod:elements":
                            // Schema requirements: one instance of this element
                            ReadElements(reader.ReadSubtree());
                            reader.Read(); // "umod:elements", if it exists, might not have child nodes; consume the remains
                            break;
                        case "umod:modifications":
                            // Schema requirements: zero to one instances of this element
                            ReadModifications(reader.ReadSubtree());
                            reader.Read(); // "umod:modifications", if it exists, might not have child nodes; consume the remains
                            break;
                        case "umod:amino_acids":
                            // Schema requirements: zero to one instances of this element
                            ReadAminoAcids(reader.ReadSubtree());
                            reader.Read(); // "umod:amino_acids", if it exists, might not have child nodes; consume the remains
                            break;
                        case "umod:mod_bricks":
                            // Schema requirements: zero to one instances of this element
                            ReadModBricks(reader.ReadSubtree());
                            reader.Read(); // "umod:mod_bricks", if it exists, might not have child nodes; consume the remains
                            break;
                        default:
                            // We are not reading anything out of the tag, so bypass it
                            reader.Skip();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handle the umod:elements element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:elements element</param>
        private static void ReadElements(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement("umod:elements");
            while (reader.Name == "umod:elem")
            {
                ReadElement(reader.ReadSubtree());

                // Consume the element we just processed.
                reader.Read();
            }
        }

        /// <summary>
        /// Handle a single umod:elem element
        /// Called by ReadElements (XML hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single umod:elem element</param>
        private static void ReadElement(XmlReader reader)
        {
            reader.MoveToContent();

            var title = reader.GetAttribute("title");
            var fullName = reader.GetAttribute("full_name");
            var monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            var avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

            var elem = new Element(title, fullName, monoMass, avgMass);
            Elements.Add(title, elem);

            // To advance to the next node
            reader.Close();
        }

        /// <summary>
        /// Handle a the umod:modifications element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:modifications element</param>
        private static void ReadModifications(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();

            while (reader.Name == "umod:mod")
            {
                ReadModification(reader.ReadSubtree());
                reader.ReadEndElement();
            }

            reader.Close();
        }

        /// <summary>
        /// Handle a single umod:mod element and child nodes
        /// Called by ReadModifications (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single umod:mod element</param>
        private static void ReadModification(XmlReader reader)
        {
            reader.MoveToContent();

            var title = reader.GetAttribute("title");
            var fullName = reader.GetAttribute("full_name");
            var recordId = Convert.ToInt32(reader.GetAttribute("record_id"));

            reader.ReadToDescendant("umod:delta");
            var monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            var avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));
            var composition = reader.GetAttribute("composition");

            var formula = ReadFormula(reader.ReadSubtree());

            var mod = new Modification(title, fullName, monoMass, avgMass, composition, recordId, formula);

            ModList.Add(title, mod);

            reader.Close();
        }

        /// <summary>
        /// Handle the umod:amino_acids element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:amino_acids element</param>
        private static void ReadAminoAcids(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement(); // digest the start element for the xml

            while (reader.Name == "umod:aa")
            {
                ReadAminoAcid(reader.ReadSubtree());
                // There might not be any child nodes, so we should do a Read() instead of a ReadEndElement().
                reader.Read();
            }
            reader.Close();
        }

        /// <summary>
        /// Handle a single umod:aa element and child nodes
        /// Called by ReadAminoAcids (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single umod:aa element</param>
        private static void ReadAminoAcid(XmlReader reader)
        {
            reader.MoveToContent(); // Move to the "aa" element

            var title = reader.GetAttribute("title");
            var shortName = reader.GetAttribute("three_letter");
            var fullName = reader.GetAttribute("full_name");
            var monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            var avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

            var formula = ReadFormula(reader.ReadSubtree());

            var amAcid = new AminoAcid(title, shortName, fullName, monoMass, avgMass, formula);
            AminoAcids.Add(title, amAcid);

            reader.Close();
        }

        /// <summary>
        /// Handle the umod:mod_bricks element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:mod_bricks element</param>
        private static void ReadModBricks(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement(); // digest the start element for the xml

            while (reader.Name == "umod:brick")
            {
                ReadModBrick(reader.ReadSubtree());
                // There might not be any child nodes, so we should do a Read() instead of a ReadEndElement().
                reader.Read();
            }
            reader.Close();
        }

        /// <summary>
        /// Handle a single umod:brick element and child nodes
        /// Called by ReadModBricks (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single umod:brick element</param>
        private static void ReadModBrick(XmlReader reader)
        {
            reader.MoveToContent(); // Move to the "aa" element

            var title = reader.GetAttribute("title");
            var fullName = reader.GetAttribute("full_name");
            var monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            var avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

            var formula = ReadFormula(reader.ReadSubtree());

            var brick = new ModBrick(title, fullName, monoMass, avgMass, formula);
            ModBricks.Add(title, brick);

            reader.Close();
        }

        /// <summary>
        /// Read the chemical formula of an item from unimod.xml
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope containing the chemical formula</param>
        /// <returns>List containing the chemical formula</returns>
        private static ChemFormula ReadFormula(XmlReader reader)
        {
            var formula = new ChemFormula();

            reader.MoveToContent();
            while (reader.ReadToFollowing("umod:element"))
            {
                var symbol = reader.GetAttribute("symbol");
                var number = Convert.ToInt32(reader.GetAttribute("number"));
                formula.Add(symbol, number);
            }

            return formula;
        }
        #endregion
    }
}
