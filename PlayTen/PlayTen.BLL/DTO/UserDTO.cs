using Microsoft.AspNetCore.Identity;
using PlayTen.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTen.BLL.DTO
{
    public class UserDTO : IdentityUser
    {
        [Display(Name = "Ім'я")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ім'я повинне складати від 2 до 25 символів")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Прізвище має містити тільки літери")]
        [Required(ErrorMessage = "Поле прізвище є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Прізвище повинне складати від 2 до 25 символів")]
        public string Surname { get; set; }
        public int TennisLevelId { get; set; }
        public DateTime RegistredOn { get; set; }
        public string HowToConnect { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageFilename { get; set; }
        public TennisLevel TennisLevel { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
        public IEnumerable<Training> Trainings { get; set; }
        public IEnumerable<Tournament> Tournaments { get; set; }
    }
}
