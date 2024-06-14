namespace Product.Domain.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTime { get; set; }
        public string SecKey { get; set; }
        public string IV { get; set; }
    }
}
