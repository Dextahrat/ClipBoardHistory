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
        private static readonly object _logFileLock = new object();

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        /// <summary>
        /// Hatayı önce %LOCALAPPDATA%\ClipBoardHistory\Logs altındaki günlük dosyasına yazar
        /// (yönetici izni gerektirmez), ardından mümkünse Windows Olay Görüntüleyicisi'ne de yazar.
        /// Loglama sırasında oluşan hiçbir hata uygulamayı çökertmez.
        /// </summary>
        public static void HandleException(Exception e)
        {
            WriteToLogFile(e);
            WriteToEventLog(e);
        }

        private static void WriteToLogFile(Exception e)
        {
            try
            {
                Directory.CreateDirectory(AppConstants.LogDirectory);
                var now = DateTime.Now;
                var logPath = Path.Combine(AppConstants.LogDirectory, "error-" + now.ToString("yyyy-MM-dd") + ".log");
                var entry = "[" + now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + e + Environment.NewLine + Environment.NewLine;

                lock (_logFileLock)
                {
                    File.AppendAllText(logPath, entry);
                }
            }
            catch
            {
                // Loglama başarısız olsa bile uygulama çalışmaya devam etmeli.
            }
        }

        private static void WriteToEventLog(Exception e)
        {
            try
            {
                // Kaynak kayıtlı değilse Windows onu oluşturmaya çalışır ve bu yönetici izni ister.
                // Yetki yoksa sessizce geçilir; hata zaten dosyaya yazılmıştır.
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(e.Message + " " + Environment.NewLine + e.StackTrace, EventLogEntryType.Error);
                }
            }
            catch
            {
            }
        }


    }
}
