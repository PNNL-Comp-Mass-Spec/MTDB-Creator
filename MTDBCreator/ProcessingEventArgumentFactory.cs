using System;
using System.Collections.Generic;
using System.Text;

namespace MTDBCreator
{     
    /// <summary>
    /// Event arguments for status messages.
    /// </summary>
    public class ProcessingStatusEventArgs : EventArgs
    {
        public ProcessingStatusEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets or sets the message to be displayed.
        /// </summary>
        public string Message { get; set; }
    }
    /// <summary>
    /// Event arguments for status messages.
    /// </summary>
    public class ProcessingErrorEventArgs : EventArgs
    {
        public ProcessingErrorEventArgs(string message, Exception ex)
        {
            Message     = message;
            Exception   = ex;
        }

        /// <summary>
        /// Gets or sets the message to be displayed.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception, if applicable.
        /// </summary>
        public Exception Exception{get;set;}
    }
    /// <summary>
    /// Analysis complete event arguments.
    /// </summary>
    public class ProcessingCompleteEventArgs : EventArgs
    {
        public ProcessingCompleteEventArgs(string message, clsMTDB database, List<string> failed)
        {
            Message  = message;
            Database = database;
            FailedDatasets = failed;
        }
        public List<string> FailedDatasets { get; set; }

        /// <summary>
        /// Gets or sets the message to be displayed.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception, if applicable.
        /// </summary>
        public clsMTDB Database {get; set; }
    }    
}
