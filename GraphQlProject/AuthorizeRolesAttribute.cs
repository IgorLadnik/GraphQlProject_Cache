using GraphQlProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace GraphQlProject
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserType[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
