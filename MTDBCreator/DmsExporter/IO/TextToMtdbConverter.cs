using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;

namespace MTDBCreator.DmsExporter.IO
{
    internal class TextToMtdbConverter : ITextToDbConverter
    {
        private char m_separator = '\t';

        private Dictionary<int, string> m_idToMassTagDict;
        private Dictionary<int, ConsensusTarget> m_idToConensusTargetDict;
        private Dictionary<int, Tuple<string, List<short>>> m_idToChargeAndPeptide;
        private Dictionary<string, Tuple<double, string>> m_modTagsToModMass;
        private Dictionary<string, PostTranslationalModification> m_ptmDictionary;
        private Dictionary<int, ConsensusPtmPair> m_consensusTargetToPtmDict;
        private Dictionary<int, ProteinInformation> m_idToProteinDict;
        private Dictionary<int, ConsensusProteinPair> m_ctToProtDict;
        private Dictionary<int, List<int>> m_ctToEvidenceMap;
        private Dictionary<int, Evidence> m_evidenceDict;

        /// <summary>
        /// Writes as the data from DMS to the MTDB format
        /// </summary>
        /// <param name="path">Location of the written database</param>
        public void ConvertToDbFormat(string path)
        {
            m_idToMassTagDict = new Dictionary<int, string>();
            m_idToConensusTargetDict = new Dictionary<int, ConsensusTarget>();
            m_idToChargeAndPeptide = new Dictionary<int, Tuple<string, List<short>>>();
            m_modTagsToModMass = new Dictionary<string, Tuple<double, string>>();
            m_ptmDictionary = new Dictionary<string, PostTranslationalModification>();
            m_consensusTargetToPtmDict = new Dictionary<int, ConsensusPtmPair>();
            m_idToProteinDict = new Dictionary<int, ProteinInformation>();
            m_ctToProtDict = new Dictionary<int, ConsensusProteinPair>();
            m_ctToEvidenceMap = new Dictionary<int, List<int>>();
            m_evidenceDict = new Dictionary<int, Evidence>();

            if (File.Exists(path))
            {
                File.Delete(path);
            }


            var pieces = path.Split('\\');
            string directory = "";
            foreach (var piece in pieces)
            {
                if (piece.Contains("."))
                {
                    continue;
                }
                directory += piece;
                directory += "\\";
            }

            RetrieveDataFromTextFiles(directory);

            CreateDataTables(path);

            PutDataIntoDatabase(path);

            File.Delete(directory + "tempMassTags.txt");
            File.Delete(directory + "tempPeptides.txt");
            File.Delete(directory + "tempModInfo.txt");
            File.Delete(directory + "tempMassTagsNet.txt");
            File.Delete(directory + "tempProteins.txt");
            File.Delete(directory + "tempMassTagToProteins.txt");
            File.Delete(directory + "tempAnalysisDescription.txt");
            File.Delete(directory + "tempFilterSet.txt");
        }

