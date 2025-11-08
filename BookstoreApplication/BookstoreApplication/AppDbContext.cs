using BookstoreApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace BookstoreApplication
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
                    .HasColumnName("Birthday")
                    .HasColumnType("date");
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
                cfg.Property(b => b.PublishedDate)
                     .HasColumnType("date");

                cfg.HasOne(b => b.Publisher)
                    .WithMany()
                    .HasForeignKey(b => b.PublisherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "role-biblio-id", Name = "Bibliotekar", NormalizedName = "BIBLIOTEKAR", ConcurrencyStamp = "cs1" },
                new IdentityRole { Id = "role-urednik-id", Name = "Urednik", NormalizedName = "UREDNIK", ConcurrencyStamp = "cs2" }
            );


            // === SEED PODACI (v3) ===
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Penguin Books", Address = "80 Strand, London", Website = "https://penguin.co.uk" },
                new Publisher { Id = 2, Name = "Bloomsbury", Address = "50 Bedford Square, London", Website = "https://www.bloomsbury.com" },
                new Publisher { Id = 3, Name = "Vintage Books", Address = "New York, USA", Website = "https://www.vintagebooks.com" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "George Orwell", Biography = "British writer and journalist.", DateOfBirth = new DateTime(1903, 6, 25) },
                new Author { Id = 2, FullName = "Jane Austen", Biography = "English novelist known for romantic fiction.", DateOfBirth = new DateTime(1775, 12, 16) },
                new Author { Id = 3, FullName = "J.K. Rowling", Biography = "British author best known for Harry Potter.", DateOfBirth = new DateTime(1965, 7, 31) },
                new Author { Id = 4, FullName = "Mark Twain", Biography = "American writer and humorist.", DateOfBirth = new DateTime(1835, 11, 30) },
                new Author { Id = 5, FullName = "Leo Tolstoy", Biography = "Russian novelist, known for War and Peace.", DateOfBirth = new DateTime(1828, 9, 9) }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", PageCount = 328, PublishedDate = new DateTime(1949, 6, 8), ISBN = "9780451524935", AuthorId = 1, PublisherId = 1 },
                new Book { Id = 2, Title = "Animal Farm", PageCount = 112, PublishedDate = new DateTime(1945, 8, 17), ISBN = "9780451526342", AuthorId = 1, PublisherId = 1 },
                new Book { Id = 3, Title = "Pride and Prejudice", PageCount = 432, PublishedDate = new DateTime(1813, 1, 28), ISBN = "9780141439518", AuthorId = 2, PublisherId = 1 },
                new Book { Id = 4, Title = "Emma", PageCount = 474, PublishedDate = new DateTime(1815, 12, 23), ISBN = "9780141439587", AuthorId = 2, PublisherId = 3 },
                new Book { Id = 5, Title = "Harry Potter and the Philosopher's Stone", PageCount = 223, PublishedDate = new DateTime(1997, 6, 26), ISBN = "9780747532699", AuthorId = 3, PublisherId = 2 },
                new Book { Id = 6, Title = "Harry Potter and the Chamber of Secrets", PageCount = 251, PublishedDate = new DateTime(1998, 7, 2), ISBN = "9780747538493", AuthorId = 3, PublisherId = 2 },
                new Book { Id = 7, Title = "Harry Potter and the Prisoner of Azkaban", PageCount = 317, PublishedDate = new DateTime(1999, 7, 8), ISBN = "9780747542155", AuthorId = 3, PublisherId = 2 },
                new Book { Id = 8, Title = "Adventures of Huckleberry Finn", PageCount = 366, PublishedDate = new DateTime(1884, 12, 10), ISBN = "9780142437179", AuthorId = 4, PublisherId = 3 },
                new Book { Id = 9, Title = "The Adventures of Tom Sawyer", PageCount = 274, PublishedDate = new DateTime(1876, 6, 1), ISBN = "9780143039563", AuthorId = 4, PublisherId = 1 },
                new Book { Id = 10, Title = "War and Peace", PageCount = 1225, PublishedDate = new DateTime(1869, 1, 1), ISBN = "9780140447934", AuthorId = 5, PublisherId = 3 },
                new Book { Id = 11, Title = "Anna Karenina", PageCount = 864, PublishedDate = new DateTime(1877, 4, 1), ISBN = "9780143035008", AuthorId = 5, PublisherId = 3 },
                new Book { Id = 12, Title = "The Death of Ivan Ilyich", PageCount = 86, PublishedDate = new DateTime(1886, 1, 1), ISBN = "9780553210354", AuthorId = 5, PublisherId = 1 }
            );

            modelBuilder.Entity<Award>().HasData(
                new Award { Id = 1, Name = "Nobel Prize in Literature", Description = "Global award for outstanding contributions to literature.", StartYear = 1901 },
                new Award { Id = 2, Name = "Pulitzer Prize for Fiction", Description = "Distinguished fiction by an American author.", StartYear = 1917 },
                new Award { Id = 3, Name = "Booker Prize", Description = "Best original novel in English.", StartYear = 1969 },
                new Award { Id = 4, Name = "National Book Award", Description = "Annual U.S. awards for literature.", StartYear = 1950 }
            );

            // 15 unikatnih dodela (AuthorId, AwardId, YearAwarded) — po 3 dodele za svakog autora
            modelBuilder.Entity<AuthorAward>().HasData(
                // George Orwell (1)
                new AuthorAward { AuthorId = 1, AwardId = 2, YearAwarded = 1949 },
                new AuthorAward { AuthorId = 1, AwardId = 4, YearAwarded = 1951 },
                new AuthorAward { AuthorId = 1, AwardId = 3, YearAwarded = 1970 },

                // Jane Austen (2)
                new AuthorAward { AuthorId = 2, AwardId = 1, YearAwarded = 1905 },
                new AuthorAward { AuthorId = 2, AwardId = 2, YearAwarded = 1925 },
                new AuthorAward { AuthorId = 2, AwardId = 3, YearAwarded = 1971 },

                // J.K. Rowling (3)
                new AuthorAward { AuthorId = 3, AwardId = 4, YearAwarded = 2001 },
                new AuthorAward { AuthorId = 3, AwardId = 2, YearAwarded = 2000 },
                new AuthorAward { AuthorId = 3, AwardId = 3, YearAwarded = 1999 },

                // Mark Twain (4)
                new AuthorAward { AuthorId = 4, AwardId = 1, YearAwarded = 1907 },
                new AuthorAward { AuthorId = 4, AwardId = 2, YearAwarded = 1918 },
                new AuthorAward { AuthorId = 4, AwardId = 4, YearAwarded = 1952 },

                // Leo Tolstoy (5)
                new AuthorAward { AuthorId = 5, AwardId = 1, YearAwarded = 1902 },
                new AuthorAward { AuthorId = 5, AwardId = 2, YearAwarded = 1920 },
                new AuthorAward { AuthorId = 5, AwardId = 3, YearAwarded = 1970 }
            );

            var u1 = new ApplicationUser
            {
                Id = "user-urednik-1",
                UserName = "urednik1",
                NormalizedUserName = "UREDNIK1",
                Email = "urednik1@biblioteka.com",
                NormalizedEmail = "UREDNIK1@BIBLIOTEKA.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEACiMOTSBAIcnQk9wEBEA/qdrm3UemBBof9VgRrlvEcniJCDlriPbIvHTUWYpg5kuQ==",
                SecurityStamp = "9e8b281e-fbc2-4cd1-aab5-a02df6166468",
                ConcurrencyStamp = "b535b366-ea99-448e-ad9a-fdd70e579f27",
                Name = "Urednik",
                Surname = "Prvi"
            };

            var u2 = new ApplicationUser
            {
                Id = "user-urednik-2",
                UserName = "urednik2",
                NormalizedUserName = "UREDNIK2",
                Email = "urednik2@biblioteka.com",
                NormalizedEmail = "UREDNIK2@BIBLIOTEKA.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAELquf/7MEr6z/q/HjUr4YVhSCZhc1vVfEAMdIcxcVYjwPatpn1luBCzTkHVAjWpdDQ==",
                SecurityStamp = "ada5ff65-a5c2-4309-8643-bb98c648f5fb",
                ConcurrencyStamp = "5a1d86b1-39b7-463e-9b13-218c9023c82b"
            };

            var u3 = new ApplicationUser
            {
                Id = "user-urednik-3",
                UserName = "urednik3",
                NormalizedUserName = "UREDNIK3",
                Email = "urednik3@biblioteka.com",
                NormalizedEmail = "UREDNIK3@BIBLIOTEKA.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEL894LICQk6+LQGwiiVbELAskhk4fjWpAqzJ6rRF9lv8q74F6qMias6mM5H7xBFYqg==",
                SecurityStamp = "01fa1d41-5f06-4dd0-92c1-1ef9c43f0987",
                ConcurrencyStamp = "52da9751-5bef-4fe6-9715-0a8699ea733a"
            };

            var b1 = new ApplicationUser
            {
                Id = "user-biblio-1",
                UserName = "biblio1",
                NormalizedUserName = "BIBLIO1",
                Email = "biblio1@biblioteka.com",
                NormalizedEmail = "BIBLIO1@BIBLIOTEKA.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEBhOFhUG/S2uCEetAY5/ImfdCtPFtxu+28R05wK9NJO/GX22MFPBn7y2ppdThe56QA==",
                SecurityStamp = "b609a2b5-128f-43f8-8720-ed7710073ed2",
                ConcurrencyStamp = "07636b11-9fd5-437d-a9f7-a2374af6aacf"
            };

            modelBuilder.Entity<ApplicationUser>().HasData(u1, u2, u3, b1);

            // === Seed veze korisnik–rola ===
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "user-urednik-1", RoleId = "role-urednik-id" },
                new IdentityUserRole<string> { UserId = "user-urednik-2", RoleId = "role-urednik-id" },
                new IdentityUserRole<string> { UserId = "user-urednik-3", RoleId = "role-urednik-id" },

                new IdentityUserRole<string> { UserId = "user-biblio-1", RoleId = "role-biblio-id" }
            );
        }
    }
}
