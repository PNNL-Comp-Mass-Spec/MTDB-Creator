#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class MtdbProgressChangedEventArgs : EventArgs
    {
        public MtdbProgressChangedEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        public MtdbProgressChangedEventArgs(int current, int total, object userObject)
            : this(current, total)
        {
            UserObject = userObject;
        }

        public int Current { get; private set; }
        public int Total { get; private set; }
        public object UserObject { get; private set; }
    }

    public delegate void MtdbProgressChangedEventHandler(object sender, MtdbProgressChangedEventArgs e);
}