        private void PutDataIntoDatabase(string path)
        {
            using (var dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;"))
            {
                dbConnection.Open();

                using (var cmd = new SQLiteCommand(dbConnection))
                {
                    using (var trans = dbConnection.BeginTransaction())
                    {
                        var ctInsertText = "Insert into Target (TargetId, AverageNet, " +
                                       " StdevNet, PredictedNet, TheoreticalMonoIsotopicMass, " +
                                       " CleanSequence, PrefixResidue, SuffixResidue, " +
                                       " ModificationCount, ModificationDescription, MultiProteinCount) " +
                                       " VALUES (";

                        foreach (var target in m_idToConensusTargetDict.Values)
                        {
                            var ctValue = string.Format("{0}, {1}, {2}, {3}, {4}, '{5}', '{6}', '{7}', {8}, '{9}', {10});",
                                                        target.Id,
                                                        target.AverageNet,
                                                        target.StdevNet,
                                                        target.PredictedNet,
                                                        target.TheoreticalMonoIsotopicMass,
                                                        target.CleanSequence,
                                                        target.PrefixResidue,
                                                        target.SuffixResidue,
                                                        target.ModificationCount,
                                                        target.ModificationDescription,
                                                        target.MultiProteinCount);

                            cmd.CommandText = ctInsertText + ctValue;
                            cmd.ExecuteNonQuery();
                        }

                        var protInsertText = "Insert into ProteinInformation (ProteinId, ProteinName) " +
                                       " VALUES (";

                        foreach (var prot in m_idToProteinDict.Values)
                        {
                            var protValue = string.Format("{0}, '{1}');",
                                                        prot.Id,
                                                        prot.ProteinName);

                            cmd.CommandText = protInsertText + protValue;
                            cmd.ExecuteNonQuery();
                        }

                        var ptmInsertText = "Insert into PostTranslationalModification (PostTranslationModId, " +
                                       " Formula, Mass, Name) " +
                                       " VALUES (";

                        foreach (var ptm in m_ptmDictionary.Values)
                        {
                            var ptmValue = string.Format("{0}, '{1}', {2}, '{3}');",
                                                        ptm.Id,
                                                        ptm.Formula,
                                                        ptm.Mass,
                                                        ptm.Name);

                            cmd.CommandText = ptmInsertText + ptmValue;
                            cmd.ExecuteNonQuery();
                        }

                        var evInsertText = "Insert into Evidence (EvidenceId, Charge, ObservedNet, " +
                                       " NetShift, Mz, Scan, DelM, DelMPpm, QValue, SpecProb, " +
                                       " TargetId) " +
                                       " VALUES (";

                        foreach (var target in m_evidenceDict.Values)
                        {
                            var evValue = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});",
                                                        target.Id,
                                                        target.Charge,
                                                        target.ObservedMonoisotopicMass,
                                                        Convert.ToDouble(target.NetShift),
                                                        target.Mz,
                                                        target.Scan,
                                                        target.DelM,
                                                        target.DelMPpm,
                                                        target.DiscriminantValue,
                                                        target.SpecProb,
                                                        target.Parent.Id);

                            cmd.CommandText = evInsertText + evValue;
                            cmd.ExecuteNonQuery();
                        }

                        var ctPtmPairInsertText = "Insert into ConsensusPtmPair (PairId, Location, TargetId, PostTranslationModId) " +
                                       " VALUES (";

                        foreach (var ctPtmPair in m_consensusTargetToPtmDict)
                        {
                            var ctPtmPairValue = string.Format("{0}, {1}, {2}, {3});",
                                                        ctPtmPair.Key,
                                                        ctPtmPair.Value.Location,
                                                        ctPtmPair.Value.ConsensusId,
                                                        ctPtmPair.Value.PtmId);

                            cmd.CommandText = ctPtmPairInsertText + ctPtmPairValue;
                            cmd.ExecuteNonQuery();
                        }

                        var ctProtPairInsertText = "Insert into ConsensusProteinPair (PairId, CleavageState, TerminusState, " +
                                                   " ResidueStart, ResidueEnd, TargetId, ProteinId) " +
                                                   " VALUES (";

                        foreach (var ctProtPair in m_ctToProtDict)
                        {
                            var ctProtPairValue = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6});",
                                                        ctProtPair.Key,
                                                        ctProtPair.Value.CleavageState,
                                                        ctProtPair.Value.TerminusState,
                                                        ctProtPair.Value.ResidueStart,
                                                        ctProtPair.Value.ResidueEnd,
                                                        ctProtPair.Value.ConsensusId,
                                                        ctProtPair.Value.ProteinId);

