using System.Diagnostics;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Algorithms
{
    [TestFixture]
    public class MathRegressionTests
    {
        // Data sets should have a linear regression slope between 0.5 and 2.0
        // Test runs over 200 varied datasets for verification that it's accurate in many instances
        [Test]
        public void LinearRegression()
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
                    Debug.Assert(data.RegressionResult.Slope > 0.5);
                    Debug.Assert(data.RegressionResult.Slope < 2.0);

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
                    Debug.Assert(data.RegressionResult.Slope > 0.5);
                    Debug.Assert(data.RegressionResult.Slope < 2.0);

                    pathName = sr.ReadLine();
                }
            }
        }


        [Test]
        public void Sequest1Test()
        {
            LcmsDataSet data = null;
            PeptideCache.Clear();
            var num = 0;
            using (var sr = new StreamReader(@"C:\UnitTestFolder\ManySequestList.txt"))
            {
                var pathName = sr.ReadLine();
                //while (pathName != null)
                //{
                data = new LcmsDataSet();
                num++;
                pathName = @"C:\UnitTestFolder\Sequest\" + pathName;

                var options = new Options();

                var reader = new SequestPhrpReader(options);

                data = reader.Read(pathName);
                Debug.Assert(data.RegressionResult.Slope > 0.5);
                Debug.Assert(data.RegressionResult.Slope < 2.0);

                pathName = sr.ReadLine();
                // }
            }
            Debug.WriteLine("Read {0} files", num);
        }
    }
}