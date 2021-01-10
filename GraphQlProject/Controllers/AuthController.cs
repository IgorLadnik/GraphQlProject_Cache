using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraphQlHelperLib;
using GraphQlProject.Data;
using JwtHelperLib;

namespace GraphQlProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private DbProvider<GraphQLDbContext> _dbProvider;
        private AuthService _authService;

        public AuthController(DbProvider<GraphQLDbContext> dbProvider, AuthService authService)
        {
            _dbProvider = dbProvider;
            _authService = authService;
        }

        [HttpPost("login")]
        [Route("login")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(401)]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync(string userName, string password)
        {
            try
            {
                var user = await _dbProvider.FetchAsync(dbContext => dbContext.Users
                            .Where(u => u.UserName == userName && u.Password == password)
                            .FirstOrDefault());
                return user == null 
                        ? StatusCode(500, "User not found.")
                        : Ok(await _authService.Login(user.UserName, user.Type));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
