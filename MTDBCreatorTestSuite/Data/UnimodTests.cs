using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Data
{
    class UnimodTests : TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobPath"></param>
        /// <param name="expectedEvidences"></param>
        [Test]
        //[TestCase(@"MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt", 1819)] // Used to cross-verify the results
        //[TestCase(@"MSGFPlus\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfdb_syn.txt", 12414)]    // Used to cross-verify the results
        //[TestCase(@"MSGFPlus\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfdb_syn.txt", 12544)]    // Used to cross-verify the results
        //[TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", 1819)]
        //[TestCase(@"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", 12414)]
        [TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", 12544)]
        public void TestLoadingSingleFile(string jobPath, int expectedEvidences)
        {
            //var reader = new UniModReader();
            //reader.Read("unimod.xml");
            Assert.AreEqual("Iodoacetamide derivative", UniModData.ModList["Carbamidomethyl"]._fullName);
            Assert.AreEqual(4, UniModData.ModList["Carbamidomethyl"]._recordId);
            Assert.AreEqual(57.021464, UniModData.ModList["Carbamidomethyl"]._monoMass);
            Assert.AreEqual(57.0513, UniModData.ModList["Carbamidomethyl"]._avgMass);
            Assert.AreEqual("H(3) C(2) N O", UniModData.ModList["Carbamidomethyl"]._composition);
            Assert.AreEqual("H3C2N1O1", UniModData.ModList["Carbamidomethyl"].Formula);
        }
    }
}
