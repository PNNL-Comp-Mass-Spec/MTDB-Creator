using System;

namespace MTDBCreatorTestSuite
{
    public static class TestPathSingleton
    {
        static TestPathSingleton()
        {
            TestDirectory = @"\\protoapps\UserData\DegantestData\";
            var now = DateTime.Now;
            OutputDirectory = string.Format(@"C:\MTDBCreatorTestResults\{0}_{1}_{2}_{3}_{4}_{5}",
                                                now.Year,
                                                now.Month,
                                                now.Day,
                                                now.Hour,
                                                now.Minute,
                                                now.Second);
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
    }
}