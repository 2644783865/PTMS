namespace PTMS.Common
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public string[] CorsAllowedOrigins { get; set; }
        public string SmtpServerPort { get; set; }
        public string SmtpServerName { get; set; }
        public bool SmtpUseSsl { get; set; }
        public string SmtpServerLogin { get; set; }
        public string SmtpServerPassword { get; set; }
        public string EmailSender { get; set; }
        public string BaseSiteUrl { get; set; }
        public string AdminRecipient { get; set; }
        public string ProjectsDatabaseConnection { get; set; }
        public string DataDatabaseConnection { get; set; }
    }
}
