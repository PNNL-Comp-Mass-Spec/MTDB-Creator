using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;
using PNNLOmics.Algorithms.Regression;
using MTDBFramework.Algorithms.Alignment;

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
            
            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs) 
                {
                    pathName    = Path.Combine(jobDirectoryPath, pathName);
                    var reader  = PhrpReaderFactory.Create(pathName, options);
                    var linData        = reader.Read(pathName);
                    var mixData = new LcmsDataSet();
                    var linRegressor = LinearRegressorFactory.Create(RegressionType.LinearEm);
                    var mixRegressor = LinearRegressorFactory.Create(RegressionType.MixtureRegression);

                    var targetFilter = TargetFilterFactory.Create(linData.Tool, options);
                    var alignmentFilter = AlignmentFilterFactory.Create(linData.Tool, options);

                    var filteredTargets = new List<Evidence>();
                    var alignedTargets = new List<Evidence>();

                    foreach (var t in linData.Evidences)
                    {
                        if (!targetFilter.ShouldFilter(t))
                        {
                            filteredTargets.Add(t);

                            if (!alignmentFilter.ShouldFilter(t))
                            {
                                alignedTargets.Add(t);
                            }
                        }
                    }

                    linData.RegressionResult =
                        linRegressor.CalculateRegression(alignedTargets.Select(d => d.ObservedNet).ToList(),
                            alignedTargets.Select(d => d.PredictedNet).ToList());
                    mixData.RegressionResult =
                        mixRegressor.CalculateRegression(alignedTargets.Select(d => d.ObservedNet).ToList(),
                            alignedTargets.Select(d => d.PredictedNet).ToList());
                    Debug.Assert(linData.RegressionResult.RSquared < 1.0001);
                    Debug.Assert(mixData.RegressionResult.RSquared < 1.0001);
                    Debug.Assert(mixData.RegressionResult.RSquared > 0.8500);
                    pathName = sr.ReadLine();
                    num++;
                }
            }
        }
    }
}