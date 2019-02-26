using System.IO;

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
        /// Absolute path to the MTDBCreatorTestSuite\TestData directory
        /// </summary>
        private readonly string m_testSuiteDataDir;

        protected TestBase()
        {
            TextDelimiter = ",";
            m_pathBase = TestPathSingleton.TestDirectory;
            m_outputPathBase = TestPathSingleton.OutputDirectory;
            TestSuiteDir = TestPathSingleton.TestSuiteDirectory;
            m_testSuiteDataDir = TestPathSingleton.TestSuiteTestDataDirectory;
            ProjectDir = TestPathSingleton.ProjectDirectory;
        }
        /// <summary>
        /// Resolves the absolute path for a given relative test data path.
        /// </summary>
        /// <param name="path">Relative path to source.</param>
        /// <returns>Absolute path based on common test data directory path.</returns>
        protected string GetPath(string path)
        {
            return Path.Combine(m_pathBase, path);
        }

        protected string GetOutputPath(string path)
        {
            return Path.Combine(m_outputPathBase, path);
        }

        protected string GetTestSuiteDataPath(string path)
        {
            return Path.Combine(m_testSuiteDataDir, path);
        }

        /// <summary>
        /// Absolute path to the Project/Solution Directory
        /// </summary>
        protected string ProjectDir { get; }

        /// <summary>
        /// Absolute path to the MTDBCreatorTestSuite directory
        /// </summary>
        protected string TestSuiteDir { get; }

        protected string TextDelimiter { get; }

    }
}