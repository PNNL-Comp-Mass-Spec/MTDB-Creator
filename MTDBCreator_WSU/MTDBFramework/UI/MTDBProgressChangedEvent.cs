#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class MTDBProgressChangedEventArgs : EventArgs
    {
        public MTDBProgressChangedEventArgs(int current, int total)
            : base()
        {
            this.Current = current;
            this.Total = total;
        }

        public MTDBProgressChangedEventArgs(int current, int total, object userObject)
            : this(current, total)
        {
            this.UserObject = userObject;
        }

        public int Current { get; private set; }
        public int Total { get; private set; }
        public object UserObject { get; private set; }
    }

    public delegate void MTDBProgressChangedEventHandler(object sender, MTDBProgressChangedEventArgs e);
}