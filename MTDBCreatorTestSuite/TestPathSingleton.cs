using System;

namespace MTDBCreatorTestSuite
{
    public static class TestPathSingleton
    {
        static TestPathSingleton()
        {
            TestDirectory = @"D:\testData\";
            var now = DateTime.Now;
            OutputDirectory = string.Format(@"D:\testResults\",
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