using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MTDBFramework.Algorithms;
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
        [TestCase("Xtandem", "ManyXtandemList.txt", 1)]
        [TestCase("Xtandem", "ManyXtandemList.txt", 3)]
        [TestCase("Sequest", "ManySequestList.txt", 3)]
        public void LinearRegression(string jobDirectory, string jobList, int numJobs)
        {
            var jobDirectoryPath    = GetPath(jobDirectory);
            var jobListPath         = GetPath(jobList);
            var num                 = 0;
            var options             = new Options();
            PeptideCache.Clear();
            var dataSets = new List<LcmsDataSet>();
            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs)
                {
                    pathName    = Path.Combine(jobDirectoryPath, pathName);
                    var reader  = PhrpReaderFactory.Create(pathName, options);
                    dataSets.Add(reader.Read(pathName));
                    pathName    = sr.ReadLine();
                    num++;
                }
            }
            var processor   = new MtdbProcessor(options);
            processor.Process(dataSets, new BackgroundWorker());
            foreach (var set in dataSets)
            {
                Assert.AreNotEqual(set.RegressionResult.RSquared, 0);

                // Set to 1.000001 due to (for some reason) RSquared occasionally
                // being calculated as 1.00000000000044
                Assert.Less(set.RegressionResult.RSquared, 1.000001);
            }
        }
    }
}