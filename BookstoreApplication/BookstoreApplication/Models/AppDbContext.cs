using Microsoft.EntityFrameworkCore;


namespace BookstoreApplication.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<AuthorAward> AuthorAwards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Award>(cfg =>
            {
                cfg.Property(a => a.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                cfg.Property(a => a.Description)
                    .HasMaxLength(2000)
                    .IsRequired();

                cfg.Property(a => a.StartYear)
                    .IsRequired();

                cfg.ToTable(t => t.HasCheckConstraint("CK_Award_StartYear", "\"StartYear\" >= 1800 AND \"StartYear\" <= EXTRACT(YEAR FROM CURRENT_DATE)"));
            });

            modelBuilder.Entity<Author>(cfg =>
            {
                cfg.Property(a => a.FullName)
                    .HasMaxLength(200)
                    .IsRequired();

                cfg.Property(a => a.Biography)
                    .HasMaxLength(2000)
                    .IsRequired();

                cfg.Property(a => a.DateOfBirth)
                    .HasColumnName("Birthday");
            });

            modelBuilder.Entity<AuthorAward>(cfg =>
            {
                cfg.ToTable("AuthorAwardBridge", t =>
                    t.HasCheckConstraint("CK_AuthorAward_YearAwarded",
                                        "\"YearAwarded\" >= 1800 AND \"YearAwarded\" <= EXTRACT(YEAR FROM CURRENT_DATE)"));

                cfg.HasKey(x => new { x.AuthorId, x.AwardId, x.YearAwarded });

                cfg.HasOne(x => x.Author)
                    .WithMany(a => a.AuthorAwards)
                    .HasForeignKey(a => a.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                cfg.HasOne(x => x.Award)
                    .WithMany(a => a.AuthorAwards)
                    .HasForeignKey(x => x.AwardId)
                    .OnDelete(DeleteBehavior.Cascade);

                cfg.Property(x => x.YearAwarded)
                    .IsRequired();
            });

            modelBuilder.Entity<Book>(cfg =>
            {
                cfg.HasOne(b => b.Publisher)
                    .WithMany()
                    .HasForeignKey(b => b.PublisherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
