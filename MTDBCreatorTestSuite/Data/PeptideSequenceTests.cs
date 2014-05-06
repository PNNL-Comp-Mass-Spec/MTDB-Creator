using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Data
{
    [TestFixture]
    public sealed class PeptideSequenceTests
    {
        

        // Peptides shouldn't have negative number of tryptic ends OR a number greater than 3
        [Test]
        public void TrypticEnds()
        {
            LcmsDataSet data = null;
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    data = new LcmsDataSet();
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    data = reader.Read(pathName);
                    foreach (SequestResult evidence in data.Evidences)
                    {
                        Debug.Assert(evidence.NumTrypticEnds >= 0);
                        Debug.Assert(evidence.NumTrypticEnds < 3);
                    }

                    pathName = sr.ReadLine();
                }
            }
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    data = new LcmsDataSet();
                    num++;
                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    data = reader.Read(pathName);
                    foreach(XTandemResult evidence in data.Evidences)
                    {
                        Debug.Assert(evidence.TrypticState >= 0);
                        Debug.Assert(evidence.TrypticState < 3);
                    }
                    pathName = sr.ReadLine();
                }
            }
        }

        // Peptides with PTMs should have different values for observed NET, delta Mass, and MZ 
        [Test]
        public void PTMCheck()
        {
            LcmsDataSet data = null;
            var path = @"C:\deganDev\LamarcheData\XTandemData\QC_Shew_12_02_pt5_2b_20Dec12_Leopard_12-11-10_xt.txt";

            data = new LcmsDataSet();

            var options = new Options();

            var reader = new XTandemPhrpReader(options);

            data = reader.Read(path);
            var modFound = false;
            var target1 = new XTandemResult();
            var target2 = new List<XTandemResult>();
            foreach (XTandemResult evidence in data.Evidences)
            {
                if (evidence.Sequence == "R.Q&AVTNPNNTFFAIKR.L" && !modFound)
                {
                    target1 = evidence;
                    modFound = true;
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
