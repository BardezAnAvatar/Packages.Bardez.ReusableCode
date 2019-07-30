using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Extension class for retrieving additional common info on Exception instances</summary>
    public static class ExceptionExtension
    {
        /// <summary>
        ///     Gets the Exception type stack, indicating the innermost Exception type first,
        ///     followed by those containing it.
        /// </summary>
        /// <param name="ex">Exception to extract data from</param>
        /// <returns>The Exception Type stack</returns>
        public static String GetExceptionTypeStack(this Exception ex)
        {
            StringBuilder ets = new StringBuilder();

            if (ex.InnerException != null)
            {
                ets.AppendLine(ExceptionExtension.GetExceptionTypeStack(ex.InnerException));
                ets.AppendLine("---inside of:---");
            }

            ets.Append(ex.GetType().ToString());

            return ets.ToString();
        }

        /// <summary>
        ///     Gets the Exception message stack, indicating the innermost Exception message first,
        ///     followed by those containing it.
        /// </summary>
        /// <param name="ex">Exception to extract data from</param>
        /// <returns>The Exception message stack</returns>
        public static String GetExceptionMessageStack(this Exception ex)
        {
            StringBuilder message = new StringBuilder();

            if (ex.InnerException != null)
            {
                message.AppendLine(ExceptionExtension.GetExceptionMessageStack(ex.InnerException));
                message.AppendLine("---Next Message---");
            }

            message.Append(ex.Message);

            return message.ToString();
        }

        /// <summary>Gets the name of the method that is throwing the Exception</summary>
        /// <param name="ex">Exception to extract data from</param>
        /// <returns>Name of the method that is throwing the Exception</returns>
        public static String GetExceptionCallingMethod(this Exception ex)
        {
            String method = null;

            StackTrace stackTrace = new StackTrace(ex);
            StackFrame stackFrame = stackTrace.GetFrame(1);

            if (stackFrame == null)
                method = "Cannot retrieve calling method name; stackFrame is null";
            else
            {
                MethodBase methodBase = stackFrame.GetMethod();
                method =  (methodBase == null) ? "Cannot retrieve calling method name; methodBase is null" : (methodBase.Name ?? String.Empty);
            }

            return method;
        }

        /// <summary>Recursively gets the stack trace of this exception and its inner exceptions</summary>
        /// <param name="ex">Exception to extract data from</param>
        /// <returns>The complete stack trace listing</returns>
        public static String GetExceptionStackTrace(this Exception ex)
        {
            StringBuilder message = new StringBuilder();

            if (ex.InnerException != null)
            {
                message.AppendLine(ExceptionExtension.GetExceptionStackTrace(ex.InnerException));
                message.AppendLine("---inside of:---");
            }

            message.Append(ex.StackTrace);

            return message.ToString();
        }

        /// <summary>Builds consistent message for error logging.</summary>
        /// <param name="ex">Exception to build a message for</param>
        /// <returns>A StringBuilder object reference</returns>
        public static String GetExceptionMessageForLog(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Method: ");
            sb.AppendLine(ex.GetExceptionCallingMethod());

            // Append exception Type stack;
            sb.Append("| ExceptionType: ");
            sb.AppendLine(ex.GetExceptionTypeStack());

            // Append full error message;
            sb.Append("| Error: ");
            sb.AppendLine(ex.GetExceptionMessageStack());

            //append the whole tree of exceptions
            sb.AppendLine("| Stack Trace: ");
            sb.AppendLine(ex.GetExceptionStackTrace());

            return sb.ToString();
        }
    }
}