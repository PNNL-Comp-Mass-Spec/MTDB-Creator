#region Namespaces

using System;
using System.Diagnostics;

#endregion

namespace MTDBCreator.Helpers
{
    public static class ErrorHelper
    {
        public static void WriteExceptionTraceInformation(Exception ex)
        {
            if (ex != null)
            {
                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("=============");
                Trace.WriteLine("= Exception =");
                Trace.WriteLine("=============");

                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("Type: ");
                Trace.WriteLine("------");
                Trace.WriteLine(ex.GetType().ToString());

                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("Description: ");
                Trace.WriteLine("-------------");
                Trace.WriteLine(ex.Message);

                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("Source: ");
                Trace.WriteLine("--------");
                Trace.WriteLine(ex.Source);

                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("StackTrace: ");
                Trace.WriteLine("------------");
                Trace.WriteLine(ex.StackTrace);

                Trace.WriteLine(Environment.NewLine);

                Trace.WriteLine("InnerException: ");
                Trace.WriteLine("----------------");

                if (ex.InnerException == null)
                {
                    Trace.WriteLine("null");
                }
                else
                {
                    WriteExceptionTraceInformation(ex.InnerException);
                }
            }
        }
    }
}
