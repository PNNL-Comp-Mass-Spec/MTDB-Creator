using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Data
{
    [TestFixture]
    public sealed class PeptideSequenceTests : TestBase
    {
        // Peptides shouldn't have negative number of tryptic ends OR a number greater than 3
        [Test]
        [TestCase("Sequest", "ManySequestList.txt", 1)]
        [TestCase("Sequest", "ManySequestList.txt", 1)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 50)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 50)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 100)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 100)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 500)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 500)]
        public void TrypticEnds(string jobDirectory, string jobList, int numJobs)
        {
            var jobDirectoryPath    = GetPath(jobDirectory);
            var jobListPath         = GetPath(jobList);
            LcmsDataSet data        = null;
            var num                 = 0;
            PeptideCache.Clear();

            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs)
                {
                    data        = new LcmsDataSet();
                    pathName    = System.IO.Path.Combine(jobDirectoryPath, pathName);
                    var options = new Options();
                    var reader  = new SequestPhrpReader(options);
                    data        = reader.Read(pathName);

                    foreach (SequestResult evidence in data.Evidences)
                    {
                        Debug.Assert(evidence.NumTrypticEnds >= 0);
                        Debug.Assert(evidence.NumTrypticEnds < 3);
                    }

                    pathName = sr.ReadLine();
                    num++;
                }
            }
        }

        // Peptides with PTMs should have different values for observed NET, delta Mass, and MZ 
        [Test]
        [TestCase(@"XTandemData\QC_Shew_12_02_pt5_2b_20Dec12_Leopard_12-11-10_xt.txt")]
        public void PTMCheck(string relativePath)
        {
            LcmsDataSet data    = null;
            var path            = GetPath(relativePath);
            data                = new LcmsDataSet();
            var options         = new Options();
            var reader          = new XTandemPhrpReader(options);
            data                = reader.Read(path);
            var modFound        = false;
            var target1         = new XTandemResult();
            var target2         = new List<XTandemResult>();

            foreach (XTandemResult evidence in data.Evidences)
            {
                if (evidence.Sequence == "R.Q&AVTNPNNTFFAIKR.L" && !modFound)
                {
                    target1     = evidence;
                    modFound    = true;
                }
                if (evidence.Sequence == "R.QAVTNPNNTFFAIKR.L")
                {
                    target2.Add(evidence);
                }
            }
            foreach (var evidence in target2)
            {
                Debug.Assert(target1.ObservedNet != evidence.ObservedNet);
                Debug.Assert(target1.DelM != evidence.DelM);
                Debug.Assert(target1.Mz != evidence.Mz);
            }
        }
    }
}
