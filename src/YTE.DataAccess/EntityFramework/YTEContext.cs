using Microsoft.EntityFrameworkCore;





#nullable disable

namespace YTE.Entities.Context
{
    public partial class YTEContext : DbContext
    {
        public YTEContext()
        {
        }

        public YTEContext(DbContextOptions<YTEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumGenre> AlbumGenres { get; set; }
        public virtual DbSet<AlbumGenreAlbum> AlbumGenreAlbums { get; set; }
        public virtual DbSet<ArtObject> ArtObjects { get; set; }
        public virtual DbSet<ArtObjectType> ArtObjectTypes { get; set; }
        public virtual DbSet<ArtReview> ArtReviews { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookGenre> BookGenres { get; set; }
        public virtual DbSet<BookGenreBook> BookGenreBooks { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<FilmGenre> FilmGenres { get; set; }
        public virtual DbSet<FilmGenreFilm> FilmGenreFilms { get; set; }
        public virtual DbSet<ForbiddenWord> ForbiddenWords { get; set; }

        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Manga> Mangas { get; set; }
        public virtual DbSet<MangaGenre> MangaGenres { get; set; }
        public virtual DbSet<MangaGenreManga> MangaGenreMangas { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<TokenType> TokenTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<VideoGame> VideoGames { get; set; }
        public virtual DbSet<VideoGameGenre> VideoGameGenres { get; set; }
        public virtual DbSet<VideoGameGenreVideoGame> VideoGameGenreVideoGames { get; set; }
        public virtual DbSet<WatchList> WatchLists { get; set; }
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        public virtual DbSet<FollowList> FollowLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=(local);Initial Catalog=YTE;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Album");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Length).HasColumnType("time(7)");

                entity.HasOne(d => d.ArtObject)
                    .WithOne(p => p.Album)
                    .HasForeignKey<Album>(d => d.Id)
                    .HasConstraintName("FK_Album_ArtObject");
            });

            modelBuilder.Entity<AlbumGenre>(entity =>
            {
                entity.ToTable("AlbumGenre");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AlbumGenreAlbum>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.AlbumId })
                    .HasName("PK_Genre_MusicArt");

                entity.ToTable("AlbumGenre_Album");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.AlbumGenreAlbums)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Genre_Album_AlbumGenre");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumGenreAlbums)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_Genre_Album_Album");
            });

            modelBuilder.Entity<ArtObject>(entity =>
            {
                entity.ToTable("ArtObject");

                entity.HasIndex(e => e.ReleaseDate, "IX_Date_ASC");

                entity.HasIndex(e => e.ReleaseDate, "IX_Date_DESC");

                entity.HasIndex(e => e.Name, "IX_Name_ASC");

                entity.HasIndex(e => e.Name, "IX_Name_DESC");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.HasOne(d => d.Background)
                    .WithMany(p => p.ArtObjectBackgrounds)
                    .HasForeignKey(d => d.BackgroundId)
                    .HasConstraintName("FK_ArtObject_Image_Background");

                entity.HasOne(d => d.Poster)
                    .WithMany(p => p.ArtObjectPosters)
                    .HasForeignKey(d => d.PosterId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ArtObject_Image_Poster");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ArtObjects)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_ArtObject_ArtObjectType");
            });

            modelBuilder.Entity<ArtObjectType>(entity =>
            {
                entity.ToTable("ArtObjectType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ArtReview>(entity =>
            {
                entity.HasKey(e => new { e.ArtObjectId, e.UserId });

                entity.ToTable("ArtReview");

                entity.HasIndex(e => e.ExperiencedAt, "IX_DateExp_ASC");

                entity.HasIndex(e => e.ExperiencedAt, "IX_DateExp_DESC");

                entity.HasIndex(e => e.Score, "IX_Score_ASC");

                entity.HasIndex(e => e.Score, "IX_Score_DESC");

                entity.HasIndex(e => e.Date, "IX_Date_ASC");

                entity.HasIndex(e => e.Date, "IX_Date_DESC");

                entity.Property(e => e.ExperiencedAt).HasColumnType("date");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Score).HasColumnType("numeric(3, 1)");

                entity.HasOne(d => d.ArtObject)
                    .WithMany(p => p.ArtReviews)
                    .HasForeignKey(d => d.ArtObjectId)
                    .HasConstraintName("FK_Art_Review_ArtObject");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ArtReviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Art_Review_User");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ArtObject)
                    .WithOne(p => p.Book)
                    .HasForeignKey<Book>(d => d.Id)
                    .HasConstraintName("FK_Book_ArtObject");
            });

            modelBuilder.Entity<BookGenre>(entity =>
            {
                entity.ToTable("BookGenre");

                entity.HasIndex(e => e.Name, "IX_BookGenre")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_BookGenre_1")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BookGenreBook>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.BookId })
                    .HasName("PK_Genre_Book");

                entity.ToTable("BookGenre_Book");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookGenreBooks)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_Genre_Book_Book");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.BookGenreBooks)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Genre_Book_BookGenre");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("Film");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Length).HasColumnType("time(0)");

                entity.Property(e => e.Studio)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ArtObject)
                    .WithOne(p => p.Film)
                    .HasForeignKey<Film>(d => d.Id)
                    .HasConstraintName("FK_Film_ArtObject");
            });

            modelBuilder.Entity<FilmGenre>(entity =>
            {
                entity.ToTable("FilmGenre");

                entity.HasIndex(e => e.Name, "IX_FilmGenre")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_FilmGenre_1")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FilmGenreFilm>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.FilmId })
                    .HasName("PK_Genre_VisualArt");

                entity.ToTable("FilmGenre_Film");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.FilmGenreFilms)
                    .HasForeignKey(d => d.FilmId)
                    .HasConstraintName("FK_Genre_VisualArt_Film");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.FilmGenreFilms)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Genre_VisualArt_Genre_VisualArt1");
            });

            modelBuilder.Entity<ForbiddenWord>(entity =>
            {
                entity.HasIndex(e => e.Word, "IX_ForbiddenWords_ASC")
                   .IsUnique();

                entity.HasIndex(e => e.Word, "IX_ForbiddenWords_DESC")
                    .IsUnique();

                entity.Property(e => e.Word)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Manga>(entity =>
            {
                entity.ToTable("Manga");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsFinished).HasColumnName("isFinished");

                entity.HasOne(d => d.ArtObject)
                    .WithOne(p => p.Manga)
                    .HasForeignKey<Manga>(d => d.Id)
                    .HasConstraintName("FK_Manga_ArtObject");
            });

            modelBuilder.Entity<MangaGenre>(entity =>
            {
                entity.ToTable("MangaGenre");

                entity.HasIndex(e => e.Name, "IX_MangaGenre")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "IX_MangaGenre_1")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MangaGenreManga>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.MangaId })
                    .HasName("PK_Genre_Manga");

                entity.ToTable("MangaGenre_Manga");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MangaGenreMangas)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Genre_Manga_MangaGenre");

                entity.HasOne(d => d.Manga)
                    .WithMany(p => p.MangaGenreMangas)
                    .HasForeignKey(d => d.MangaId)
                    .HasConstraintName("FK_Genre_Manga_Genre_Manga");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");

                entity.Property(e => e.Id).HasMaxLength(72);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Token_TokenType");
            });

            modelBuilder.Entity<TokenType>(entity =>
            {
                entity.ToTable("TokenType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "CHK_Email")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Age).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Pronoun)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.UserName, "IX_User")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_User_Gender");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_User_Image");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.UserId });

                entity.ToTable("User_Role");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_User_Role_User");
            });

            modelBuilder.Entity<WatchList>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.ArtObjectId })
                    .HasName("PK_WatchList");

                entity.ToTable("WatchList");

                entity.HasOne(d => d.ArtObject)
                    .WithMany(p => p.WatchLists)
                    .HasForeignKey(d => d.ArtObjectId)
                    .HasConstraintName("FK_WatchList_ArtObject");

                entity.HasOne(d => d.User)
                   .WithMany(p => p.WatchLists)
                   .HasPrincipalKey(d => d.UserName)
                   .HasForeignKey(d => d.UserName)
                   .HasConstraintName("FK_WatchList_User");
            });

            modelBuilder.Entity<FavoriteList>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ArtObjectId })
                    .HasName("PK_FavoriteList");

                entity.ToTable("FavoriteList");

                entity.HasOne(d => d.ArtReview)
                    .WithMany(p => p.FavoriteLists)
                    .HasForeignKey(d => new
                    {
                        d.ArtObjectId,
                        d.UserId
                    })
                    .HasConstraintName("FK_FavoriteList_Art_Review");
            });

            modelBuilder.Entity<VideoGame>(entity =>
            {
                entity.ToTable("VideoGame");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Esrbrating)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ESRBRating");

                entity.Property(e => e.IsMultiplayer).HasColumnName("isMultiplayer");

                entity.HasOne(d => d.ArtObject)
                    .WithOne(p => p.VideoGame)
                    .HasForeignKey<VideoGame>(d => d.Id)
                    .HasConstraintName("FK_VideoGame_ArtObject");
            });

            modelBuilder.Entity<VideoGameGenre>(entity =>
            {
                entity.ToTable("VideoGameGenre");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VideoGameGenreVideoGame>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.VideoGameId })
                    .HasName("PK_Genre_VideoGame");

                entity.ToTable("VideoGameGenre_VideoGame");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.VideoGameGenreVideoGames)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Genre_VideoGame_VideoGameGenre");

                entity.HasOne(d => d.VideoGame)
                    .WithMany(p => p.VideoGameGenreVideoGames)
                    .HasForeignKey(d => d.VideoGameId)
                    .HasConstraintName("FK_Genre_VideoGame_VideoGame");
            });

            modelBuilder.Entity<FollowList>(entity =>
            {
                entity.HasKey(e => new { e.FollowerUserId, e.FollowedUserId });

                entity.ToTable("FollowList");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.FollowedUser)
                    .WithMany(p => p.FollowListFollowedUsers)
                    .HasForeignKey(d => d.FollowedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FollowList_User1");

                entity.HasOne(d => d.FollowerUser)
                    .WithMany(p => p.FollowListFollowerUsers)
                    .HasForeignKey(d => d.FollowerUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FollowList_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
