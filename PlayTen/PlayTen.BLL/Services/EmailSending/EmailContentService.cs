using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.BLL.Models;

namespace PlayTen.BLL.Services
{
    public class EmailContentService : IEmailContentService
    {
        public EmailContentService(IUserService userService)
        {
        }
        public EmailModel GetAuthRegisterEmail(string confirmationLink)
        {
            return new EmailModel
            {
                Title = "PlayTen",
                Subject = "Підтвердження реєстрації",
                Message = $"<p>Підтвердіть реєстрацію, перейшовши за <a href='{confirmationLink}'>посиланням</a>.</p>"
            };
        }

        public EmailModel GetNewTrainingParticipantEmail(string username)
        {
            return new EmailModel
            {
                Title = "PlayTen",
                Subject = "Нове зголошення на тренування",
                Message = $"Новий користувач {username} зголосився на Ваше тренування."
            };
        }

        public EmailModel GetTrainingParticipantStatusChangedEmail(string trainingName, string status, string howToConnect = null)
        {
            var message = $"<p>Ваш статус для тренування  <b>{trainingName}</b> змінено на {status}.</p>";
            if(status == "Прийнято")
            {
                message += "<p>Вітаємо! Тепер Ви можете зв'язатись з організатором для подальшого узгодження деталей тренування. Ось контакти, які залишив користувач:</p>";
                message += $"<p>{howToConnect}</p>";
            }

            return new EmailModel
            {
                Title = "PlayTen",
                Subject = "Ваш статус учасника змінено",
                Message = message
            };
        }

        public EmailModel GetApprovedParticipantContactsEmail(string trainingName, string participantUserName, string howToConnect)
        {
            var message = $"<p>Вітаємо! Ви прийняли нового учасника {participantUserName} до свого тренування <b>{trainingName}</b>. Тепер Ви можете зв'язатись з ним для подальшого узгодження деталей тренування. Ось контакти, які залишив користувач:</p>";
            message += $"<p>{howToConnect}</p>";

            return new EmailModel
            {
                Title = "PlayTen",
                Subject = "Прийнято нового учасника тренування",
                Message = message
            };
        }
    }
}
