using System.Threading;

namespace ClipBoardHistory
{
    internal static class Program
    {
        private static int tryInstanceCount = 0;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            const string appName = "ClipBoardHistory";//test
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, appName, out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    ApplicationConfiguration.Initialize();
                    Application.Run(new MainForm()); 
                }
                else
                {
                    SendActivateCurrentInstanceMessage();
                }
            } 
        }

        private static void SendActivateCurrentInstanceMessage()
        {
            try
            {
                Clipboard.SetDataObject(AppConstants.InstanceKey,true,2,100);
            }
            catch
            {
                if (tryInstanceCount > 2)
                {
                    MessageBox.Show("Application allready running!");
                    return;
                }
                    
                tryInstanceCount++;
                SendActivateCurrentInstanceMessage();

            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        } 
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }

    }
}