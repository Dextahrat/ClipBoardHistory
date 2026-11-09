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

            using var dbcontext = new SQLiteDbContext();
            dbcontext.Database.EnsureCreated();

            // EnsureCreated var olan veritabanına yeni indeks eklemez; eski veritabanları için elle eklenir.
            dbcontext.Database.ExecuteSqlRaw(
                "CREATE INDEX IF NOT EXISTS \"" + CreateDateIndexName + "\" ON \"ClipboardDatas\" (\"CreateDate\");");
        }

        /// <summary>
        /// Önceki sürümler veritabanını çalışma dizinine (genellikle exe'nin yanına) yazıyordu.
        /// Yeni konumda veritabanı yoksa ve eski konumlardan birinde varsa oradan kopyalanır.
        /// Eski dosya silinmez, yedek olarak yerinde kalır.
        /// </summary>
        private static void CopyLegacyDatabaseIfNeeded()
        {
            if (File.Exists(AppConstants.DatabasePath))
                return;

            var legacyPath = new[] { AppContext.BaseDirectory, Environment.CurrentDirectory }
                .Select(dir => Path.GetFullPath(Path.Combine(dir, AppConstants.DatabaseFileName)))
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
        }
    }
}
