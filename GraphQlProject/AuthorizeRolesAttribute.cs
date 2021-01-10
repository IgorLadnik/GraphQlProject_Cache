using GraphQlProject.Models;
using Microsoft.AspNetCore.Authorization;

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
