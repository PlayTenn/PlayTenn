using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;

namespace PlayTen.DAL
{
    public class PlayTenDbContext: IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Surface> Surfaces { get; set; }
        public DbSet<TennisLevel> TennisLevels { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Match> Matches { get; set; }
        public PlayTenDbContext(DbContextOptions<PlayTenDbContext> options) : base(options)
        {
            //Database.Migrate();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(x=>!x.Name.Contains("Identity")).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
              .HasMany(x => x.Participants)
              .WithOne(x => x.User)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
              .HasMany(x => x.Trainings)
              .WithOne(x => x.User)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
              .HasMany(x => x.Tournaments)
              .WithOne(x => x.User)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
             .HasOne(x => x.TennisLevel)
             .WithMany(x => x.Users)
             .HasForeignKey(x => x.TennisLevelId)
             .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>().HasOne<TrainingProgram>().WithOne(x => x.User).HasForeignKey<User>(x => x.TrainingProgramId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>().HasOne<SportKind>().WithMany(x => x.Users).HasForeignKey(x => x.SportKindId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SportKind>().HasMany<TrainingProgram>().WithOne(x => x.SportKind).HasForeignKey(x => x.SportKindId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SportKind>().HasMany<User>().WithOne(x => x.SportKind).HasForeignKey(x => x.SportKindId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SportKind>().Property(x => x.Id).
            //modelBuilder.Entity<TrainingProgram>().Property(x => x.SportKindId).IsRequired(false);
            //    .HasForeignKey(x => x.SportKindId).IsRequired(false);

            var surfacesNames = new string[] { "Ґрунт", "Хард", "Трава", "Штучна трава" };
            var participantStatusesNames = new string[] { "Прийнято", "Відмовлено", "Розглядається", "Учасник" };
            var tennisLevelsName = new string[] { "Початківець", "Любитель", "Професіонал" };

            var surfaces = surfacesNames.Select((x, y) => new Surface() { Id = y + 1, Type = x }).ToList();
            modelBuilder.Entity<Surface>()
                .HasData(surfaces);

            var participantStatuses = participantStatusesNames.Select((x, y) => new ParticipantStatus() { ID = y + 1, ParticipantStatusName = x }).ToList();
            modelBuilder.Entity<ParticipantStatus>()
                .HasData(participantStatuses);

            var tennisLevels = tennisLevelsName.Select((x, y) => new TennisLevel() { Id = y + 1, Level = x }).ToList();
            modelBuilder.Entity<TennisLevel>()
                .HasData(tennisLevels);

            modelBuilder.Entity<Place>().HasData(
            new Place { Id = 1, Name = "Динамо", StreetAddress = "в. Янева, 10", SurfaceId = 1 },
            new Place { Id = 2, Name = "ТЕН", StreetAddress = "в. Болгарська, 4", SurfaceId = 1 },
            new Place { Id = 3, Name = "ТЕН", StreetAddress = "в. Болгарська, 4", SurfaceId = 2 },
            new Place { Id = 4, Name = "Євроспорт", StreetAddress = "пл. Петрушевича, 1", SurfaceId = 1 },
            new Place { Id = 5, Name = "Іскра", StreetAddress = "в. Вулицька, 14", SurfaceId = 1 },
            new Place { Id = 6, Name = "Південний", StreetAddress = "в. Щирецька, 36", SurfaceId = 1 },
            new Place { Id = 7, Name = "Південний", StreetAddress = "в. Щирецька, 36", SurfaceId = 2 },
            new Place { Id = 8, Name = "Динамо", StreetAddress = "в. Зелена, 59-63", SurfaceId = 2 },
            new Place { Id = 9, Name = "Панська гора", StreetAddress = " с. Сокільники", SurfaceId = 2 },
            new Place { Id = 10, Name = "Tennis Life", StreetAddress = "в. Стрийська, 195 А", SurfaceId = 1 },
            new Place { Id = 11, Name = "Tennis Life", StreetAddress = "в. Стрийська, 195 А", SurfaceId = 2 },
            new Place { Id = 12, Name = "Брюховичі 1", StreetAddress = "в. Львівська, 80, Брюховичі", SurfaceId = 4 },
            new Place { Id = 13, Name = "Брюховичі 2", StreetAddress = "в. Пляжна, 2, Брюховичі", SurfaceId = 1 },
            new Place { Id = 14, Name = "Цитадель", StreetAddress = "в. Колесси, 8", SurfaceId = 1 });

            modelBuilder.Entity<Participant>().HasData(
            new Participant { ID = 1, ParticipantStatusId = 4 });
        }
    }
}
