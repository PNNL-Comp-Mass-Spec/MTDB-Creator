#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class PercentCompleteEventArgs : EventArgs
    {
        public PercentCompleteEventArgs(float percentComplete)
        {
            PercentComplete = percentComplete;
        }

        public float PercentComplete { get; private set; }
    }

    public delegate void PercentCompleteEventHandler(object sender, PercentCompleteEventArgs e);
}


