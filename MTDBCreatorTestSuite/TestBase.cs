namespace MTDBCreatorTestSuite
{
    /// <summary>
    /// Base class to assist resolving test data
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Absolute base path to main (root) test data directory.
        /// </summary>
        private readonly string m_pathBase;
        /// <summary>
        /// Absolute base path to a new test folder directory.
        /// </summary>
        private readonly string m_outputPathBase;

        /// <summary>
        /// Absolute path to the MTDBCreatorTestSuite directory
        /// </summary>
        private readonly string m_testSuiteDir;

        /// <summary>
        /// Absolute path to the MTDBCreatorTestSuite\TestData directory
        /// </summary>
        private readonly string m_testSuiteDataDir;

        /// <summary>
        /// Absolute path to the Project/Solution Directory
        /// </summary>
        private readonly string m_projectDir;

        protected TestBase()
        {
            TextDelimiter = ",";
            m_pathBase = TestPathSingleton.TestDirectory;
            m_outputPathBase = TestPathSingleton.OutputDirectory;
            m_testSuiteDir = TestPathSingleton.TestSuiteDirectory;
            m_testSuiteDataDir = TestPathSingleton.TestSuiteTestDataDirectory;
            m_projectDir = TestPathSingleton.ProjectDirectory;
        }
        /// <summary>
        /// Resolves the absolute path for a given relative test data path.
        /// </summary>
        /// <param name="path">Relative path to source.</param>
        /// <returns>Absolute path based on common test data directory path.</returns>
        protected string GetPath(string path)
        {
            return System.IO.Path.Combine(m_pathBase, path);
        }

        protected string GetOutputPath(string path)
        {
            return System.IO.Path.Combine(m_outputPathBase, path);
        }

        protected string GetTestSuiteDataPath(string path)
        {
            return System.IO.Path.Combine(m_testSuiteDataDir, path);
        }

        protected string ProjectDir => m_projectDir;

        protected string TestSuiteDir => m_testSuiteDir;

        protected string TextDelimiter
        {
            get;
            private set;
        }
    }
}