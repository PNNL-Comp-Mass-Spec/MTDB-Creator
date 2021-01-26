using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;
using MTDBFrameworkBase.Database;
using PHRPReader;
using PSI_Interface.IdentData;

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
        /// experimental mz, charge state, MSGF raw score, DeNovo score, MSGF SpecEValue, MSGF EValue,
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
            var reader = new SimpleMZIdentMLReader();

            // Read in the file
            var mzidData = reader.ReadLowMem(path);
            UpdateProgress(20);

            // Map to MSGF+ results
            MapToMsgf(results, path, mzidData.Identifications);

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MZIdentML, results);
        }

        /// <summary>
        /// Map the results of a MZIdentML read to MSGF+
        /// </summary>
        /// <param name="results">Object to populate with the results of the Mapping</param>
        /// <param name="path">Path to MZIdentML file</param>
        /// <param name="identifications">identifications from mzid file</param>
        private void MapToMsgf(ICollection<MsgfPlusResult> results, string path, IEnumerable<SimpleMZIdentMLReader.SpectrumIdItem> identifications)
        {
            var filter = new MsgfPlusTargetFilter(ReaderOptions);

            var peptideMassCalculator = new clsPeptideMassCalculator();
            var cleavageStateCalculator = new clsPeptideCleavageStateCalculator();

            var i = 0;
            // Go through each Spectrum ID and map it to an MSGF+ result
            foreach (var item in identifications)
            {
                i++;
                // Skip this PSM if it doesn't pass the import filters
                // Note that qValue is basically FDR
                var qValue = item.QValue;

                var specProb = item.SpecEv;

                if (filter.ShouldFilter(qValue, specProb))
                    continue;

                if (item.PepEvidence.Count == 0)
                    continue;

                var evidence = item.PepEvidence[0];

                var result = new MsgfPlusResult
                {
                    AnalysisId = i,
                    Charge = Convert.ToInt16(item.Charge),
                    CleanPeptide = item.Peptide.Sequence,
                    SeqWithNumericMods = null,
                    MonoisotopicMass = peptideMassCalculator.ConvoluteMass(item.CalMz, item.Charge, 0),
                    ObservedMonoisotopicMass = peptideMassCalculator.ConvoluteMass(item.ExperimentalMz, item.Charge, 0),
                    MultiProteinCount = Convert.ToInt16(item.PepEvCount),
                    Scan = item.ScanNum,
                    Sequence = evidence.Pre + "." + item.Peptide.Sequence + "." + evidence.Post,
                    Mz = 0,
                    SpecProb = specProb,
                    DelM = 0,
                    ModificationCount = Convert.ToInt16(item.Peptide.Mods.Count)
                };

                // Populate some mass related items
                result.DelM = result.ObservedMonoisotopicMass - result.MonoisotopicMass;
                result.DelMPpm = clsPeptideMassCalculator.MassToPPM(result.DelM, result.ObservedMonoisotopicMass);
                // We could compute m/z:
                //     Mz = clsPeptideMassCalculator.ConvoluteMass(result.ObservedMonoisotopicMass, 0, result.Charge);
                // But it's stored in the mzid file, so we'll use that
                result.Mz = item.ExperimentalMz;

                StoreDatasetInfo(result, path);

                result.DataSet.Tool = LcmsIdentificationTool.MZIdentML;

                // Populate items specific to the MSGF+ results (stored as mzid)

                result.Reference = evidence.DbSeq.Accession;

                var eCleavageState = cleavageStateCalculator.ComputeCleavageState(item.Peptide.Sequence, evidence.Pre, evidence.Post);
                result.NumTrypticEnds = clsPeptideCleavageStateCalculator.CleavageStateToShort(eCleavageState);

                result.DeNovoScore = item.DeNovoScore;
                result.MsgfScore = (int)item.RawScore;
                result.SpecEValue = item.SpecEv;
                result.RankSpecEValue = item.Rank;

                result.EValue = item.EValue;
                result.QValue = qValue;
                result.DiscriminantValue = qValue;
                result.PepQValue = item.PepQValue;

                result.IsotopeError = item.IsoError;

                if (result.ModificationCount > 0)
                {
                    var j = 0;

                    var numModSeq = evidence.Pre + ".";
                    var encodedSeq = numModSeq;
                    foreach (var mod in item.Peptide.Mods)
                    {
                        var ptm = new PostTranslationalModification
                        {
                            Location = mod.Key,
                            Mass = mod.Value.Mass
                        };
                        if (UniModData.ModList.TryGetValue(mod.Value.Tag, out var unimod))
                        {
                            ptm.Formula = unimod.Formula.ToString();
                            ptm.Name = unimod.Title;
                        }
                        else
                        {
                            // Unknown modification (to unimod) - store the name if we have it, otherwise use the mass as the name
                            if (mod.Value.Tag.Length > 0)
                            {
                                ptm.Name = mod.Value.Tag;
                            }
                            else
                            {
                                var plusSign = mod.Value.Mass >= 0 ? "+" : "";
                                ptm.Name = plusSign + mod.Value.Mass.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        result.Ptms.Add(ptm);

                        for (; j < ptm.Location; j++)
                        {
                            numModSeq = numModSeq + item.Peptide.Sequence[j];
                            encodedSeq = encodedSeq + item.Peptide.Sequence[j];
                        }

                        numModSeq += (ptm.Mass > 0) ? "+" : "-";
                        numModSeq = numModSeq + ptm.Mass;

                        encodedSeq += "[" + ((ptm.Mass > 0) ? "+" : "-") + ptm.Formula + "]";
                    }
                    for (; j < item.Peptide.Sequence.Length; j++)
                    {
                        numModSeq = numModSeq + item.Peptide.Sequence[j];
                        encodedSeq += item.Peptide.Sequence[j];
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

                foreach (var thing in item.PepEvidence)
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

                    foreach (var mod in item.Peptide.Mods)
                    {
                        // TODO: Confirm that this is valid math (MEM thinks it may not be)
                        result.SeqInfoMonoisotopicMass += mod.Value.Mass;

                        result.ModificationDescription += mod.Value.Tag + ":" + mod.Key + "  ";
                    }
                }

                results.Add(result);
            }

            UpdateProgress(100);
        }

        private void ComputeTerminusState(SimpleMZIdentMLReader.PeptideEvidence evidence, short numTrypticEnds, ProteinInformation protein)
        {
            if (evidence.Pre[0] == '-')
            {
                if (evidence.Post[0] == '-')
                {
                    protein.TerminusState =
                        clsPeptideCleavageStateCalculator.PeptideTerminusStateConstants.ProteinNandCCTerminus;
                    protein.CleavageState = clsPeptideCleavageStateCalculator.PeptideCleavageStateConstants.Full;
                }
                else
                {
                    protein.TerminusState = clsPeptideCleavageStateCalculator.PeptideTerminusStateConstants.ProteinNTerminus;
                }
            }
            else if (evidence.Post[0] == '-')
            {
                protein.TerminusState = clsPeptideCleavageStateCalculator.PeptideTerminusStateConstants.ProteinCTerminus;
            }
            else
            {
                protein.TerminusState = clsPeptideCleavageStateCalculator.PeptideTerminusStateConstants.None;
            }

            switch (numTrypticEnds)
            {
                case 0:
                    protein.CleavageState = clsPeptideCleavageStateCalculator.PeptideCleavageStateConstants.NonSpecific;
                    break;
                case 1:
                    protein.CleavageState =
                        clsPeptideCleavageStateCalculator.PeptideCleavageStateConstants.Partial;
                    break;
                case 2:
                    protein.CleavageState = clsPeptideCleavageStateCalculator.PeptideCleavageStateConstants.Full;
                    break;
            }
        }
    }
}
