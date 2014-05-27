using System.Diagnostics;
using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Algorithms
{
    [TestFixture]
    public class MathRegressionTests : TestBase
    {
        // Data sets should have a linear regression slope between 0.5 and 2.0
        // Test runs over 200 varied datasets for verification that it's accurate in many instances
        [Test]
        [TestCase("Sequest", "ManySequestList.txt", 1)]
        [TestCase("Sequest", "ManySequestList.txt", 1)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 5)]
        [TestCase("Sequest", "ManySequestList.txt", 5)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 10)]
        [TestCase("Sequest", "ManySequestList.txt", 10)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 50, Ignore = true)]
        [TestCase("Sequest", "ManySequestList.txt", 50, Ignore = true)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 100, Ignore = true)]
        [TestCase("Sequest", "ManySequestList.txt", 100, Ignore = true)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 500, Ignore = true)]
        [TestCase("Sequest", "ManySequestList.txt", 500, Ignore = true)]
        public void LinearRegression(string jobDirectory, string jobList, int numJobs)
        {
            var jobDirectoryPath    = GetPath(jobDirectory);
            var jobListPath         = GetPath(jobList);
            LcmsDataSet data        = null;
            var num                 = 0;
            var options             = new Options();
            PeptideCache.Clear();
            
            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs) 
                {
                    data        = new LcmsDataSet();
                    pathName    = Path.Combine(jobDirectoryPath, pathName);
                    var reader  = PhrpReaderFactory.Create(pathName, options);
                    data        = reader.Read(pathName);

                    Debug.Assert(data.RegressionResult.Slope > 0.5);
                    Debug.Assert(data.RegressionResult.Slope < 2.0);

                    pathName = sr.ReadLine();
                    num++;
                }
            }
        }
    }
}