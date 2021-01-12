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
        private Authenticator _authenticator;

        public AuthController(IRepo<UserDbContext> repo, AuthenticationService authService)
        {
            _authenticator = new Authenticator(repo, authService);
        }

        [HttpPost]
        [Route("login")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(401)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var result = await _authenticator.LoginAsync(userName, password);
            return _authenticator.IsOK ? Ok(result) : StatusCode(500, result);
        }
    }
}
