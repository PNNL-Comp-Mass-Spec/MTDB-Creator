using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Read and perform some processing on a MZIdentML file
    /// Processes the data into an LCMS DataSet
    /// </summary>
    public class MzIdentMlReader : PHRPReaderBase
    {
        /// <summary>
        /// Initialize a MzIdentMlReader object
        /// </summary>
        /// <param name="options">Options used for the MSGFPlus Target Filter</param>
        public MzIdentMlReader(Options options)
        {
            ReaderOptions = options;
        }

        private readonly Dictionary<string, DatabaseSequence> m_database = new Dictionary<string, DatabaseSequence>();
        private readonly Dictionary<string, PeptideRef> m_peptides = new Dictionary<string, PeptideRef>();
        private readonly Dictionary<string, PeptideEvidence> m_evidences = new Dictionary<string, PeptideEvidence>();
        private readonly Dictionary<string, SpectrumIdItem> m_specItems = new Dictionary<string, SpectrumIdItem>();

        private class SpectrumIdItem
        {
            #region Spectrum ID Public Properties

            public SpectrumIdItem()
            {
                PepEvidence = new List<PeptideEvidence>();
            }

            public string SpecItemId { get; set; }

            public bool PassThreshold { get; set; }

            public int Rank { get; set; }

            public PeptideRef Peptide { get; set; }

            public double CalMz { get; set; }

            public double ExperimentalMz { get; set; }

            public int Charge { get; set; }

            public List<PeptideEvidence> PepEvidence { get; private set; }

            public int PepEvCount { get; set; }

            public int RawScore { get; set; }

            public int DeNovoScore { get; set; }

            public double SpecEv { get; set; }

            public double EValue { get; set; }

            public double QValue { get; set; }

            public double PepQValue { get; set; }

            public int IsoError { get; set; }

            public int ScanNum { get; set; }

            #endregion

        }

        private class DatabaseSequence
        {
            public string Accession { get; set; }

            public int Length { get; set; }

            public string ProteinDescription { get; set; }
        }

        private class Modification
        {
            public double Mass { get; set; }

            public string Tag { get; set; }
        }

        private class PeptideRef
        {
            readonly Dictionary<int, Modification> m_mods;

            public PeptideRef()
            {
                m_mods = new Dictionary<int, Modification>();
            }

            public string Sequence { get; set; }

            public void ModsAdd(int location, Modification mod)
            {
                m_mods.Add(location, mod);
            }

            public Dictionary<int, Modification> Mods
            {
                get { return m_mods; }
            }
        }

        private class PeptideEvidence
        {
            public bool IsDecoy { get; set; }

            public string Post { get; set; }

            public string Pre { get; set; }

            public int End { get; set; }

            public int Start { get; set; }

            public PeptideRef PeptideRef { get; set; }

            public DatabaseSequence DbSeq { get; set; }
        }

        /// <summary>
        /// Entry point for MZIdentMLReader, overriden from PHRPReaderBase
        /// Read the MZIdentML file, map the data to MSGF+ data, compute the NETs, and return the LCMS DataSet
        /// </summary>
        /// <param name="path">Path to *.mzid/mzIdentML file</param>
        /// <returns>LCMSDataSet</returns>
        /// <remarks>
        /// XML Reader parses an MZIdentML file, storing data as follows:
        ///   PeptideRef holds Peptide data, such as sequence, number, and type of modifications
        ///   Database Information holds the length of the peptide and the protein description 
        ///   Peptide Evidence holds the pre, post, start and end for the peptide for Tryptic End calculations.
        /// The element that holds the most information is the Spectrum ID Item, which has the calculated mz,
        /// experimental mz, charge state, MSGF raw score, Denovo score, MSGF SpecEValue, MSGF EValue, 
        /// MSGF QValue, MSGR PepQValue, Scan number as well as which peptide it is and which evidences 
        /// it has from the analysis run.
        /// 
        /// After the XML Reader, it then goes through each Spectrum ID item and maps the appropriate values
        /// to the appropriate variables as a MSGF+ result. If the result passes the filter for MSGF+, it
        /// then adds the data for if there are modifications and adds the result to a running list of results.
        /// When all the results are tabulated, it passes them through to the AnalysisHelper class to calculate
        /// both the observed and the predicted NETs and then returns an LCMSDataSet of the results with the MZIdent tool
        /// </remarks>
        public override LcmsDataSet Read(string path)
        {
            var results = new List<MsgfPlusResult>();
			// Set a large buffer size. Doesn't affect gzip reading speed, but speeds up non-gzipped
	        Stream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536);

            if (path.EndsWith(".mzid.gz") || path.EndsWith(".zip"))
            {
                file = new GZipStream(file, CompressionMode.Decompress);
            }

            var xSettings = new XmlReaderSettings { IgnoreWhitespace = true };
            var reader = XmlReader.Create(new StreamReader(file, System.Text.Encoding.UTF8, true, 65536), xSettings);

            // Read in the file
            ReadMzIdentMl(reader);

            // Map to MSGF+ results
            MapToMsgf(results, path);

            // Calculate the Normal Elution Times
            ComputeNets(results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MZIdentML, results);
        }

        /// <summary>
        /// Read and parse a .mzid file, or mzIdentML
        /// Files are commonly larger than 30 MB, so use a streaming reader instead of a DOM reader
        /// </summary>
        /// <param name="reader">XmlReader object for the file to be read</param>
        private void ReadMzIdentMl(XmlReader reader)
        {
            // Handle disposal of allocated object correctly
            using (reader)
            {
                // Guarantee a move to the root node
                reader.MoveToContent();
                // Consume the MzIdentML root tag
                // Throws exception if we are not at the "MzIdentML" tag.
                // This is a critical error; we want to stop processing for this file if we encounter this error
                reader.ReadStartElement("MzIdentML");
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
                        case "cvList":
                            // Schema requirements: one instance of this element
                            reader.Skip();
                            break;
                        case "AnalysisSoftwareList":
                            // Schema requirements: zero to one instances of this element
                            reader.Skip();
                            break;
                        case "Provider":
                            // Schema requirements: zero to one instances of this element
                            reader.Skip();
                            break;
                        case "AuditCollection":
                            // Schema requirements: zero to one instances of this element
                            reader.Skip();
                            break;
                        case "AnalysisSampleCollection":
                            // Schema requirements: zero to one instances of this element
                            reader.Skip();
                            break;
                        case "SequenceCollection":
                            // Schema requirements: zero to one instances of this element
                            // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                            ReadSequenceCollection(reader.ReadSubtree());
                            reader.ReadEndElement(); // "SequenceCollection", if it exists, must have child nodes
                            break;
                        case "AnalysisCollection":
                            // Schema requirements: one instance of this element
                            reader.Skip();
                            break;
                        case "AnalysisProtocolCollection":
                            // Schema requirements: one instance of this element
                            reader.Skip();
                            break;
                        case "DataCollection":
                            // Schema requirements: one instance of this element
                            // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                            ReadDataCollection(reader.ReadSubtree());
                            reader.ReadEndElement(); // "DataCollection" must have child nodes
                            break;
                        case "BibliographicReference":
                            // Schema requirements: zero to many instances of this element
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
        /// Handle the child nodes of the SequenceCollection element
        /// Called by ReadMzIdentML (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single SequenceCollection element</param>
        private void ReadSequenceCollection(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement("SequenceCollection"); // Throws exception if we are not at the "SequenceCollection" tag.
            while (reader.ReadState == ReadState.Interactive)
            {
                // Handle exiting out properly at EndElement tags
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                    continue;
                }
                switch (reader.Name)
                {
                    case "DBSequence":
                        // Schema requirements: one to many instances of this element
                        // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                        ReadDbSequence(reader.ReadSubtree());
                        // "DBSequence" might not have any child nodes
                        // We will either consume the EndElement, or the same element that was passed to ReadDBSequence (in case of no child nodes)
                        reader.Read();
                        break;
                    case "Peptide":
                        // Schema requirements: zero to many instances of this element
                        // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                        ReadPeptide(reader.ReadSubtree());
                        // "Peptide" might not have any child nodes
                        // We will either consume the EndElement, or the same element that was passed to ReadPeptide (in case of no child nodes)
                        reader.Read();
                        break;
                    case "PeptideEvidence":
                        // Schema requirements: zero to many instances of this element
                        // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                        ReadPeptideEvidence(reader.ReadSubtree());
                        // "PeptideEvidence" might not have any child nodes; guarantee advance
                        // We will either consume the EndElement, or the same element that was passed to ReadPeptideEvidence (in case of no child nodes)
                        reader.Read();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Handle DBSequence element
        /// Called by ReadSequenceCollection (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single DBSequence element</param>
        private void ReadDbSequence(XmlReader reader)
        {
            reader.MoveToContent();
            string id = reader.GetAttribute("id");
            if (id != null)
            {
                var dbSeq = new DatabaseSequence
                {
                    Length = Convert.ToInt32(reader.GetAttribute("length")),
                    Accession = reader.GetAttribute("accession")
                };
                if (reader.ReadToDescendant("cvParam"))
                {
                    dbSeq.ProteinDescription = reader.GetAttribute("value"); //.Split(' ')[0];
                }
                m_database.Add(id, dbSeq);
            }
            reader.Close();
        }

        /// <summary>
        /// Handle Peptide element
        /// Called by ReadSequenceCollection (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single Peptide element</param>
        private void ReadPeptide(XmlReader reader)
        {
            reader.MoveToContent();
            string id = reader.GetAttribute("id");
            if (id != null)
            {
                var pepRef = new PeptideRef();
                reader.ReadToDescendant("PeptideSequence");
                pepRef.Sequence = reader.ReadElementContentAsString(); // record the peptide sequence, and consume the start and end elements
                // Read in all the modifications
                // If a modification exists, we are already on the start tag for it
                while (reader.Name == "Modification")
                {
                    var mod = new Modification
                    {
                        Mass = Convert.ToDouble(reader.GetAttribute("monoisotopicMassDelta"))
                    };
                    var mods = new KeyValuePair<int, Modification>(Convert.ToInt32(reader.GetAttribute("location")),
                                                                                                mod);
                    // Read down to get the name of the modification, then add the modification to the peptide reference
                    reader.ReadToDescendant("cvParam"); // The cvParam child node is required

                    mod.Tag = reader.GetAttribute("name");
                    pepRef.ModsAdd(mods.Key, mods.Value);

                    // There could theoretically exist more than one cvParam element. Clear them out.
                    while (reader.ReadToNextSibling("cvParam"))
                    {
                        // This is supposed to be empty. The loop condition does everything that needs to happen
                    }
                    reader.ReadEndElement(); // Consume EndElement for Modification
                }
                m_peptides.Add(id, pepRef);
            }
            reader.Close();
        }

        /// <summary>
        /// Handle PeptideEvidence element
        /// Called by ReadSequenceCollection (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single PeptideEvidence element</param>
        private void ReadPeptideEvidence(XmlReader reader)
        {
            reader.MoveToContent();
            var pepEvidence = new PeptideEvidence
            {
                IsDecoy = Convert.ToBoolean(reader.GetAttribute("isDecoy")),
                Post = reader.GetAttribute("post"),
                Pre = reader.GetAttribute("pre"),
                End = Convert.ToInt32(reader.GetAttribute("end")),
                Start = Convert.ToInt32(reader.GetAttribute("start")),
                PeptideRef = m_peptides[reader.GetAttribute("peptide_ref")],
                DbSeq = m_database[reader.GetAttribute("dBSequence_ref")]
            };
            m_evidences.Add(reader.GetAttribute("id"), pepEvidence);
            reader.Close();
        }

        /// <summary>
        /// Handle the child nodes of the DataCollection element
        /// Called by ReadMzIdentML (xml hierarchy)
        /// Currently we are only working with the AnalysisData child element
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single DataCollection element</param>
        private void ReadDataCollection(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement("DataCollection"); // Throws exception if we are not at the "DataCollection" tag.
            while (reader.ReadState == ReadState.Interactive)
            {
                // Handle exiting out properly at EndElement tags
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                    continue;
                }
                if (reader.Name == "AnalysisData")
                {
                    // Schema requirements: one and only one instance of this element
                    // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                    ReadAnalysisData(reader.ReadSubtree());
                    reader.ReadEndElement(); // "AnalysisData" must have child nodes
                }
                else
                {
                    reader.Skip();
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Handle child nodes of AnalysisData element
        /// Called by ReadDataCollection (xml hierarchy)
        /// Currently we are only working with the SpectrumIdentificationList child elements
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single AnalysisData element</param>
        private void ReadAnalysisData(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement("AnalysisData"); // Throws exception if we are not at the "AnalysisData" tag.
            while (reader.ReadState == ReadState.Interactive)
            {
                // Handle exiting out properly at EndElement tags
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                    continue;
                }
                if (reader.Name == "SpectrumIdentificationList")
                {
                    // Schema requirements: one to many instances of this element
                    // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                    ReadSpectrumIdentificationList(reader.ReadSubtree());
                    reader.ReadEndElement(); // "SpectrumIdentificationList" must have child nodes
                }
                else
                {
                    reader.Skip();
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Handle the child nodes of a SpectrumIdentificationList element
        /// Called by ReadAnalysisData (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single SpectrumIdentificationList element</param>
        private void ReadSpectrumIdentificationList(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement("SpectrumIdentificationList"); // Throws exception if we are not at the "SpectrumIdentificationList" tag.
            while (reader.ReadState == ReadState.Interactive)
            {
                // Handle exiting out properly at EndElement tags
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                    continue;
                }
                if (reader.Name == "SpectrumIdentificationResult")
                {
                    // Schema requirements: one to many instances of this element
                    // Use reader.ReadSubtree() to provide an XmlReader that is only valid for the element and child nodes
                    ReadSpectrumIdentificationResult(reader.ReadSubtree());
                    reader.ReadEndElement(); // "SpectrumIdentificationResult" must have child nodes
                }
                else
                {
                    reader.Skip();
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Handle a single SpectrumIdentificationResult element and child nodes
        /// Called by ReadSpectrumIdentificationList (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single SpectrumIdentificationResult element</param>
        private void ReadSpectrumIdentificationResult(XmlReader reader)
        {
            var specRes = new List<SpectrumIdItem>();

            reader.MoveToContent();
            reader.ReadStartElement("SpectrumIdentificationResult"); // Throws exception if we are not at the "SpectrumIdentificationResult" tag.
            while (reader.ReadState == ReadState.Interactive)
            {
                // Handle exiting out properly at EndElement tags
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                    continue;
                }

                switch (reader.Name)
                {
                    case "SpectrumIdentificationItem":
                        // Schema requirements: one to many instances of this element
                        ReadSpectrumIdentificationItem(reader.ReadSubtree(), specRes);
                        reader.ReadEndElement(); // "SpectrumIdentificationItem" must have child nodes
                        break;
                    case "cvParam":
                        // Schema requirements: zero to many instances of this element
                        int value = Convert.ToInt32(reader.GetAttribute("value"));
                        foreach (var item in specRes)
                        {
                            item.ScanNum = value;
                            m_specItems.Add(item.SpecItemId, item);
                        }
                        reader.Read(); // Consume the cvParam element (no child nodes)
                        break;
                    case "userParam":
                        // Schema requirements: zero to many instances of this element
                        reader.Skip();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Handle a single SpectrumIdentificationItem element and child nodes
        /// Called by ReadSpectrumIdentificationResult (xml hierarchy)
        /// </summary>
        /// <param name="reader">XmlReader that is only valid for the scope of the single SpectrumIdentificationItem element</param>
        /// <param name="specRes">List of SpectrumIdItems that the read data is stored in</param>
        private void ReadSpectrumIdentificationItem(XmlReader reader, List<SpectrumIdItem> specRes)
        {
            reader.MoveToContent(); // Move to the "SpectrumIdentificationItem" element
            var specItem = new SpectrumIdItem
            {
                PepEvCount = 0,
                SpecItemId = reader.GetAttribute("id"),
                PassThreshold = Convert.ToBoolean(reader.GetAttribute("passThreshold")),
                Rank = Convert.ToInt32(reader.GetAttribute("rank")),
                Peptide = m_peptides[reader.GetAttribute("peptide_ref")],
                CalMz = Convert.ToDouble(reader.GetAttribute("calculatedMassToCharge")),
                ExperimentalMz =
                    Convert.ToDouble(reader.GetAttribute("experimentalMassToCharge")),
                Charge = Convert.ToInt32(reader.GetAttribute("chargeState"))
            };

            // Read all child PeptideEvidenceRef tags
            reader.ReadToDescendant("PeptideEvidenceRef");
            while (reader.Name == "PeptideEvidenceRef")
            {
                specItem.PepEvidence.Add(m_evidences[reader.GetAttribute("peptideEvidence_ref")]);
                reader.Read();
            }

            // Parse all of the cvParam/userParam fields
            while (reader.Name == "cvParam" || reader.Name == "userParam")
            {
                switch (reader.GetAttribute("name"))
                {
                    case "MS-GF:RawScore":
                        specItem.RawScore = Convert.ToInt32(reader.GetAttribute("value"));
                        break;
                    case "MS-GF:DeNovoScore":
                        specItem.DeNovoScore = Convert.ToInt32(reader.GetAttribute("value"));
                        break;
                    case "MS-GF:SpecEValue":
                        specItem.SpecEv = Convert.ToDouble(reader.GetAttribute("value"));
                        break;
                    case "MS-GF:EValue":
                        specItem.EValue = Convert.ToDouble(reader.GetAttribute("value"));
                        break;
                    case "MS-GF:QValue":
                        specItem.QValue = Convert.ToDouble(reader.GetAttribute("value"));
                        break;
                    case "MS-GF:PepQValue":
                        specItem.PepQValue = Convert.ToDouble(reader.GetAttribute("value"));
                        break;
                    case "IsotopeError":
                        // userParam field
                        specItem.IsoError = Convert.ToInt32(reader.GetAttribute("value"));
                        break;
                    case "AssumedDissociationMethod":
                        // userParam field
                        break;
                }
                reader.Read();
            }
            specItem.PepEvCount = specItem.PepEvidence.Count;
            specRes.Add(specItem);

            reader.Close();
        }

        /// <summary>
        /// Map the results of a MZIdentML read to MSGF+
        /// </summary>
        /// <param name="results">Object to populate with the results of the Mapping</param>
        /// <param name="path">Path to MZIdentML file</param>
        private void MapToMsgf(List<MsgfPlusResult> results, string path)
        {
            var filter = new MsgfPlusTargetFilter(ReaderOptions);

            var cleavageStateCalculator = new clsPeptideCleavageStateCalculator();

            var i = 0;
            var total = m_specItems.Count;
            // Go through each Spectrum ID and map it to an MSGF+ result
            foreach (var item in m_specItems)
            {
                i++;
                if (i % 500 == 0)
                {
                    UpdateProgress((100 * ((float)i / total)));
                }
                // Skip this PSM if it doesn't pass the import filters
                // Note that qValue is basically FDR
                double qValue = item.Value.QValue;

                double specProb = item.Value.SpecEv;

                if (filter.ShouldFilter(qValue, specProb))
                    continue;

                if (item.Value.PepEvidence.Count == 0)
                    continue;

                var evidence = item.Value.PepEvidence[0];

                var result = new MsgfPlusResult
                {
                    AnalysisId = i,
                    Charge = Convert.ToInt16(item.Value.Charge),
                    CleanPeptide = item.Value.Peptide.Sequence,
                    SeqWithNumericMods = null,
                    MonoisotopicMass = clsPeptideMassCalculator.ConvoluteMass(item.Value.CalMz, item.Value.Charge, 0),
                    ObservedMonoisotopicMass = clsPeptideMassCalculator.ConvoluteMass(item.Value.ExperimentalMz, item.Value.Charge, 0),
                    MultiProteinCount = Convert.ToInt16(item.Value.PepEvCount),
                    Scan = item.Value.ScanNum,
                    Sequence = evidence.Pre + "." + item.Value.Peptide.Sequence + "." + evidence.Post,
                    Mz = 0,
                    SpecProb = specProb,
                    DelM = 0,
                    ModificationCount = Convert.ToInt16(item.Value.Peptide.Mods.Count)
                };

                // Populate some mass related items
                result.DelM = result.ObservedMonoisotopicMass - result.MonoisotopicMass;
                result.DelMPpm = clsPeptideMassCalculator.MassToPPM(result.DelM, result.ObservedMonoisotopicMass);
                // We could compute m/z:
                //     Mz = clsPeptideMassCalculator.ConvoluteMass(result.ObservedMonoisotopicMass, 0, result.Charge);                
                // But it's stored in the mzid file, so we'll use that
                result.Mz = item.Value.ExperimentalMz;

                StoreDatasetInfo(result, path);

                result.DataSet.Tool = LcmsIdentificationTool.MZIdentML;

                // Populate items specific to the MSGF+ results (stored as mzid)

                result.Reference = evidence.DbSeq.Accession;

                var eCleavageState = cleavageStateCalculator.ComputeCleavageState(item.Value.Peptide.Sequence, evidence.Pre, evidence.Post);
                result.NumTrypticEnds = clsPeptideCleavageStateCalculator.CleavageStateToShort(eCleavageState);

                result.DeNovoScore = item.Value.DeNovoScore;
                result.MsgfScore = item.Value.RawScore;
                result.SpecEValue = item.Value.SpecEv;
                result.RankSpecEValue = item.Value.Rank;

                result.EValue = item.Value.EValue;
                result.QValue = qValue;
                result.DiscriminantValue = qValue;
                result.PepQValue = item.Value.PepQValue;

                result.IsotopeError = item.Value.IsoError;

                if (result.ModificationCount > 0)
                {
                    var j = 0;
                    
                    var numModSeq = evidence.Pre + ".";
                    var encodedSeq = numModSeq;
                    foreach (var mod in item.Value.Peptide.Mods)
                    {
                        var ptm = new PostTranslationalModification
                        {
                            Location = mod.Key,
                            Mass = mod.Value.Mass,
                            Formula = UniModData.ModList[mod.Value.Tag].Formula.ToString(),
                            Name = UniModData.ModList[mod.Value.Tag].Title
                        };
                        result.Ptms.Add(ptm);

                        for (; j < ptm.Location; j++)
                        {
                            numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                            encodedSeq = encodedSeq + item.Value.Peptide.Sequence[j];
                        }

                        numModSeq += (ptm.Mass > 0) ? "+" : "-";
                        numModSeq = numModSeq + ptm.Mass;

                        encodedSeq += "[" + ((ptm.Mass > 0)? "+":"-") + ptm.Formula + "]";
                    }
                    for (; j < item.Value.Peptide.Sequence.Length; j++)
                    {
                        numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                        encodedSeq += item.Value.Peptide.Sequence[j];
                    }
                    numModSeq = numModSeq + "." + evidence.Post;
                    encodedSeq += "." + evidence.Post;
                    result.SeqWithNumericMods = numModSeq;
                    result.EncodedNonNumericSequence = encodedSeq;                    
                }
                else
                {
                    result.SeqWithNumericMods = result.Sequence;
                    result.EncodedNonNumericSequence = result.Sequence;
                }

                result.PeptideInfo = new TargetPeptideInfo
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide,
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };


                result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;
                result.ModificationDescription = null;

                foreach (var thing in item.Value.PepEvidence)
                {
                    var protein = new ProteinInformation
                    {
                        ProteinName = thing.DbSeq.Accession,
                        ResidueStart = thing.Start,
                        ResidueEnd = thing.End
                    };
                    ComputeTerminusState(evidence, result.NumTrypticEnds, protein);
                    result.Proteins.Add(protein);
                }

                if (result.ModificationCount > 0)
                {

                    foreach (var mod in item.Value.Peptide.Mods)
                    {
                        // TODO: Confirm that this is valid math (MEM thinks it may not be)
                        result.SeqInfoMonoisotopicMass += mod.Value.Mass;

                        result.ModificationDescription += mod.Value.Tag + ":" + mod.Key + "  ";
                    }
                }

                results.Add(result);

            }
        }


        private void ComputeTerminusState(PeptideEvidence evidence, short numTrypticEnds, ProteinInformation protein)
        {
            if (evidence.Pre[0] == '-')
            {
                if (evidence.Post[0] == '-')
                {
                    protein.TerminusState =
                        clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinNandCCTerminus;
                    protein.CleavageState = clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.Full;
                }
                else
                {
                    protein.TerminusState = clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinNTerminus;
                }
            }
            else if (evidence.Post[0] == '-')
            {
                protein.TerminusState = clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinCTerminus;
            }
            else
            {
                protein.TerminusState = clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.None;
            }

            switch (numTrypticEnds)
            {
                case 0:
                    protein.CleavageState = clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.NonSpecific;
                    break;
                case 1:
                    protein.CleavageState =
                        clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.Partial;
                    break;
                case 2:
                    protein.CleavageState = clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.Full;
                    break;
            }
        }
    }
}
