#region Namespaces

using System;

#endregion

namespace MTDBCreator.Commands
{
    public class MTDBResultChangedEventArgs : EventArgs
    {
        public MTDBResultChangedEventArgs(object result)
            : base()
        {
            this.Result = result;
        }

        public MTDBResultChangedEventArgs(object result, object userObject)
            : this(result)
        {
            this.UserObject = userObject;
        }

        public object Result { get; private set; }
        public object UserObject { get; private set; }
    }

    public delegate void MTDBResultChangedEventHandler(object sender, MTDBResultChangedEventArgs e);
}