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
			paths.Add("Output2.mtdb");
			paths.Add("Output3.mtdb");
			paths.Add("Output4.mtdb");
			foreach (var path in paths)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}

		[Test]
		[TestCase(@"Output2.mtdb", @"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", "", Ignore = false)]
		[TestCase(@"Output3.mtdb", @"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", @"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", Ignore = false)]
		public void TestWriteMZID(string outputPath, params string[] paths)
		{
			var mtdbCreator = new MTDBFramework.MtdbCreator();
			var testPaths = new List<string>();
			foreach (var path in paths)
			{
				if (!string.IsNullOrWhiteSpace(path))
				{
					testPaths.Add(GetPath(path));
				}
			}
			mtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
		}

		[Test]
		[TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		[TestCase(@"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		[TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		[TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", @"Output4.mtdb", Ignore = false)]
		public void TestAppendMZID(string path, string outputPath)
		{
			var mtdbCreator = new MTDBFramework.MtdbCreator();
			var testPaths = new List<string>();
			testPaths.Add(GetPath(path));
			mtdbCreator.CreateDB(testPaths, GetTestSuiteDataPath(outputPath));
		}
    }
}