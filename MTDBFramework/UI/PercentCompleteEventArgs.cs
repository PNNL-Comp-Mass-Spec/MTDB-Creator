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


        public float PercentComplete { get; private set; }

        public string CurrentTask
        {
            get { return mCurrentTask; }
        }
    }

    public delegate void PercentCompleteEventHandler(object sender, PercentCompleteEventArgs e);
}


