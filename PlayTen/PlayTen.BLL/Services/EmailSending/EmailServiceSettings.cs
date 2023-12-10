namespace PlayTen.BLL.Services.EmailSending
{
    public class EmailServiceSettings
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}
