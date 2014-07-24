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
			foreach (var path in paths)
			{
				if (File.Exists(GetTestSuiteDataPath(path)))
				{
					File.Delete(GetTestSuiteDataPath(path));
				}
			}
		}

		[Test]
		[TestCase(@"Output2.mtdb", @"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", "", Ignore = false)]
		[TestCase(@"Output3.mtdb", @"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", @"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", Ignore = false)]
		public void TestWriteMZID(string outputPath, params string[] paths)
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
			MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
		}

		[Test]
		[TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		[TestCase(@"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		[TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		public void TestAppendMZID(string path, string outputPath)
		{
			var testPaths = new List<string>();
			testPaths.Add(GetPath(path));
			MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
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
			Assert.AreEqual(atomic.ConsensusTargets.Count, appended.ConsensusTargets.Count);
		}

		[TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		public void TestAppendDuplicateMZID(string path, string outputPath)
		{
			var testPaths = new List<string>();
			testPaths.Add(GetPath(path));
			MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
			MTDBFramework.MtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
		}
    }
}