using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;

namespace PlayTen.DAL
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var surfacesNames = new string[] {"Ґрунт", "Хард", "Трава", "Штучна трава" };
            var participantStatusesNames = new string[] { "Прийнято", "Відмовлено", "Розглядається" };
            var tennisLevelsName = new string[] { "Початківець", "Любитель", "Професіонал" };

            var surfaces = surfacesNames.Select((x, y) => new Surface() {Id = y + 1, Type = x}).ToList();
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
        }
    }
}
