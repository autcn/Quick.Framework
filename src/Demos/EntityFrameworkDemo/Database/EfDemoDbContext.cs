using Microsoft.EntityFrameworkCore;
using EntityFrameworkDemo.Model;
using Quick;

namespace EntityFrameworkDemo.Database
{
    //基于Ef的特点， DbContext不能跨线程，不建议设置为Singleton
    [TransientDependency]
    public class EfDemoDbContext : DbContext
    {
        private readonly DatabaseConfiguration _databaseConfig;
        public EfDemoDbContext(DatabaseConfiguration databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = _databaseConfig["Blog"];
            options.UseSqlite(dbPath);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<Post>()
                        .HasOne<Blog>()
                        .WithMany(t => t.Posts)
                        .HasForeignKey(t => t.BlogId);

            modelBuilder.Entity<Book>()
                        .HasKey(c => new { c.Code, c.Lang });
        }

        public EfDemoDbContext AsNoTracking()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return this;
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
    }

}
