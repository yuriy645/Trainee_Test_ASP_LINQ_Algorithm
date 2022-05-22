using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class TestContext : DbContext
    {
        public TestContext()
        {
        }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Book> Books { get; set; }


        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<AuthorsBook> AuthorsBooks { get; set; }
        
        public virtual DbSet<FirstName> FirstNames { get; set; }
        
        public virtual DbSet<Patronymic> Patronymics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocaldb; database=Test; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.AuthorId)
                    .HasName("PK_Authors_AuthorId");

                //entity.Property(e => e.AuthorId).ValueGeneratedNever();

                entity.Property(e => e.AuthorSecondName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.FirstName)
                    .WithMany(p => p.Authors)
                    .HasForeignKey(d => d.FirstNameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Authors_FirstNames");

                entity.HasOne(d => d.Patronymic)
                    .WithMany(p => p.Authors)
                    .HasForeignKey(d => d.PatronymicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Authors_Patronymics");
            });

            modelBuilder.Entity<AuthorsBook>(entity =>
            {
                entity.HasKey(e => new { e.AuthorId, e.BookId })
                      .HasName("PK_AuthorsBooks");

                //entity.HasKey(e => e.AuthorsBookId)
                //      .HasName("PK_AuthorsBooks_AuthorsBookId");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuthorsBooks)
                    .HasForeignKey(d => d.AuthorId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .OnDelete(DeleteBehavior.ClientNoAction) //чтобы можно было удалять Autors
                    .HasConstraintName("FK_AuthorsBooks_Authors");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.AuthorsBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorsBooks_Books");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId)
                   .HasName("PK_Books_BookId");

                //entity.Property(e => e.BookId).ValueGeneratedNever();

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Genres");
            });

            modelBuilder.Entity<FirstName>(entity =>
            {
                entity.HasKey(e => e.FirstNameId)
                   .HasName("PK_FirstNames_FirstNameId");

               // entity.Property(e => e.FirstNameId).ValueGeneratedNever();

                entity.Property(e => e.FirstName1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FirstName");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId)
                   .HasName("PK_Genres_GenreId");

                //entity.Property(e => e.GenreId).ValueGeneratedNever();

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Patronymic>(entity =>
            {
                entity.HasKey(e => e.PatronymicId)
                   .HasName("PK_Patronymics_PatronymicId");

               // entity.Property(e => e.PatronymicId).ValueGeneratedNever();

                entity.Property(e => e.Patronymic1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Patronymic");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
