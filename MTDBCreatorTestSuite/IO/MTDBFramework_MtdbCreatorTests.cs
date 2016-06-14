using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
	[TestFixture]
	class MTDBFramework_MtdbCreatorTests : TestBase
	{
		[TestFixtureSetUp]
		public void DeleteOutputFiles()
		{
			var paths = new List<string>();
			paths.Add(@"Output2.mtdb");
			paths.Add(@"Output3.mtdb");
			paths.Add(@"Output4.mtdb");
			paths.Add(@"Output5.mtdb");
			paths.Add(@"Output6.mtdb");
			paths.Add(@"Output7.mtdb");
			foreach (var path in paths)
			{
				if (File.Exists(GetTestSuiteDataPath(path)))
				{
					File.Delete(GetTestSuiteDataPath(path));
				}
			}
		}

		[Test]
		[TestCase(@"Output2.mtdb", 579, @"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", "", Ignore = false)]
		[TestCase(@"Output3.mtdb", 13076, @"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", @"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", Ignore = false)]
		public void TestWriteMZID(string outputPath, int expectedConsensusTargets, params string[] paths)
		{
			//var mtdbCreator = new MTDBFramework.MtdbCreator();
			var testPaths = new List<string>();
			foreach (var path in paths)
			{
				if (!string.IsNullOrWhiteSpace(path))
				{
					testPaths.Add(GetPath(path));
				}
			}
			var db = MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
            Assert.AreEqual(expectedConsensusTargets, db.ConsensusTargets.Count);
		}

	    [Test]
	    public void TestAppendMZID()
	    {
	        var file1 = @"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid";
	        var file2 = @"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid";
            var file3 = @"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid";
	        var path1 = GetPath(file1);
	        var path2 = GetPath(file2);
	        var path3 = GetPath(file3);
	        var outputPath = GetTestSuiteDataPath(@"Output4.mtdb");
            Console.WriteLine("Adding file \"{0}\" to the database...", file1);
	        var db = MTDBFramework.MtdbCreator.CreateDB(new List<string>() {path1}, outputPath);
            Console.WriteLine("Consensus Targets Count for file should be: 579");
            Console.WriteLine("Consensus Targets Count (total) should be: 579");
            Assert.AreEqual(579, db.ConsensusTargets.Count);

            Console.WriteLine("Adding file \"{0}\" to the database...", file2);
            db = MTDBFramework.MtdbCreator.CreateDB(new List<string>() { path2 }, outputPath);
            Console.WriteLine("Consensus Targets Count for file should be: 10360");
	        Console.WriteLine("Consensus Targets Count (total) should be: 10937");
            Assert.AreEqual(10937, db.ConsensusTargets.Count);

            Console.WriteLine("Adding file \"{0}\" to the database...", file3);
            db = MTDBFramework.MtdbCreator.CreateDB(new List<string>() { path3 }, outputPath);
            Console.WriteLine("Consensus Targets Count for file should be: 10410");
	        Console.WriteLine("Consensus Targets Count (total) should be: 13653");
            Assert.AreEqual(13653, db.ConsensusTargets.Count);
	    }

	    [Test]
		public void CheckAppendMZID()
		{
			var inPath1 = @"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid";
			var inPath2 = @"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid";
			var outPath1 = @"Output5.mtdb";
			var outPath2 = @"Output6.mtdb";
			var paths1 = new List<string>();
			var paths2 = new List<string>();
			paths1.Add(GetPath(inPath1));
			paths2.Add(GetPath(inPath2));
			Console.WriteLine("Creating DB1 from Data1");
			MTDBFramework.MtdbCreator.CreateDB(paths1, GetTestSuiteDataPath(outPath1));
			Console.WriteLine("Appending Data2 to DB1");
			var appended = MTDBFramework.MtdbCreator.CreateDB(paths2, GetTestSuiteDataPath(outPath1));
			paths1.Add(GetPath(inPath2));
			Console.WriteLine("Creating DB2 from Data1 and Data2");
			var atomic = MTDBFramework.MtdbCreator.CreateDB(paths1, GetTestSuiteDataPath(outPath2));
            Console.WriteLine("Consensus Targets Count should be: 10937");
            Console.WriteLine("Atomic Creation Consensus Targets: " + atomic.ConsensusTargets.Count);
            Console.WriteLine("Appended Creation Consensus Targets: " + appended.ConsensusTargets.Count);
			Assert.AreEqual(atomic.ConsensusTargets.Count, appended.ConsensusTargets.Count);
		}

		[TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", @"Output7.mtdb", Ignore = false)]
		public void TestAppendDuplicateMZID(string path, string outputPath)
		{
			var testPaths = new List<string>();
			testPaths.Add(GetPath(path));
			Console.WriteLine("Running dataset: First time");
			var target1 = MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
		    var target1Count = target1.ConsensusTargets.Count;
			Console.WriteLine("Running dataset: Second time");
            var target2 = MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
            Console.WriteLine("Consensus Target Count should be: 10410");
            Console.WriteLine("Initial Creation Consensus Targets: " + target1Count);
            Console.WriteLine("Appended Consensus Targets: " + target2.ConsensusTargets.Count);
			// The number of consensus targets should not increase
            Assert.AreEqual(target1Count, target2.ConsensusTargets.Count);
		}
    }
}