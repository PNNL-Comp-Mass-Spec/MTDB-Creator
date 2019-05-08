using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using MTDBFrameworkBase.Data;
using NUnit.Framework;
using PHRPReader;

namespace MTDBCreatorTestSuite.Data
{
    [TestFixture]
    public sealed class PeptideSequenceTests : TestBase
    {
        // Peptides shouldn't have negative number of tryptic ends OR a number greater than 3
        [Test]
        [TestCase("Sequest", "ManySequestList.txt", 1)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 1)]
        [TestCase("Sequest", "ManySequestList.txt", 3)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 3)]
        public void TrypticEnds(string jobDirectory, string jobList, int numJobs)
        {
            var jobDirectoryPath    = GetPath(jobDirectory);
            var jobListPath         = GetPath(jobList);
            var num                 = 0;
            var options             = new Options();
            PeptideCache.Clear();
            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs)
                {
                    pathName        = Path.Combine(jobDirectoryPath, pathName);
                    var reader      = PhrpReaderFactory.Create(pathName, options);
                    var data            = reader.Read(pathName);
                    var resultType  = clsPHRPReader.AutoDetermineResultType(pathName);
                    switch (resultType)
                    {
                        case clsPHRPReader.ePeptideHitResultType.XTandem:
                            foreach (var item in data.Evidences)
                            {
                                var evidence = (XTandemResult)item;
                                Debug.Assert(evidence.NumTrypticEnds >= 0);
                                Debug.Assert(evidence.NumTrypticEnds < 3);
                            }
                            break;

                        case clsPHRPReader.ePeptideHitResultType.Sequest:
                            foreach (var item in data.Evidences)
                            {
                                var evidence = (SequestResult)item;
                                Debug.Assert(evidence.NumTrypticEnds >= 0);
                                Debug.Assert(evidence.NumTrypticEnds < 3);
                            }
                            break;

                        case clsPHRPReader.ePeptideHitResultType.MSGFPlus:
                            foreach (var item in data.Evidences)
                            {
                                var evidence = (MsgfPlusResult)item;
                                Debug.Assert(evidence.NumTrypticEnds >= 0);
                                Debug.Assert(evidence.NumTrypticEnds < 3);
                            }
                            break;
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
            var path            = GetPath(relativePath);
            var options         = new Options();
            var reader          = new XTandemPhrpReader(options);
            var data            = reader.Read(path);
            var modFound        = false;
            var target1         = new Evidence();
            var target2         = new List<Evidence>();

            foreach (var evidence in data.Evidences)
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
                Debug.Assert(Math.Abs(target1.ObservedNet - evidence.ObservedNet) > double.Epsilon);
                Debug.Assert(Math.Abs(target1.DelM - evidence.DelM) > double.Epsilon);
                Debug.Assert(Math.Abs(target1.Mz - evidence.Mz) > double.Epsilon);
            }
        }
    }
}