                            cmd.CommandText = ctProtPairInsertText + ctProtPairValue;
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                }
            }
        }

        private void RetrieveDataFromTextFiles(string directory)
        {
            using (var reader = new StreamReader(directory + "tempModInfo.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);
                    m_modTagsToModMass.Add(rowPieces[0],
                        new Tuple<double, string>(Convert.ToDouble(rowPieces[1]), rowPieces[2]));
                    row = reader.ReadLine();
                }
            }

            var ptmId = 1;
            var targetId = 1;
            var ctToPtmId = 1;

            using (var reader = new StreamReader(directory + "tempMassTags.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);
                    var target = new ConsensusTarget();
                    target.Id = targetId++;
                    var quote = "";
                    var unescapedPiece = rowPieces[14].Replace("\"\"", quote);
                    var sequence = rowPieces[1];
                    if (unescapedPiece != "")
                    {
                        var unquotedPiece = unescapedPiece.Substring(1, rowPieces[14].Length - 2);
                        var mods = unquotedPiece.Split(',');
                        foreach (var mod in mods)
                        {
                            var modPieces = mod.Split(':');
                            var modMass = m_modTagsToModMass[modPieces[0]].Item1;
                            var ptm = new PostTranslationalModification();
                            ptm.Name = modPieces[0];
                            if (!m_ptmDictionary.ContainsKey(ptm.Name))
                            {
                                ptm.Mass = modMass;
                                ptm.Id = ptmId++;
                                ptm.Formula = m_modTagsToModMass[modPieces[0]].Item2;
                                m_ptmDictionary.Add(ptm.Name, ptm);
                            }
                            target.Ptms.Add(m_ptmDictionary[ptm.Name]);
                            ptm.Location = Convert.ToInt32(modPieces[1]);

                            var ctToPtm = new ConsensusPtmPair
                            {
                                ConsensusId = target.Id,
                                PtmId = m_ptmDictionary[ptm.Name].Id,
                                Location = ptm.Location,
                                Id = ctToPtmId++
                            };

                            m_consensusTargetToPtmDict[ctToPtm.Id] = ctToPtm;
                        }
                        target.ModificationDescription = unquotedPiece;
                    }
                    var fullSequence = sequence;
                    var backPtms = target.Ptms.OrderByDescending(x => x.Location);
                    foreach (var ptm in backPtms)
                    {
                        if (ptm.Location == rowPieces[1].Length)
                        {
                            rowPieces[1] += ptm.Mass.ToString();
                        }
                        else
                        {
                            rowPieces[1] = fullSequence.Insert(ptm.Location, ptm.Mass.ToString());
                        }
                    }
                    target.EncodedNumericSequence = rowPieces[1];
                    target.Sequence = fullSequence;
                    target.TheoreticalMonoIsotopicMass = Convert.ToDouble(rowPieces[2]);
                    target.MultiProteinCount = Convert.ToInt16(rowPieces[4]);
                    target.ModificationCount = Convert.ToInt16(rowPieces[13]);

                    m_idToMassTagDict.Add(Convert.ToInt32(rowPieces[0]), row);
                    m_idToConensusTargetDict.Add(Convert.ToInt32(rowPieces[0]), target);

                    row = reader.ReadLine();
                }
            }

            using (var reader = new StreamReader(directory + "tempMassTagsNet.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);

                    var id = Convert.ToInt32(rowPieces[0]);
                    m_idToConensusTargetDict[id].PredictedNet = Convert.ToDouble(rowPieces[7]);
                    m_idToConensusTargetDict[id].StdevNet = Convert.ToDouble(rowPieces[5]);
                    m_idToConensusTargetDict[id].AverageNet = Convert.ToDouble(rowPieces[3]);

                    row = reader.ReadLine();
                }
            }

            var proteinId = 1;
            using (var reader = new StreamReader(directory + "tempProteins.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);
                    var unquotedPiece = rowPieces[1].Substring(1, rowPieces[1].Length - 2);
                    var prot = new ProteinInformation {Id = proteinId++, ProteinName = unquotedPiece};

                    m_idToProteinDict[Convert.ToInt32(rowPieces[0])] = prot;

                    row = reader.ReadLine();
                }
            }

            var cppId = 1;
            using (var reader = new StreamReader(directory + "tempMassTagToProteins.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);
                    var mt_id = Convert.ToInt32(rowPieces[0]);
                    var prot_id = Convert.ToInt32(rowPieces[2]);
                    var ctToProt = new ConsensusProteinPair();
                    ctToProt.CleavageState = Convert.ToInt16(rowPieces[3]);
                    ctToProt.ResidueStart = Convert.ToInt32(rowPieces[6]);
                    ctToProt.ResidueEnd = Convert.ToInt32(rowPieces[7]);
                    ctToProt.TerminusState = Convert.ToInt16(rowPieces[9]);
                    ctToProt.ConsensusId = m_idToConensusTargetDict[mt_id].Id;
                    ctToProt.ProteinId = m_idToProteinDict[prot_id].Id;
                    m_ctToProtDict[cppId] = ctToProt;
                    cppId++;

                    row = reader.ReadLine();
                }
            }

            var totalCharges = 0;
            var evId = 1;
            using (var reader = new StreamReader(directory + "tempPeptides.txt"))
            {
                reader.ReadLine();
                var row = reader.ReadLine();
                while (!string.IsNullOrEmpty(row))
                {
                    var rowPieces = row.Split(m_separator);
                    var id = Convert.ToInt32(rowPieces[0]);
                    if (!m_idToChargeAndPeptide.ContainsKey(id))
                    {
                        m_idToChargeAndPeptide[id] = new Tuple<string, List<short>>(rowPieces[1], new List<short>());
                        m_idToChargeAndPeptide[id].Item2.Add(Convert.ToInt16(rowPieces[2]));
                        m_idToConensusTargetDict[id].Sequence = rowPieces[1][0] + "." +
                                                                m_idToConensusTargetDict[id].Sequence + "." +
                                                                rowPieces[1][rowPieces[1].Length - 1];
                        m_idToConensusTargetDict[id].CleanSequence = m_idToConensusTargetDict[id].Sequence;
                        m_idToConensusTargetDict[id].Charges.Add(Convert.ToInt16(rowPieces[2]));
                        totalCharges++;
                    }
                    if (!m_idToChargeAndPeptide[id].Item2.Contains(Convert.ToInt16(rowPieces[2])))
                    {
                        m_idToChargeAndPeptide[id].Item2.Add(Convert.ToInt16(rowPieces[2]));
                        m_idToConensusTargetDict[id].Charges.Add(Convert.ToInt16(rowPieces[2]));
                        totalCharges++;
                    }

                    var ctId = m_idToConensusTargetDict[id].Id;
                    var ev = new Evidence();
                    ev.Id = evId++;
                    ev.Charge = Convert.ToInt16(rowPieces[2]);
                    ev.Sequence = m_idToConensusTargetDict[id].CleanSequence;
                    ev.Scan = Convert.ToInt32(rowPieces[3]);
                    ev.DelMPpm = Convert.ToDouble(rowPieces[4]);
                    ev.ObservedNet = Convert.ToDouble(rowPieces[5]);
                    ev.ObservedMonoisotopicMass = Convert.ToDouble(rowPieces[6]);
                    ev.Mz = ev.ObservedMonoisotopicMass/ev.Charge;
                    ev.NetShift = 0;
                    ev.DelM = ev.DelMPpm/1000000;
                    ev.Parent = m_idToConensusTargetDict[id];

                    m_evidenceDict[evId] = ev;
                    if (!m_ctToEvidenceMap.ContainsKey(ctId))
                    {
                        m_ctToEvidenceMap[ctId] = new List<int>();
                    }
                    m_ctToEvidenceMap[ctId].Add(evId);

                    row = reader.ReadLine();
                }
            }
        }

        private void CreateDataTables(string path)
        {
            SQLiteConnection.CreateFile(path);
            var dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnection.Open();

            var protTableCreate = "Create table ProteinInformation " +
                                   " (ProteinId integer Primary Key, " +
                                   " ProteinName text)";

            var command = new SQLiteCommand(protTableCreate, dbConnection);
            command.ExecuteNonQuery();

            var targetTableCreate = "Create table Target " +
                                   " (TargetId integer Primary Key, " +
                                   " AverageNet double, " +
                                   " StdevNet double, " +
                                   " PredictedNet double, " +
                                   " TheoreticalMonoIsotopicMass double, " +
                                   " CleanSequence text, " +
                                   " PrefixResidue text, " +
                                   " SuffixResidue text, " +
                                   " ModificationCount smallint, " +
                                   " ModificationDescription text, " +
                                   " MultiProteinCount smallint)";

            command = new SQLiteCommand(targetTableCreate, dbConnection);
            command.ExecuteNonQuery();

            var ptmTableCreate = "Create table PostTranslationalModification " +
                                   " (PostTranslationModId integer Primary Key, " +
                                   " Formula text, " +
                                   " Mass double, " +
                                   " Name text)";

            command = new SQLiteCommand(ptmTableCreate, dbConnection);
            command.ExecuteNonQuery();

            var evidenceTableCreate = "Create table Evidence " +
                                   " (EvidenceId integer Primary Key, " +
                                   " Charge smallint, " +
                                   " ObservedNet double, " +
                                   " NetShift double, " +
                                   " Mz double, " +
                                   " Scan int, " +
                                   " DelM double, " +
                                   " DelMPpm double, " +
                                   " QValue double, " +
                                   " SpecProb double, " +
                                   " TargetId int," +
                                   " Foreign key(TargetId) References Target(TargetId))";

            command = new SQLiteCommand(evidenceTableCreate, dbConnection);
            command.ExecuteNonQuery();

            var ctProtPairCreate = "Create table ConsensusProteinPair " +
                                   " (PairId integer Primary Key, " +
                                   " CleavageState smallInt, " +
                                   " TerminusState smallInt, " +
                                   " ResidueStart smallInt, " +
                                   " ResidueEnd smallInt, " +
                                   " TargetId int, " +
                                   " ProteinId int," +
                                   " Foreign key(TargetId) References Target(TargetId), " +
                                   " Foreign key(ProteinId) References ProteinInformation(ProteinId))";

            command = new SQLiteCommand(ctProtPairCreate, dbConnection);
            command.ExecuteNonQuery();

            var ctPtmPairCreate = "Create table ConsensusPtmPair " +
                                   " (PairId integer Primary Key, " +
                                   " Location int, " +
                                   " TargetId int, " +
                                   " PostTranslationModId int," +
                                   " Foreign key(TargetId) References Target(TargetId), " +
                                   " Foreign key(PostTranslationModId) References PostTranslationModification(PostTranslationModId))";

            command = new SQLiteCommand(ctPtmPairCreate, dbConnection);
            command.ExecuteNonQuery();


            dbConnection.Close();
        }
    }
}

