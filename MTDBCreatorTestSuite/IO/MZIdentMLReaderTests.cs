using System;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    /// <summary>
    /// These tests integrates reading using Phrp and creating an analysis in MTDBCreator
    /// </summary>
    [TestFixture]
    public class MZIdentMLReaderTests : TestBase
    {
        [Test]
        [TestCase(@"MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt", 2189)] // Used to cross-verify the results
        [TestCase(@"MSGFPlus\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfdb_syn.txt", 15384)]    // Used to cross-verify the results
        [TestCase(@"MSGFPlus\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfdb_syn.txt", 15475)]    // Used to cross-verify the results
        [TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", 2189)]
        [TestCase(@"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", 15384)]
        [TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", 15475)]
        public void TestLoadingSingleFile(string jobPath,  int expectedEvidences)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var jobDirectoryPath    = GetPath(jobPath);

            var pathName    = jobDirectoryPath;
            var reader      = PhrpReaderFactory.Create(pathName, options);
            var data        = reader.Read(pathName);
            Assert.AreEqual(expectedEvidences, data.Evidences.Count);
        }

        [Test]
        [TestCase(@"GzipTestFolder\1057199_Dey_IMERblast_02_08May14_Alder_14-01-33_msgfplus.mzid", @"GzipTestFolder\1057199_Dey_IMERblast_02_08May14_Alder_14-01-33_msgfplus.mzid.gz")]
        public void TestLoadingGZippedFile(string txtPath, string gZipPath)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var gZipDirectoryPath   = GetPath(gZipPath);
            var txtDirectoryPath    = GetPath(txtPath);

            var gPathName   = gZipDirectoryPath;
            var gReader     = PhrpReaderFactory.Create(gPathName, options);
            var timeStart   = DateTime.Now;
            var gData       = gReader.Read(gPathName);
            var timeEnd     = DateTime.Now;
            Console.WriteLine(string.Format("Reading .gz took {0} seconds", timeEnd - timeStart));

            var tPathName   = txtDirectoryPath;
            var tReader     = PhrpReaderFactory.Create(tPathName, options);
            timeStart       = DateTime.Now;
            var tData       = tReader.Read(tPathName);
            timeEnd         = DateTime.Now;

            Console.WriteLine(string.Format("Reading .mzid took {0} seconds", timeEnd - timeStart));

            // To ensure that reading the same data gets the same number of evidences
            Assert.AreEqual(gData.Evidences.Count, tData.Evidences.Count);
            for (var i = 0; i < gData.Evidences.Count; i++)
            {
                var gEv = gData.Evidences[i];
                var tEv = tData.Evidences[i];
                //Test for each portion of the evidence being identical
                Assert.AreEqual(gEv.MonoisotopicMass, tEv.MonoisotopicMass);
                Assert.AreEqual(gEv.Charge, tEv.Charge);
                Assert.AreEqual(gEv.DelM, tEv.DelM);
                Assert.AreEqual(gEv.DelMPpm, tEv.DelMPpm);
                Assert.AreEqual(gEv.ModificationCount, tEv.ModificationCount);
                Assert.AreEqual(gEv.ModificationDescription, tEv.ModificationDescription);
                Assert.AreEqual(gEv.Mz, tEv.Mz);
                Assert.AreEqual(gEv.ObservedMonoisotopicMass, tEv.ObservedMonoisotopicMass);
                Assert.AreEqual(gEv.ObservedNet, tEv.ObservedNet);
                Assert.AreEqual(gEv.Scan, tEv.Scan);
                Assert.AreEqual(gEv.Sequence, tEv.Sequence);
                Assert.AreEqual(gEv.SpecProb, tEv.SpecProb);
            }
        }
    }
}