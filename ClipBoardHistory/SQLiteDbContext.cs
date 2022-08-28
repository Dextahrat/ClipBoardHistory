using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ClipBoardHistory
{
    public class SQLiteDbContext : DbContext
    {
        public DbSet<ClipBoardData>? ClipBoardDatas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "FileName=clipBoardMessages.db", sqliteOptionsAction: option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClipBoardData>().ToTable(name: "ClipboardDatas");
            modelBuilder.Entity<ClipBoardData>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
