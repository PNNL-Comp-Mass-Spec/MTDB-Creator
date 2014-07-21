using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MTDBFramework.Data;

namespace MTDBFramework.IO
{
    public class UniModReader
    {
        /// <summary>
        /// Read and parse a unimod format XML file
        /// </summary>
        /// <param name="path">System path of file to read in</param>
        public void Read(string path)
        {
            var xSettings = new XmlReaderSettings { IgnoreWhitespace = true };
            var sr = new StreamReader(path);

            // Handle disposal of allocated object correctly
            using (var reader = XmlReader.Create(sr, xSettings))
            {
                // Guarantee a move to the root node
                reader.MoveToContent();
                // Consume the MzIdentML root tag
                // Throws exception if we are not at the "MzIdentML" tag.
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
                            ReadElement(reader.ReadSubtree());
                            reader.ReadEndElement(); // "SequenceCollection", if it exists, must have child nodes
                            break;
                        case "umod:modifications":
                            // Schema requirements: zero to one instances of this element
                            ReadModification(reader.ReadSubtree());
                            reader.ReadEndElement(); // "SequenceCollection", if it exists, must have child nodes
                            break;
                        case "umod:amino_acids":
                            // Schema requirements: zero to one instances of this element
                            ReadAminoAcid(reader.ReadSubtree());
                            reader.ReadEndElement(); // "SequenceCollection", if it exists, must have child nodes
                            break;
                        case "umod:mod_bricks":
                            // Schema requirements: zero to one instances of this element
                            reader.Skip();
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
        /// Handle a single Modification element and child nodes
        /// Called by (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single Modification element</param>
        private void ReadElement(XmlReader reader)
        {
            reader.MoveToContent();

            string title;
            string fullName;
            double monoMass;
            double avgMass;

            title = reader.GetAttribute("title");
            fullName = reader.GetAttribute("full_name");
            monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

            var elem = new UniModData.Element(title, fullName, monoMass, avgMass);
            UniModData.Elements.Add(title, elem);

            reader.Close();
        }

        /// <summary>
        /// Handle a single Modification element and child nodes
        /// Called by (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single Modification element</param>
        private void ReadModification(XmlReader reader)
        {
            reader.MoveToContent();

            string title;
            string fullName;
            double monoMass;
            double avgMass;
            string composition;
            int recordId;
            var formula = new List<UniModData.Symbol>();

            title = reader.GetAttribute("title");
            fullName = reader.GetAttribute("full_name");
            recordId = Convert.ToInt32(reader.GetAttribute("record_id"));

            reader.ReadToDescendant("umod:delta");
            monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));
            composition = reader.GetAttribute("composition");

            // Read all child element tags
            while (reader.ReadToFollowing("umod:element"))
            {
                var component = new UniModData.Symbol();
                component.symbol = reader.GetAttribute("symbol");
                component.number = Convert.ToInt32(reader.GetAttribute("number"));
                formula.Add(component);
            }

            var mod = new UniModData.Modification();
            mod._title = title;
            mod._fullName = fullName;
            mod._monoMass = monoMass;
            mod._avgMass = avgMass;
            mod._composition = composition;
            mod._recordId = recordId;
            mod._formula = formula;

            UniModData.ModList.Add(title, mod);

            reader.Close();
        }

        private void ReadAminoAcid(XmlReader reader)
        {
            reader.MoveToContent(); // Move to the "aa" element
            string title;
            string shortName;
            string fullName;
            double monoMass;
            var formula = new List<UniModData.Symbol>();

            title = reader.GetAttribute("title");
            shortName = reader.GetAttribute("three_letter");
            fullName = reader.GetAttribute("full_name");
            monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));

            // Read all child element tags
            while (reader.ReadToFollowing("umod:element"))
            {
                var component = new UniModData.Symbol();
                component.symbol = reader.GetAttribute("symbol");
                component.number = Convert.ToInt32(reader.GetAttribute("number"));
                formula.Add(component);
            }

            var amAcid = new UniModData.AminoAcid(title, shortName, fullName, monoMass, formula);
            UniModData.AminoAcids.Add(title, amAcid);

            reader.Close();
        }
    }
}
