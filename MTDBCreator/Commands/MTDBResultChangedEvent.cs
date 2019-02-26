#region Namespaces

using System;

#endregion

namespace MTDBCreator.Commands
{
    public class MtdbResultChangedEventArgs : EventArgs
    {
        public MtdbResultChangedEventArgs(object result)
        {
            Result = result;
        }

        public MtdbResultChangedEventArgs(object result, object userObject)
            : this(result)
        {
            UserObject = userObject;
        }

        public object Result { get; }
        public object UserObject { get; }
    }

    public delegate void MtdbResultChangedEventHandler(object sender, MtdbResultChangedEventArgs e);
}