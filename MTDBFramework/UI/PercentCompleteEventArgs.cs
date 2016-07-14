#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    /// <summary>
    /// Arguments for percent completion display
    /// </summary>
    public class PercentCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Currently running task
        /// </summary>
        private readonly string m_currentTask = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percentComplete">Percent complete</param>
        public PercentCompleteEventArgs(float percentComplete)
        {
            PercentComplete = percentComplete;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percentComplete">Percent complete</param>
        /// <param name="currentTask">Currently running task</param>
        public PercentCompleteEventArgs(float percentComplete, string currentTask)
        {
            PercentComplete = percentComplete;
            m_currentTask = currentTask;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="current">Current subtasks completed</param>
        /// <param name="total">Total subtasks</param>
        /// <param name="percentComplete">Percent complete</param>
        /// <param name="currentTask">Currently running task</param>
        public PercentCompleteEventArgs(int current, int total, float percentComplete, string currentTask)
        {
            Current = current;
            Total = total;
            PercentComplete = percentComplete;
            m_currentTask = currentTask;
        }

        /// <summary>
        /// Current subtasks completed
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Total subtasks
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Percent completion
        /// </summary>
        public float PercentComplete { get; private set; }

        /// <summary>
        /// Name of currently running task
        /// </summary>
        public string CurrentTask
        {
            get { return m_currentTask; }
        }
    }

    /// <summary>
    /// Event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PercentCompleteEventHandler(object sender, PercentCompleteEventArgs e);
}


