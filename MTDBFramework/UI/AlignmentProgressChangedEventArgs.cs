#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class AlignmentProgressChangedEventArgs : EventArgs
    {
        public AlignmentProgressChangedEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        public int Current { get; private set; }
        public int Total { get; private set; }

    }

    public delegate void AlignmentProgressChangedEventHandler(object sender, AlignmentProgressChangedEventArgs e);
}