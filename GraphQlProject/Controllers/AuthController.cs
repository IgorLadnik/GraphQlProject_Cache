using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JwtHelperLib;
using JwtHelperLib.Data;
using RepoInterfaceLib;

namespace GraphQlProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepo<UserDbContext> _repo;
        private AuthService _authService;

        public AuthController(IRepo<UserDbContext> repo, AuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(401)]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync(string userName, string password)
        {
            try
            {
                var user = await _repo.FetchAsync(dbContext => dbContext.Users
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
