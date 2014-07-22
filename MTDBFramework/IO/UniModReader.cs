using System;
using System.Collections.Generic;
using System.IO;
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
        private void ReadElements(XmlReader reader)
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
        /// Called by ReadElements (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single umod:elem element</param>
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
            // To advance to the next node
            
            reader.Close();
        }

        /// <summary>
        /// Handle a the umod:modifications element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:modifications element</param>
        private void ReadModifications(XmlReader reader)
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
        private void ReadModification(XmlReader reader)
        {
            reader.MoveToContent();

            string title;
            string fullName;
            double monoMass;
            double avgMass;
            string composition;
            int recordId;
	        List<UniModData.Symbol> formula;

	        title = reader.GetAttribute("title");
            fullName = reader.GetAttribute("full_name");
            recordId = Convert.ToInt32(reader.GetAttribute("record_id"));

            reader.ReadToDescendant("umod:delta");
            monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
            avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));
            composition = reader.GetAttribute("composition");

			formula = ReadFormula(reader.ReadSubtree());

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

        /// <summary>
        /// Handle the umod:amino_acids element and child nodes
        /// Called by Read (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the umod:amino_acids element</param>
        private void ReadAminoAcids(XmlReader reader)
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
        private void ReadAminoAcid(XmlReader reader)
        {
            reader.MoveToContent(); // Move to the "aa" element

            string title;
            string shortName;
            string fullName;
			double monoMass;
			double avgMass;
	        List<UniModData.Symbol> formula;

            title = reader.GetAttribute("title");
            shortName = reader.GetAttribute("three_letter");
            fullName = reader.GetAttribute("full_name");
			monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
			avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

	        formula = ReadFormula(reader.ReadSubtree());

            var amAcid = new UniModData.AminoAcid(title, shortName, fullName, monoMass, avgMass, formula);
            UniModData.AminoAcids.Add(title, amAcid);
            
            reader.Close();
		}

		/// <summary>
		/// Handle the umod:mod_bricks element and child nodes
		/// Called by Read (xml hierarchy)
		/// </summary>
		/// <param name="reader">XmlReader that is only valid for the scope of the umod:mod_bricks element</param>
		private void ReadModBricks(XmlReader reader)
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
		private void ReadModBrick(XmlReader reader)
		{
			reader.MoveToContent(); // Move to the "aa" element

			string title;
			string fullName;
			double monoMass;
			double avgMass;
			List<UniModData.Symbol> formula;

			title = reader.GetAttribute("title");
			fullName = reader.GetAttribute("full_name");
			monoMass = Convert.ToDouble(reader.GetAttribute("mono_mass"));
			avgMass = Convert.ToDouble(reader.GetAttribute("avge_mass"));

			formula = ReadFormula(reader.ReadSubtree());

			var brick = new UniModData.ModBrick(title, fullName, monoMass, avgMass, formula);
			UniModData.ModBricks.Add(title, brick);

			reader.Close();
		}

		/// <summary>
		/// Read the chemical formula of an item from unimod.xml
		/// </summary>
		/// <param name="reader">XmlReader that is only valid for the scope containing the chemical formula</param>
		/// <returns>List containing the chemical formula</returns>
		private List<UniModData.Symbol> ReadFormula(XmlReader reader)
		{
			var formula = new List<UniModData.Symbol>();

			reader.MoveToContent();
			while (reader.ReadToFollowing("umod:element"))
			{
				var component = new UniModData.Symbol();
				component.symbol = reader.GetAttribute("symbol");
				component.number = Convert.ToInt32(reader.GetAttribute("number"));
				formula.Add(component);
			}

			return formula;
		}
    }
}
