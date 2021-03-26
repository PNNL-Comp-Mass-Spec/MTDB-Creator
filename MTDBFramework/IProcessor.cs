using MTDBFrameworkBase.Data;

namespace MTDBFramework
{
    /// <summary>
    /// Interface for processing management
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Options
        /// </summary>
        Options ProcessorOptions { get; set; }

        /// <summary>
        /// Support thread cancellation
        /// </summary>
        void AbortProcessing();
    }
}