
namespace ClipBoardHistory
{
    public static class AppConstants
    {
        public static readonly string InstanceKey = "4251DE57040A4E3BA78BDEFCC45CE454";

        public const string DatabaseFileName = "clipBoardMessages.db";

        /// <summary>
        /// Uygulama verilerinin tutulduğu klasör: exe'nin bulunduğu klasör (taşınabilir kullanım).
        /// Uygulama klasörü başka bir bilgisayara kopyalandığında veritabanı ve resimler de birlikte taşınır.
        /// Mutlak yoldur; uygulama hangi çalışma dizininden başlatılırsa başlatılsın aynı yeri gösterir.
        /// </summary>
        public static readonly string DataDirectory = Path.GetFullPath(AppContext.BaseDirectory);

        /// <summary>
        /// Bir önceki geliştirme sürümünün kullandığı klasör (%LOCALAPPDATA%\ClipBoardHistory).
        /// Sadece oradaki verileri exe'nin yanına geri kopyalamak için kullanılır.
        /// </summary>
        public static readonly string LegacyLocalDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ClipBoardHistory");

        public static readonly string DatabasePath = Path.Combine(DataDirectory, DatabaseFileName);

        public static readonly string LogDirectory = Path.Combine(DataDirectory, "Logs");

        /// <summary>
        /// Kopyalanan resimler bu klasörde PNG olarak saklanır; veritabanında sadece dosya adı tutulur.
        /// Dosya adı resmin SHA-256 özetidir, böylece aynı resim diske bir kez yazılır.
        /// </summary>
        public static readonly string ImageDirectory = Path.Combine(DataDirectory, "Images");

        /// <summary>
        /// RTF/HTML biçimli metin bu uzunluktan büyükse (örn. içine resim gömülmüş Word içerikleri)
        /// biçim saklanmaz, sadece düz metin kaydedilir. Veritabanının şişmesini önler.
        /// </summary>
        public const int MaxRichTextLength = 1_000_000;
    }
}
