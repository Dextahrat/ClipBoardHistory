using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using System.Reflection;

namespace ClipBoardHistory
{
    public class SQLiteDbContext : DbContext
    {
        /// <summary>
        /// Tarihlerin veritabanında saklandığı biçim. Bu biçim metin olarak sıralandığında
        /// kronolojik sırayı verir; bu sayede tarih karşılaştırmaları doğrudan SQL'de yapılabilir.
        /// Mevcut veritabanlarındaki kayıtlar da bu biçimde olduğu için veri dönüşümü gerekmez.
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        private const string CreateDateIndexName = "IX_ClipboardDatas_CreateDate";

        public DbSet<ClipBoardData>? ClipBoardDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = AppConstants.DatabasePath
            }.ToString();

            optionsBuilder.UseSqlite(connectionString, sqliteOptionsAction: option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateConverter = new ValueConverter<DateTime, string>(
                v => v.ToString(DateFormat, CultureInfo.InvariantCulture),
                v => DateTime.ParseExact(v, DateFormat, CultureInfo.InvariantCulture));

            var entity = modelBuilder.Entity<ClipBoardData>();
            entity.ToTable(name: "ClipboardDatas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CreateDate).HasConversion(dateConverter);
            entity.HasIndex(x => x.CreateDate).HasDatabaseName(CreateDateIndexName);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Uygulama açılışında bir kez çağrılır: veri klasörünü oluşturur, eski konumdaki
        /// veritabanını yeni konuma kopyalar, tabloyu ve tarih indeksini garanti eder.
        /// </summary>
        public static void Initialize()
        {
            Directory.CreateDirectory(AppConstants.DataDirectory);
            CopyLegacyDatabaseIfNeeded();

            Directory.CreateDirectory(AppConstants.ImageDirectory);

            using var dbcontext = new SQLiteDbContext();
            dbcontext.Database.EnsureCreated();

            // EnsureCreated var olan tabloya yeni sütun eklemez; eski veritabanlarına eksik sütunlar elle eklenir.
            // Sütunlar boş geçilebilir olduğu için mevcut kayıtlar değişmez.
            AddMissingColumns(dbcontext, "Rtf", "Html", "ImagePath");

            // EnsureCreated var olan veritabanına yeni indeks eklemez; eski veritabanları için elle eklenir.
            dbcontext.Database.ExecuteSqlRaw(
                "CREATE INDEX IF NOT EXISTS \"" + CreateDateIndexName + "\" ON \"ClipboardDatas\" (\"CreateDate\");");
        }

        /// <summary>
        /// Verilen listede resim dosyası diskte bulunmayan kayıtları veritabanından siler ve listeden çıkarır.
        /// Hata fırlatmaz; bir sorun olursa sadece loglar. Silinen kayıt olduysa true döner.
        /// </summary>
        public static bool RemoveRecordsWithMissingImages(List<ClipBoardData> records)
        {
            var missingIds = records
                .Where(x => x.IsImage && !File.Exists(x.ImageFullPath!))
                .Select(x => x.Id)
                .ToList();

            if (missingIds.Count == 0)
                return false;

            try
            {
                using var dbcontext = new SQLiteDbContext();
                dbcontext.ClipBoardDatas!.Where(x => missingIds.Contains(x.Id)).ExecuteDelete();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return false;
            }

            records.RemoveAll(x => missingIds.Contains(x.Id));
            return true;
        }

        private static void AddMissingColumns(SQLiteDbContext dbcontext, params string[] columnNames)
        {
            var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var connection = dbcontext.Database.GetDbConnection();
            dbcontext.Database.OpenConnection();
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "PRAGMA table_info(\"ClipboardDatas\");";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    existingColumns.Add(reader.GetString(1)); // 1 = sütun adı
            }
            finally
            {
                dbcontext.Database.CloseConnection();
            }

            foreach (var columnName in columnNames)
            {
                if (!existingColumns.Contains(columnName))
                    dbcontext.Database.ExecuteSqlRaw("ALTER TABLE \"ClipboardDatas\" ADD COLUMN \"" + columnName + "\" TEXT NULL;");
            }
        }

        /// <summary>
        /// Exe'nin yanında veritabanı yoksa eski konumlardan birinde aranır ve oradan kopyalanır:
        /// - Çalışma dizini (ilk sürümler veritabanını göreli yolla buraya yazıyordu)
        /// - %LOCALAPPDATA%\ClipBoardHistory (bir önceki geliştirme sürümü; resimleri de kopyalanır)
        /// Eski dosyalar silinmez, yedek olarak yerinde kalır.
        /// </summary>
        private static void CopyLegacyDatabaseIfNeeded()
        {
            if (File.Exists(AppConstants.DatabasePath))
                return;

            var legacyPath = new[] { Environment.CurrentDirectory, AppConstants.LegacyLocalDataDirectory }
                .Select(dir => Path.GetFullPath(Path.Combine(dir, AppConstants.DatabaseFileName)))
                .Where(path => !string.Equals(path, AppConstants.DatabasePath, StringComparison.OrdinalIgnoreCase))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(path => File.Exists(path))
                .OrderByDescending(path => File.GetLastWriteTimeUtc(path))
                .FirstOrDefault();

            if (legacyPath == null)
                return;

            // SQLite WAL modundaysa yardımcı dosyalar da taşınmalı.
            foreach (var suffix in new[] { "", "-wal", "-shm" })
            {
                var source = legacyPath + suffix;
                if (File.Exists(source))
                    File.Copy(source, AppConstants.DatabasePath + suffix, overwrite: false);
            }

            // Veritabanı %LOCALAPPDATA%'dan geldiyse oradaki resimler de exe'nin yanına kopyalanır.
            var legacyImageDirectory = Path.Combine(Path.GetDirectoryName(legacyPath)!, "Images");
            if (Directory.Exists(legacyImageDirectory))
            {
                Directory.CreateDirectory(AppConstants.ImageDirectory);
                foreach (var source in Directory.GetFiles(legacyImageDirectory, "*.png"))
                {
                    var target = Path.Combine(AppConstants.ImageDirectory, Path.GetFileName(source));
                    if (!File.Exists(target))
                        File.Copy(source, target);
                }
            }
        }
    }
}
