using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepoInterfaceLib;
using JwtLoginLib;
using JwtLoginLib.Data;

namespace LoginService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private Authenticator _authenticator;

        public LoginController(IRepo<UserDbContext> repo, AuthenticationService authService)
        {
            _authenticator = new Authenticator(repo, authService);
        }

        [HttpPost]
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

