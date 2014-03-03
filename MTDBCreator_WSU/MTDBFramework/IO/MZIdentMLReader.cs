using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;

namespace MTDBFramework.IO
{
    public class MZIdentMLReader : IPHRPReader
    {
        public Options ReaderOptions { get; set; }

        public MZIdentMLReader(Options options)
        {
            this.ReaderOptions = options;
        }

        private static Dictionary<string, DatabaseSequence> database = new Dictionary<string, DatabaseSequence>();
        private static Dictionary<string, PeptideRef> peptides = new Dictionary<string, PeptideRef>();
        private static Dictionary<string, PeptideEvidence> evidences = new Dictionary<string, PeptideEvidence>();
        private static Dictionary<string, SpectrumIDItem> specItems = new Dictionary<string, SpectrumIDItem>();

        private class SpectrumIDItem
        {
            string m_specItemID;
            bool m_passThreshold;
            int m_rank;
            PeptideRef m_peptide;
            double m_calMZ;
            double m_experimentalMZ;
            int m_charge;
            PeptideEvidence m_pepEvidence;
            int m_rawScore;
            int m_deNovoScore;
            double m_specEV;
            double m_EValue;
            double m_QValue;
            double m_pepQValue;
            int m_isoError;
            int m_scanNum;

            public string SpecItemID
            {
                get { return m_specItemID; }
                set { m_specItemID = value; }
            }
            public bool PassThreshold
            {
                get { return m_passThreshold; }
                set { m_passThreshold = value; }
            }
            public int Rank
            {
                get { return m_rank; }
                set { m_rank = value; }
            }
            public PeptideRef Peptide
            {
                get { return m_peptide; }
                set { m_peptide = value; }
            }
            public double CalMZ
            {
                get { return m_calMZ; }
                set { m_calMZ = value; }
            }
            public double ExperimentalMZ
            {
                get { return m_experimentalMZ; }
                set { m_experimentalMZ = value; }
            }
            public int Charge
            {
                get { return m_charge; }
                set { m_charge = value; }
            }
            public PeptideEvidence PepEvidence
            {
                get { return m_pepEvidence; }
                set { m_pepEvidence = value; }
            }
            public int RawScore
            {
                get { return m_rawScore; }
                set { m_rawScore = value; }
            }
            public int DeNovoScore
            {
                get { return m_deNovoScore; }
                set { m_deNovoScore = value; }
            }
            public double SpecEV
            {
                get { return m_specEV; }
                set { m_specEV = value; }
            }
            public double EValue
            {
                get { return m_EValue; }
                set { m_EValue = value; }
            }
            public double QValue
            {
                get { return m_QValue; }
                set { m_QValue = value; }
            }
            public double PepQValue
            {
                get { return m_pepQValue; }
                set { m_pepQValue = value; }
            }
            public int IsoError
            {
                get { return m_isoError; }
                set { m_isoError = value; }
            }
            public int ScanNum
            {
                get { return m_scanNum; }
                set { m_scanNum = value; }
            }
        }

        private class SpectrumIDResult
        {
            int m_index;
            public List<SpectrumIDItem> items;

            public SpectrumIDResult()
            {
                items = new List<SpectrumIDItem>();
            }
        }

        private class DatabaseSequence
        {
            string m_accession;
            int m_length;
            string m_proteinDescription;

            public DatabaseSequence()
            {
            }

            public string Accession
            {
                get { return m_accession; }
                set { m_accession = value; }
            }
            public int Length
            {
                get { return m_length; }
                set { m_length = value; }
            }
            public string ProteinDescription
            {
                get { return m_proteinDescription; }
                set { m_proteinDescription = value; }
            }
        }

        private class Modification
        {
            double m_mass;
            string m_tag;

            public double Mass
            {
                get { return m_mass; }
                set { m_mass = value; }
            }
            public string Tag
            {
                get { return m_tag; }
                set { m_tag = value; }
            }
        }

        private class PeptideRef
        {
            string m_sequence;
            Dictionary<int, Modification> m_mods;

            public PeptideRef()
            {
                m_mods = new Dictionary<int, Modification>();
            }
            public string Sequence
            {
                get { return m_sequence; }
                set { m_sequence = value; }
            }

            public void modsAdd(int location, Modification mod)
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
            bool m_isDecoy;
            string m_post;
            string m_pre;
            int m_end;
            int m_start;
            PeptideRef m_peptide;
            DatabaseSequence m_dBSeq;

            public PeptideEvidence()
            {
            }

