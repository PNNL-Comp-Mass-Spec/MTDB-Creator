#region Namespaces

using System;

#endregion

namespace MTDBFramework.UI
{
	/// <summary>
	/// Arguments for Alignment Progress updates
	/// </summary>
    public class AlignmentProgressChangedEventArgs : EventArgs
    {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="current">Current tasks completed</param>
		/// <param name="total">Total task to complete</param>
        public AlignmentProgressChangedEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

		/// <summary>
		/// Current tasks completed
		/// </summary>
        public int Current { get; private set; }

		/// <summary>
		/// Total tasks to complete
		/// </summary>
        public int Total { get; private set; }
    }

	/// <summary>
	/// Event handler
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    public delegate void AlignmentProgressChangedEventHandler(object sender, AlignmentProgressChangedEventArgs e);
}