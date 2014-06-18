#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
    public class MtdbProgressChangedEventArgs : EventArgs
    {
        protected readonly string mCurrentTask = string.Empty;

        public MtdbProgressChangedEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        public MtdbProgressChangedEventArgs(int current, int total, string currentTask)
        {
            Current = current;
            Total = total;
            mCurrentTask = currentTask;
        }

        public MtdbProgressChangedEventArgs(int current, int total, object userObject)
            : this(current, total)
        {
            UserObject = userObject;
        }

        public MtdbProgressChangedEventArgs(int current, int total, string currentTask, object userObject)
            : this(current, total, currentTask)
        {
            UserObject = userObject;
        }

        public int Current { get; private set; }
        public int Total { get; private set; }

        public string CurrentTask
        {
            get { return mCurrentTask; }
        }

        public object UserObject { get; private set; }
    }

    public delegate void MtdbProgressChangedEventHandler(object sender, MtdbProgressChangedEventArgs e);
}