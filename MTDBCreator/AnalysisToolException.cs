using System;

namespace MTDBCreator
{
    /// <summary>
    /// Exception thrown by the analysis tool.
    /// </summary>
    public class AnalysisToolException : System.Exception
    {
        public AnalysisToolException(string message)
            : base(message)
        {
        }
    }
}
