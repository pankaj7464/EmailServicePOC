namespace EmailService.Server.Uttils
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }

}
