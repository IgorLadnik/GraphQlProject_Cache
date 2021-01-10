using Microsoft.AspNetCore.Authorization;
using JwtHelperLib;

namespace GraphQlProject
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserAuthType[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
