using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using PHRPReader;

namespace MTDBFramework.IO
{
    public class MzIdentMlReader : IPhrpReader
    {
        public Options ReaderOptions { get; set; }

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

            public DatabaseSequence DBSeq { get; set; }
        }

		// Entry point for MZIdentMLReaders
		// Accepts a string "Path" and returns an LCMSDataSet
		// 
		// XML Reader parses an MZIdentML file, storing Peptide data, such as sequence,
		// number, and type of modifications, as a PeptideRef, Database Information holds
		// the length of the peptide and the protein description, Peptide Evidence holds the pre,
		// post, start and end for the peptide for Tryptic End calculations. The element that holds
		// the most information is the Spectrum ID Item, which has the calculated mz, experimental mz,
		// charge state, MSGF raw score, Denovo score, MSGF SpecEValue, MSGF EValue, MSGF QValue,
		// MSGR PepQValue, Scan number as well as which peptide it is and which evidences it has from
		// the analysis run.
		//
		// After the XML Reader, it then goes through each Spectrum ID item and maps the appropriate values
		// to the appropriate variables as a MSGF+ result. If the result passes the filter for MSGF+, it
		// then adds the data for if there are modifications and adds the result to a running list of results.
		// When all the results are tabulated, it passes them through to the AnalysisHelper class to calculate
		// both the observed and the predicted NETs and then returns an LCMSDataSet of the results with the MZIdent tool
		
