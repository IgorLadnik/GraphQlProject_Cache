using Microsoft.Extensions.Configuration;

namespace JwtAuthLib
{
    public class JwtOptions
    {
        public string SigningKey { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }

        public JwtOptions(IConfiguration configuration)
        {
            SigningKey = configuration.GetValue<string>("JwtOptions:SigningKey");
            Issuer = configuration.GetValue<string>("JwtOptions:Issuer");
            Audience = configuration.GetValue<string>("JwtOptions:Audience");
        }
    }
}
