using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtHelperLib
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