        public LcmsDataSet Read(string path)
        {
            var xSettings = new XmlReaderSettings {IgnoreWhitespace = true};
            var sr = new StreamReader(path);
            using (XmlReader reader = XmlReader.Create(sr, xSettings))
            {
                reader.Read();
                while (reader.Read())
                {
                    
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            string id;
                            if (reader.Name == "Peptide")
                            {
                                var pepRef = new PeptideRef();
                                id = reader.GetAttribute("id");
                                reader.ReadToDescendant("PeptideSequence");
                                reader.Read();
                                pepRef.Sequence = reader.Value; // record the peptide sequence
                                reader.Read();//Read twice to get it out of peptideSequence element
                                reader.Read();
                                // Read in all the modifications
                                while (reader.NodeType != XmlNodeType.EndElement ||
                                       reader.Name != "Peptide")
                                {
                                    if (reader.NodeType != XmlNodeType.EndElement && reader.Name == "Modification")
                                    {

                                        var mod = new Modification
                                        {
                                            Mass = Convert.ToDouble(reader.GetAttribute("monoisotopicMassDelta"))
                                        };
                                        var mods = new KeyValuePair<int, Modification>(Convert.ToInt32(reader.GetAttribute("location")),
                                                                                                                     mod);
                                        // Read down to get the name of the modification, then add the modification to the peptide reference
                                        reader.Read();

                                        mod.Tag = reader.GetAttribute("name");
                                        pepRef.ModsAdd(mods.Key, mods.Value);

                                    }
                                    reader.Read();
                                }
                                if (id != null)
                                {
                                    m_peptides.Add(id, pepRef);
                                }
                            }
                            else if (reader.Name == "DBSequence")
                            {
                                var dbSeq = new DatabaseSequence
                                {
                                    Length = Convert.ToInt32(reader.GetAttribute("length")),
                                    Accession = reader.GetAttribute("accession")
                                };
                                id = reader.GetAttribute("id");
                                reader.ReadToDescendant("cvParam");
                                dbSeq.ProteinDescription = reader.GetAttribute("value");//.Split(' ')[0];
                                if (id != null)
                                {
                                    m_database.Add(id, dbSeq);
                                }
                            }
                            else if (reader.Name == "PeptideEvidence")
                            {
                                var pepEvidence = new PeptideEvidence
                                {
                                    IsDecoy = Convert.ToBoolean(reader.GetAttribute("isDecoy")),
                                    Post = reader.GetAttribute("post"),
                                    Pre = reader.GetAttribute("pre"),
                                    End = Convert.ToInt32(reader.GetAttribute("end")),
                                    Start = Convert.ToInt32(reader.GetAttribute("start")),
                                    PeptideRef = m_peptides[reader.GetAttribute("peptide_ref")],
                                    DBSeq = m_database[reader.GetAttribute("dBSequence_ref")]
                                };
                                m_evidences.Add(reader.GetAttribute("id"), pepEvidence);
                            }
                            else if (reader.Name == "SpectrumIdentificationResult")
                            {
                                var specRes = new List<SpectrumIdItem>();

                                while (reader.NodeType != XmlNodeType.EndElement || reader.Name != "SpectrumIdentificationResult")
                                {
                                    if (reader.NodeType != XmlNodeType.EndElement &&
                                        reader.Name == "SpectrumIdentificationItem")
                                    {
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
                                        reader.ReadToDescendant("PeptideEvidenceRef");                                        
                                        do
                                        {                                                                                       
                                            specItem.PepEvidence.Add(m_evidences[reader.GetAttribute("peptideEvidence_ref")]);
                                            specItem.PepEvCount++;                                            
                                            reader.Read();
                                        }
                                        while (reader.Name == "PeptideEvidenceRef");
                                        specItem.RawScore = Convert.ToInt32(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.DeNovoScore = Convert.ToInt32(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.SpecEv = Convert.ToDouble(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.EValue = Convert.ToDouble(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.QValue = Convert.ToDouble(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.PepQValue = Convert.ToDouble(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("userParam");
                                        specItem.IsoError = Convert.ToInt32(reader.GetAttribute("value"));
                                        specRes.Add(specItem);
                                    }
                                    reader.Read();
                                    if (reader.Name == "cvParam")
                                    {
                                        foreach (var item in specRes)
                                        {
                                            item.ScanNum = Convert.ToInt32(reader.GetAttribute("value"));
                                            m_specItems.Add(item.SpecItemId, item);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            var results = new List<MsgfPlusResult>();
            var filter = new MsgfPlusTargetFilter(ReaderOptions);
            
            int i = 1;
			// Go through each Spectrum ID and map it to an MSGF+ result
            foreach (KeyValuePair<string, SpectrumIdItem> item in m_specItems)
            {
                var result = new MsgfPlusResult
                {
                    AnalysisId = i,
                    Charge = Convert.ToInt16(item.Value.Charge),
                    CleanPeptide = item.Value.Peptide.Sequence,
                    DeNovoScore = item.Value.DeNovoScore,
                    EValue = item.Value.EValue,
                    Fdr = item.Value.QValue,
                    IsotopeError = item.Value.IsoError,
                    MsgfScore = item.Value.RawScore,
                    MultiProteinCount = Convert.ToInt16(item.Value.PepEvCount),
                    MonoisotopicMass = clsPeptideMassCalculator.ConvoluteMass(item.Value.CalMz, item.Value.Charge, 0),
                    PepQValue = item.Value.PepQValue,
                    ObservedMonoisotopicMass =
                        clsPeptideMassCalculator.ConvoluteMass(item.Value.ExperimentalMz, item.Value.Charge, 0)
                };

                result.DelM = result.ObservedMonoisotopicMass - result.MonoisotopicMass;
                result.DelMPpm = clsPeptideMassCalculator.MassToPPM(result.DelM, result.ObservedMonoisotopicMass);

                result.Mz = clsPeptideMassCalculator.ConvoluteMass(result.ObservedMonoisotopicMass, 0, result.Charge);
                result.PrecursorMz = item.Value.ExperimentalMz;

                result.QValue = item.Value.QValue;
                result.Scan = item.Value.ScanNum;

                result.SpecEValue = item.Value.SpecEv;
                result.SpecProb = item.Value.SpecEv;
                result.ModificationCount = Convert.ToInt16(item.Value.Peptide.Mods.Count);

                var evidence = item.Value.PepEvidence[0];

                result.Sequence = evidence.Pre + "." + item.Value.Peptide.Sequence + "." + evidence.Post;
                result.NumTrypticEnds = MsgfPlusResult.CalculateTrypticState(result.Sequence);
                result.SeqWithNumericMods = null;

                result.Reference = evidence.DBSeq.Accession;

                if (item.Value.Peptide.Mods.Count != 0)
                {
                    int j = 0;
                    string numModSeq = evidence.Pre + ".";
                    foreach (KeyValuePair<int, Modification> mod in item.Value.Peptide.Mods)
                    {
                        if (mod.Value.Tag != "Carbamidomethyl")
                        {
                            for (; j < mod.Key; j++)
                            {
                                numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                            }
                            if (mod.Value.Mass > 0)
                            {
                                numModSeq += "+";
                            }
                            else
                            {
                                numModSeq += "-";
                            }
                            numModSeq = numModSeq + mod.Value.Mass;
                        }
                    }
                    for (; j < item.Value.Peptide.Sequence.Length; j++)
                    {
                        numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                    }
                    numModSeq = numModSeq + "." + evidence.Post;
                    result.SeqWithNumericMods = numModSeq;
                }
                else
                {
                    result.SeqWithNumericMods = result.Sequence;
                }

                result.PeptideInfo = new TargetPeptideInfo
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide,
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };
                i++;

                // If it passes the filter, it needs to be added to the final results
                if (!filter.ShouldFilter(result))
                {
                    result.DataSet = new TargetDataSet { Path = path };
                    result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;
                    result.ModificationDescription = null;

                    foreach (var thing in item.Value.PepEvidence)
                    {
                        var protein = new ProteinInformation
                        {
                            ProteinName = thing.DBSeq.Accession,
                            ResidueStart = thing.Start,
                            ResidueEnd = thing.End
                        };
                        if (evidence.Pre[0] == '-')
                        {
                            if (evidence.Post[0] == '-')
                            {
                                protein.TerminusState = clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinNandCCTerminus;
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
                        switch (result.NumTrypticEnds)
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
                            default:
                                //ERROR!!! Should never be more than 2 or less than 0 tryptic ends!\
                                break;
                        }
                        result.Proteins.Add(protein);
                    }

                    if (result.ModificationCount != 0)
                    {

                        foreach (KeyValuePair<int, Modification> mod in item.Value.Peptide.Mods)
                        {
                            if (mod.Value.Tag != "Carbamidomethyl")
                            {

                                result.SeqInfoMonoisotopicMass += mod.Value.Mass;
                            }

                            result.ModificationDescription += mod.Value.Tag + ":" + mod.Key + "  ";
                        }
                    }
                    results.Add(result);
                }
            }

            // Calculate NET for the results
            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType), results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MZIdentML, results);
        }
    }
}
