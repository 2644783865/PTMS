namespace PTMS.BusinessLogic.Config
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public string[] CorsAllowedOrigins { get; set; }
    }
}
