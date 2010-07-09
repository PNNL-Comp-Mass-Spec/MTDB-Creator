using System;
using System.Collections.Generic;
using System.Text;

namespace MTDBCreator
{
    /// <summary>
    /// Base requirements for processing results files for creating a MTDB.
    /// </summary>
    public abstract class ProcessorBase
    {
        public event DelegateSetPercentComplete PercentCompleted;
        public event DelegateSetStatusMessage   Status;
        public event DelegateSetErrorMessage    Error;

        protected virtual void StatusMessage(string message)
        {
            if (Status != null)
                Status(message);
        }
        protected virtual void PercentComplete(int percent)
        {
            if (PercentCompleted != null)
                PercentCompleted(percent);
        }
        protected virtual void ErrorMessage(string message)
        {
            if (Error != null)
                Error(message);
        }
        /// <summary>
        /// Registers the status and other events from another reference through this objects events.        
        /// </summary>
        /// <param name="child"></param>
        protected virtual void RegisterProcessing(ProcessorBase child)
        {
            child.Status            += new DelegateSetStatusMessage(StatusMessage);
            child.PercentCompleted  += new DelegateSetPercentComplete(PercentComplete);
            child.Error             += new DelegateSetErrorMessage(ErrorMessage);
        }
    }
}
