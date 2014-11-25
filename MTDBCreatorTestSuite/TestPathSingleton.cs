using System;

namespace MTDBCreatorTestSuite
{
    public static class TestPathSingleton
    {
        static TestPathSingleton()
        {
            TestDirectory = @"\\proto-2\UnitTest_Files\MTDBCreator\";
            var now = DateTime.Now;
            OutputDirectory = string.Format(@"C:\MTDBCreatorTestResults\{0}_{1}_{2}_{3}_{4}_{5}",
                                                now.Year,
                                                now.Month,
                                                now.Day,
                                                now.Hour,
                                                now.Minute,
                                                now.Second);

			// The Execution directory
			var dirFinder = Environment.CurrentDirectory;
			// Find the bin folder...
	        while (!string.IsNullOrWhiteSpace(dirFinder) && !dirFinder.EndsWith("bin"))
	        {
				//Console.WriteLine("Project: " + dirFinder);
		        dirFinder = System.IO.Path.GetDirectoryName(dirFinder);
	        }
			//Console.WriteLine("Project: " + dirFinder);
			// The Directory for MTDBCreatorTestSuite
			TestSuiteDirectory = System.IO.Path.GetDirectoryName(dirFinder);
			// The TestSuite\TestData Directory
			TestSuiteTestDataDirectory = System.IO.Path.Combine(TestSuiteDirectory, "TestData");
			// The Project/Solution Directory
			ProjectDirectory = System.IO.Path.GetDirectoryName(TestSuiteDirectory);
			//Console.WriteLine("TestSuite: " + TestSuiteDirectory);
			//Console.WriteLine("TestSuiteTestData: " + TestSuiteTestDataDirectory);
			//Console.WriteLine("ProjectDir: " + ProjectDirectory);
        }

        public static string TestDirectory
        {
            get;
            private set;
        }

        public static string OutputDirectory
        {
            get;
            private set;
        }

		public static string TestSuiteDirectory
		{
			get;
			private set;
		}

		public static string TestSuiteTestDataDirectory
		{
			get;
			private set;
		}

		public static string ProjectDirectory
		{
			get;
			private set;
		}
    }
}