﻿#region Namespaces

using System;

#endregion

namespace MTDBFrameworkBase.Events
{
    /// <summary>
    /// Arguments for MTDB Progress Changed events
    /// </summary>
    public class MtdbProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="current">Current tasks completed</param>
        /// <param name="total">Total tasks to complete</param>
        public MtdbProgressChangedEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="current">Current tasks completed</param>
        /// <param name="total">Total tasks to complete</param>
        /// <param name="currentTask">Name of the current task</param>
        public MtdbProgressChangedEventArgs(int current, int total, string currentTask)
        {
            Current = current;
            Total = total;
            CurrentTask = currentTask;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="current">Current tasks completed</param>
        /// <param name="total">Total tasks to complete</param>
        /// <param name="userObject">Object being processed</param>
        public MtdbProgressChangedEventArgs(int current, int total, object userObject)
            : this(current, total)
        {
            UserObject = userObject;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="current">Current tasks completed</param>
        /// <param name="total">Total tasks to complete</param>
        /// <param name="currentTask">Name of the current task</param>
        /// <param name="userObject">Object being processed</param>
        public MtdbProgressChangedEventArgs(int current, int total, string currentTask, object userObject)
            : this(current, total, currentTask)
        {
            UserObject = userObject;
        }

        /// <summary>
        /// Current tasks completed
        /// </summary>
        public int Current { get; }

        /// <summary>
        /// Total tasks to complete
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Name of currently running task
        /// </summary>
        public string CurrentTask { get; }

        /// <summary>
        /// Object that is currently running
        /// </summary>
        public object UserObject { get; }
    }

    /// <summary>
    /// Event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MtdbProgressChangedEventHandler(object sender, MtdbProgressChangedEventArgs e);
}