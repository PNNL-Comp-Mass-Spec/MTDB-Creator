using System;
using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;
using PHRPReader;

namespace MTDBCreatorTestSuite.IO
{
    /// <summary>
    /// Tests for the Phrpreader converting data into the MTDB Creator object model.
    /// </summary>
    [TestFixture]
    public sealed class PhrpReaderTests
    {
        [Test]
        public void PhrpDatasetNameTest()
        {
            Console.WriteLine(clsPHRPReader.AutoDetermineDatasetName("QC_Shew_10_02a_2Nov10_Cougar_10-09-06_syn.txt"));
        }

        //[TestMethod]
        //public void MZIdentReaderTest()
        //{
        //    Options options = new Options();
        //    MZIdentReaderML reader = new MZIdentReaderML();
        //    reader.TestReader();
        //}

        [Test]
        public void PhrpDatasetResultTest()
        {
            var resultType = clsPHRPReader.AutoDetermineResultType("QC_Shew_Intact_F7a_4Feb14_Tiger_13-11-13_msalign_syn.txt");
            if (resultType != clsPHRPReader.ePeptideHitResultType.Unknown)
            {
                var results = new List<MsgfPlusResult>();
                
                var option = new Options();
                var sreader = new XTandemPhrpReader(option);
                var filter = new XTandemTargetFilter(sreader.ReaderOptions);
                //data = Sreader.Read(@"C:\UnitTestFolder\Xtandem\QC_Shew_13-04_pt5_2b_CID_21Jun13_Leopard_13-05-32_xt.txt");
                var reader = new PHRPReader.clsPHRPReader(@"C:\UnitTestFolder\MSAlign\QC_Shew_Intact_F7a_4Feb14_Tiger_13-11-13_msalign_syn.txt");
                while (reader.CanRead)
                {
                    var result = new MsgfPlusResult();
                    reader.MoveNext();

                    result.AnalysisId = reader.CurrentPSM.ResultID;
                    if(reader.CurrentPSM.ResultID == 1491)
                    {
                        result.AnalysisId = reader.CurrentPSM.ResultID;
                    }
                    if (reader.CurrentPSM.ResultID == 2614)
                    {
                        result.AnalysisId = reader.CurrentPSM.ResultID;
                    }
                    result.Charge = reader.CurrentPSM.Charge;
                    result.CleanPeptide = reader.CurrentPSM.PeptideCleanSequence;
                    result.MonoisotopicMass = reader.CurrentPSM.PeptideMonoisotopicMass;
                    result.MultiProteinCount = (short)reader.CurrentPSM.Proteins.Count;
                    result.Scan = reader.CurrentPSM.ScanNumber;
                    result.Sequence = reader.CurrentPSM.Peptide;
                    //result.BScore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["b_score"]);
                    //result.DeltaCn2 = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["DeltaCn2"]);
                    //result.DeltaMass = Convert.ToDouble(reader.CurrentPSM.MassErrorDa);
                    //result.LogIntensity = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Intensity_Log(I)"]);
                    //result.LogPeptideEValue = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Expectation_Value_Log(e)"]);
                    //result.NumberBIons = Convert.ToInt16(reader.CurrentPSM.AdditionalScores["b_ions"]);
                    //result.NumberYIons = Convert.ToInt16(reader.CurrentPSM.AdditionalScores["y_ions"]);
                    //result.PeptideHyperscore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["Peptide_Hyperscore"]);
                    //result.TrypticState = reader.CurrentPSM.NumTrypticTerminii;
                    //result.YScore = Convert.ToDouble(reader.CurrentPSM.AdditionalScores["y_score"]);
                    //result.Mz = result.MonoisotopicMass / result.Charge;
                    //result.DeltaM_PMM = Convert.ToDouble(reader.CurrentPSM.MassErrorPPM);

                    //result.ModificationCount = (short)reader.CurrentPSM.ModifiedResidues.Count;
                    //result.SeqInfoMonoisotopicMass = result.MonoisotopicMass;
                    ////reader.CurrentPSM.ModifiedResidues.

                    //foreach (clsAminoAcidModInfo info in reader.CurrentPSM.ModifiedResidues)
                    //{
                    //    result.SeqInfoMonoisotopicMass += info.ModDefinition.ModificationMass;
                    //    result.ModificationDescription += info.ModDefinition.MassCorrectionTag + ":" + info.ResidueLocInPeptide + " ";
                    //}

                    if (!filter.ShouldFilter(result))
                    {
                        results.Add(result);
                    }

                }

                //Debug.Assert(results.Count == data.Evidences.Count);
                //for(int i = 0; i < results.Count; i++)
                //{
                //    Debug.Assert(results[i].AnalysisId == data.Evidences[i].AnalysisId);
                //    Debug.Assert(results[i].Charge == data.Evidences[i].Charge);
                //    Debug.Assert(results[i].CleanPeptide == data.Evidences[i].CleanPeptide);
                //    //Debug.Assert(results[i].MonoisotopicMass == data.Evidences[i].MonoisotopicMass, results[i].MonoisotopicMass + " " + data.Evidences[i].MonoisotopicMass);
                //    //Debug.Assert(results[i].Mz == data.Evidences[i].Mz);
                //    Debug.Assert(results[i].Scan == data.Evidences[i].Scan);
                //    Debug.Assert(results[i].Sequence == data.Evidences[i].Sequence);
                //    Debug.Assert(results[i].MultiProteinCount == data.Evidences[i].MultiProteinCount);
                //    //Debug.Assert(results[i].NumberBIons == data.Evidences[i].AnalysisId);
                //    //Debug.Assert(results[i].NumberYIons == data.Evidences[i].AnalysisId);
                //    //Debug.Assert(results[i].PeptideHyperscore == data.Evidences[i].AnalysisId);

                //}
            }

            Console.WriteLine(clsPHRPReader.AutoDetermineDatasetName("QC_Shew_Intact_F7a_4Feb14_Tiger_13-11-13_msalign_syn.txt"));
        }
    }
}