            public bool IsDecoy
            {
                get { return m_isDecoy; }
                set { m_isDecoy = value; }
            }
            public string Post
            {
                get { return m_post; }
                set { m_post = value; }
            }
            public string Pre
            {
                get { return m_pre; }
                set { m_pre = value; }
            }
            public int End
            {
                get { return m_end; }
                set { m_end = value; }
            }
            public int Start
            {
                get { return m_start; }
                set { m_start = value; }
            }
            public PeptideRef PeptideRef
            {
                get { return m_peptide; }
                set { m_peptide = value; }
            }
            public DatabaseSequence DBSeq
            {
                get { return m_dBSeq; }
                set { m_dBSeq = value; }
            }
        }

        public LcmsDataSet Read(string path)
        {
            XmlReaderSettings XSettings = new XmlReaderSettings();
            XSettings.IgnoreWhitespace = true;
            string id;
            DatabaseSequence dbSeq;
            PeptideRef pepRef;
            PeptideEvidence pepEvidence;
            SpectrumIDItem specItem;
            StreamReader sr = new StreamReader(path);
            using (XmlReader reader = XmlReader.Create(sr, XSettings))
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Peptide")
                            {
                                pepRef = new PeptideRef();
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
                                        if (reader.GetAttribute("name") != "Carbamidomethyl")
                                        {
                                            Modification mod = new Modification();
                                            mod.Mass = Convert.ToDouble(reader.GetAttribute("monoisotopicMassDelta"));
                                            KeyValuePair<int, Modification> Mods = new KeyValuePair<int,Modification>(Convert.ToInt32(reader.GetAttribute("location")),  
                                                                                                                         mod);
                                            reader.Read();
                                            if (reader.GetAttribute("name") != "Carbamidomethyl")
                                            {
                                                mod.Tag = reader.GetAttribute("name");
                                                pepRef.modsAdd(Mods.Key, Mods.Value);
                                            }
                                        }
                                    }
                                    reader.Read();
                                }
                                peptides.Add(id, pepRef);
                            }
                            else if (reader.Name == "DBSequence")
                            {
                                dbSeq = new DatabaseSequence();
                                dbSeq.Length = Convert.ToInt32(reader.GetAttribute("length"));
                                dbSeq.Accession = reader.GetAttribute("accession");
                                id = reader.GetAttribute("id");
                                reader.ReadToDescendant("cvParam");
                                dbSeq.ProteinDescription = reader.GetAttribute("value");
                                database.Add(id, dbSeq);
                            }
                            else if (reader.Name == "PeptideEvidence")
                            {
                                pepEvidence = new PeptideEvidence();
                                pepEvidence.IsDecoy = Convert.ToBoolean(reader.GetAttribute("isDecoy"));
                                pepEvidence.Post = reader.GetAttribute("post");
                                pepEvidence.Pre = reader.GetAttribute("pre");
                                pepEvidence.End = Convert.ToInt32(reader.GetAttribute("end"));
                                pepEvidence.Start = Convert.ToInt32(reader.GetAttribute("start"));
                                pepEvidence.PeptideRef = peptides[reader.GetAttribute("peptide_ref")];
                                pepEvidence.DBSeq = database[reader.GetAttribute("dBSequence_ref")];
                                evidences.Add(reader.GetAttribute("id"), pepEvidence);
                            }
                            else if (reader.Name == "SpectrumIdentificationResult")
                            {
                                List<SpectrumIDItem> specRes = new List<SpectrumIDItem>();

                                while (reader.NodeType != XmlNodeType.EndElement || reader.Name != "SpectrumIdentificationResult")
                                {
                                    if (reader.NodeType != XmlNodeType.EndElement &&
                                        reader.Name == "SpectrumIdentificationItem")
                                    {
                                        specItem = new SpectrumIDItem();
                                        specItem.SpecItemID = reader.GetAttribute("id");
                                        specItem.PassThreshold = Convert.ToBoolean(reader.GetAttribute("passThreshold"));
                                        specItem.Rank = Convert.ToInt32(reader.GetAttribute("rank"));
                                        specItem.Peptide = peptides[reader.GetAttribute("peptide_ref")];
                                        specItem.CalMZ = Convert.ToDouble(reader.GetAttribute("calculatedMassToCharge"));
                                        specItem.ExperimentalMZ = Convert.ToDouble(reader.GetAttribute("experimentalMassToCharge"));
                                        specItem.Charge = Convert.ToInt32(reader.GetAttribute("chargeState"));
                                        reader.ReadToDescendant("PeptideEvidenceRef");
                                        specItem.PepEvidence = evidences[reader.GetAttribute("peptideEvidence_ref")];
                                        reader.ReadToFollowing("cvParam");
                                        specItem.RawScore = Convert.ToInt32(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.DeNovoScore = Convert.ToInt32(reader.GetAttribute("value"));
                                        reader.ReadToFollowing("cvParam");
                                        specItem.SpecEV = Convert.ToDouble(reader.GetAttribute("value"));
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
                                        foreach (SpectrumIDItem item in specRes)
                                        {
                                            item.ScanNum = Convert.ToInt32(reader.GetAttribute("value"));
                                            specItems.Add(item.SpecItemID, item);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            List<MSGFPlusResult> results = new List<MSGFPlusResult>();
            MSGFPlusTargetFilter filter = new MSGFPlusTargetFilter(this.ReaderOptions);
            MSGFPlusResult result = new MSGFPlusResult();
            int i = 1;
            foreach(KeyValuePair<string, SpectrumIDItem> item in specItems)
            {
                result.AnalysisId = i;
                result.Charge = Convert.ToInt16(item.Value.Charge);
                result.CleanPeptide = item.Value.Peptide.Sequence;
                result.DeNovoScore = item.Value.DeNovoScore;
                result.EValue = item.Value.EValue;
                result.Fdr = item.Value.QValue; //Is FDR the same as the QValue?
                result.IsotopeError = item.Value.IsoError; // Not used anywhere

                result.MSGFScore = item.Value.RawScore;
                //result.MultiProteinCount = item.Value.PepEvidence

                result.MonoisotopicMass = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(item.Value.CalMZ, item.Value.Charge, 0);
                result.PepQValue = item.Value.PepQValue;
                result.ObservedMonoisotopicMass = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(item.Value.ExperimentalMZ, item.Value.Charge, 0);

                result.DelM = result.ObservedMonoisotopicMass - result.MonoisotopicMass;
                result.DelM_PPM = PHRPReader.clsPeptideMassCalculator.MassToPPM(result.DelM, result.ObservedMonoisotopicMass);

                result.Mz = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(result.ObservedMonoisotopicMass, 0, result.Charge);
                result.PrecursorMZ = item.Value.ExperimentalMZ;
                
                result.QValue = item.Value.QValue;
                //result.RankSpecEValue = 1
                result.Scan = item.Value.ScanNum;
                result.Mz = PHRPReader.clsPeptideMassCalculator.ConvoluteMass(result.PrecursorMonoMass, result.Charge, 0);
                result.Sequence = item.Value.PepEvidence.Pre + "." + item.Value.Peptide.Sequence + "." + item.Value.PepEvidence.Post;
                result.NumTrypticEnds = MSGFPlusResult.CalculateTrypticState(result.Sequence);
                result.SeqWithNumericMods = null;
                if(item.Value.Peptide.Mods.Count != 0)
                {
                    string numModSeqPre = null;
                    string numModSeq = null;
                    foreach(KeyValuePair<int, Modification> mod in item.Value.Peptide.Mods)
                    {
                        for (int j = 0; j < mod.Key; j++)
                        {
                            numModSeqPre = numModSeqPre + item.Value.Peptide.Sequence[j];
                        }
                        numModSeq = numModSeqPre + mod.Value.Mass;
                        for (int j = mod.Key; j < item.Value.Peptide.Sequence.Length; j++)
                        {
                            numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                        }  
                    }
                    result.SeqWithNumericMods = numModSeq;
                }
 
                result.SpecEValue = item.Value.SpecEV;
                //result.SpectralProbability =
                //result.Spectrum =

                result.PeptideInfo = new TargetPeptideInfo()
                {
                    Peptide = result.Sequence,
                    CleanPeptide = result.CleanPeptide,
                    PeptideWithNumericMods = result.SeqWithNumericMods
                };
                i++;

                if (!filter.ShouldFilter(result))
                {
                    result.DataSet = new TargetDataSet() { Path = path };

                    result.ModificationCount = Convert.ToInt16(item.Value.Peptide.Mods.Count);
                    result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;

                    if (result.ModificationCount != 0)
                    {

                        string numModSeqPre = null;
                        string numModSeq = null;
                        foreach (KeyValuePair<int, Modification> mod in item.Value.Peptide.Mods)
                        {
                            for (int j = 0; j < mod.Key; j++)
                            {
                                numModSeqPre = numModSeqPre + item.Value.Peptide.Sequence[j];
                            }
                            numModSeq = numModSeqPre + mod.Value.Mass;
                            for (int j = mod.Key; j < item.Value.Peptide.Sequence.Length; j++)
                            {
                                numModSeq = numModSeq + item.Value.Peptide.Sequence[j];
                            }
                            result.SeqInfoMonoisotopicMass += mod.Value.Mass;
                            result.ModificationDescription += mod.Value.Tag + ":" + mod.Key + "  ";
                        }
                    }
                    results.Add(result);
                }
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(this.ReaderOptions.PredictorType), results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MzIdentMl, results);
        }
    }
}
