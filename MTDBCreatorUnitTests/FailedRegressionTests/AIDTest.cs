using System;
using System.IO;
using System.Collections.Generic;

using MTDBCreator;
using NUnit.Framework;

namespace MTDBCreatorUnitTests.FailedRegressionTests
{
    /// <summary>
    /// Class that tests some of the LC-MS/MS results that were failing during a run.
    /// </summary>
    [TestFixture]
    public class AIDTest
    {
        /// <summary>
        /// Tests a failed AID dataset 
        /// </summary>
        [Test]
        [Description("Tests a failed AID dataset AID_MPB_Hybrid_UnitTest001.txt")]
        public void FailedDataset01()
        {
            string aidPath = @"..\..\..\data\RegressionTests\AID_MPB_Hybrid_UnitTest001.txt";

            if (!File.Exists(aidPath))
                throw new FileNotFoundException("The unit test file " + aidPath + " was not found.");

            // Make sure we have the full path so other referenced assemblies can find the test file.
            aidPath = Path.GetFullPath(aidPath);

            // Read the data file that holds all of the DMS related information.
            clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(aidPath);

            // We dont need to loop, since this test file should only have one analysis
            // file in it, however, we'll do this for completeness.  Find all the datasets
            // that we are interested in processing.
            foreach (clsAnalysisDescription description in reader.Analyses)
            {
                clsSequestAnalysisReader sequestData = new clsSequestAnalysisReader(description.mstrArchivePath, description.mstrDataset);

            }
        }
        /// <summary>
        /// Tests a failed AID dataset with other tests around it.
        /// </summary>
        [Test]
        [Description("Tests a failed AID dataset AID_MPB_Hybrid_UnitTest002.txt with other tests around the failed test")]
        public void FailedDataset02()
        {
            string aidPath = @"..\..\..\data\RegressionTests\AID_MPB_Hybrid_UnitTest002.txt";

            if (!File.Exists(aidPath))
                throw new FileNotFoundException("The unit test file " + aidPath + " was not found.");

            // Make sure we have the full path so other referenced assemblies can find the test file.
            aidPath = Path.GetFullPath(aidPath);

            // Read the data file that holds all of the DMS related information.
            clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(aidPath);

            // We dont need to loop, since this test file should only have one analysis
            // file in it, however, we'll do this for completeness.  Find all the datasets
            // that we are interested in processing.
            foreach (clsAnalysisDescription description in reader.Analyses)
            {
                clsSequestAnalysisReader sequestData = new clsSequestAnalysisReader(description.mstrArchivePath, description.mstrDataset);

            }
        }
        /// <summary>
        /// Tests a failed AID dataset with other tests around it.
        /// </summary>
        [Test]
        [Description("Tests a failed AID dataset AID_MPB_Hybrid_UnitTest002.txt with other tests around the failed test")]
        public void FailedDataset03()
        {
            string aidPath = @"..\..\..\data\RegressionTests\AID_MPB_Hybrid_UnitTest003.txt";

            if (!File.Exists(aidPath))
                throw new FileNotFoundException("The unit test file " + aidPath + " was not found.");

            // Make sure we have the full path so other referenced assemblies can find the test file.
            aidPath = Path.GetFullPath(aidPath);

            // Read the data file that holds all of the DMS related information.
            clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(aidPath);

            // We dont need to loop, since this test file should only have one analysis
            // file in it, however, we'll do this for completeness.  Find all the datasets
            // that we are interested in processing.
            foreach (clsAnalysisDescription description in reader.Analyses)
            {
                clsSequestAnalysisReader sequestData = new clsSequestAnalysisReader(description.mstrArchivePath, description.mstrDataset);

            }
        }
        /// <summary>
        /// Tests a failed AID dataset with other tests around it.
        /// </summary>
        [Test]
        [Description("Tests a analysis description file of several AID datasets that has been failing due to a network resource not being able to be found.")]
        public void FailedFullTests()
        {
            string aidPath = @"..\..\..\data\RegressionTests\AID_MPB_Hybrid.txt";

            if (!File.Exists(aidPath))
                throw new FileNotFoundException("The unit test file " + aidPath + " was not found.");

            // Make sure we have the full path so other referenced assemblies can find the test file.
            aidPath = Path.GetFullPath(aidPath);

            // Read the data file that holds all of the DMS related information.
            clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(aidPath);

            // We dont need to loop, since this test file should only have one analysis
            // file in it, however, we'll do this for completeness.  Find all the datasets
            // that we are interested in processing.
            foreach (clsAnalysisDescription description in reader.Analyses)
            {
                clsSequestAnalysisReader sequestData = new clsSequestAnalysisReader(description.mstrArchivePath, description.mstrDataset);

            }
        }
    }
}
