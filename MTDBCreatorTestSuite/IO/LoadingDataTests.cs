using System;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    /// <summary>
    /// These tests integrates reading using Phrp and creating an analysis in MTDBCreator
    /// </summary>
    [TestFixture]
    public class LoadingDataTests
    {
        [Test]
        public void RunOneMSGF()
        {
            PeptideCache.Clear();
            var pathName = @"C:\UnitTestFolder\MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt";

            var options = new Options();
            var reader = new MsgfPlusPhrpReader(options);

            reader.Read(pathName);

        }

        //Unit test to ensure that MTDB runs for a single XTandem
        [Test]
        public void Run1Xtandem()
        {
            PeptideCache.Clear();
            var pathName = @"C:\UnitTestFolder\Xtandem\QC_Shew_12_02_pt5_2b_20Dec12_Leopard_12-11-10_xt.txt";

            var options = new Options();

            var reader = new XTandemPhrpReader(options);

            reader.Read(pathName);

        }

        [Test]
        public void Run5Xtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 5 && pathName != null)
                {
                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run10Xtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 10 && pathName != null)
                {
                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run20Xtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();

                while (num < 20 && pathName != null)
                {

                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run50Xtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();

                while (num < 50 && pathName != null)
                {

                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run75Xtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();

                while (num < 75 && pathName != null)
                {

                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void RunAllXtandem()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        //Unit test to ensure that MTDB runs for a single SeQuest
        [Test]
        public void Run1SeQuest()
        {
            PeptideCache.Clear();
            var pathName = @"C:\UnitTestFolder\Sequest\QC_Shew_10_02a_2Nov10_Cougar_10-09-06_syn.txt";

            var options = new Options();

            var reader = new SequestPhrpReader(options);

            reader.Read(pathName);

        }

        [Test]
        public void Run5Sequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 5 && pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;
                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run10Sequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 10 && pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run20Sequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 20 && pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run50Sequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 50 && pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void Run75Sequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (num < 75 && pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void RunAllSequest()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            Console.WriteLine("Read {0} files", num);
        }

        [Test]
        public void RunAllFiles()
        {
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    num++;
                    pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                    var options = new Options();

                    var reader = new SequestPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                }
            }
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManyXtandemList.txt"))
            {
                var pathName = sr.ReadLine();
                while (pathName != null)
                {
                    pathName = @"C:\UnitTestFolder\Xtandem\" + pathName;

                    var options = new Options();

                    var reader = new XTandemPhrpReader(options);

                    reader.Read(pathName);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
            Console.WriteLine("Read {0} files", num);
        }
    }
}