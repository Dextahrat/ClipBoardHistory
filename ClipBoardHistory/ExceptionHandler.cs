using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipBoardHistory
{
    public class ExceptionHandler
    {

        //public static void ExceptionHandle(object sender, UnhandledExceptionEventArgs args)
        //{
        //    Exception e = (Exception)args.ExceptionObject;
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(e.Message + " " + Environment.NewLine + e.StackTrace, EventLogEntryType.Error);
        //    }
        //}

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        public static void HandleException(Exception e)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(e.Message + " " + Environment.NewLine + e.StackTrace, EventLogEntryType.Error);
            }
        }


    }
}
