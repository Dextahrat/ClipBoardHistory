
namespace ClipBoardHistory
{
    public static class AppConstants
    {
        public static readonly string InstanceKey = "4251DE57040A4E3BA78BDEFCC45CE454";

        public const string DatabaseFileName = "clipBoardMessages.db";

        /// <summary>
        /// Uygulama verilerinin tutulduğu sabit klasör: %LOCALAPPDATA%\ClipBoardHistory
        /// Uygulama hangi çalışma dizininden başlatılırsa başlatılsın aynı yeri gösterir.
        /// </summary>
        public static readonly string DataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ClipBoardHistory");

        public static readonly string DatabasePath = Path.Combine(DataDirectory, DatabaseFileName);

        public static readonly string LogDirectory = Path.Combine(DataDirectory, "Logs");
    }
}
