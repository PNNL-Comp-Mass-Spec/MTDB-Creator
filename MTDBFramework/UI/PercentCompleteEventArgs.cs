#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class PercentCompleteEventArgs : EventArgs
    {
        protected readonly string mCurrentTask = string.Empty;

        public PercentCompleteEventArgs(float percentComplete)
        {
            PercentComplete = percentComplete;
        }

        public PercentCompleteEventArgs(float percentComplete, string currentTask)
        {
            PercentComplete = percentComplete;
            mCurrentTask = currentTask;
        }

        public PercentCompleteEventArgs(int current, int total, float percentComplete, string currentTask)
        {
            Current = current;
            Total = total;
            PercentComplete = percentComplete;
            mCurrentTask = currentTask;
        }

        public int Current { get; set; }
        public int Total { get; set; }

        public float PercentComplete { get; private set; }

        public string CurrentTask
        {
            get { return mCurrentTask; }
        }
    }

    public delegate void PercentCompleteEventHandler(object sender, PercentCompleteEventArgs e);
}


