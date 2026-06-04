namespace WarshaOnEverythingYouLearned.Configrations
{
    public class JWTOptions
    {
        public string key { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
        public string expireHoure { get; set; }
    }
}
