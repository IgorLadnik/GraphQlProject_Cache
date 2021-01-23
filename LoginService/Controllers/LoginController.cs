using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepoInterfaceLib;
using JwtLoginLib;
using JwtLoginLib.Data;
using System.Net.Http;
using System.Text.Json;

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
        
        [HttpPost]
        [Route("fromcode")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(401)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(JsonElement arg)
        {
            var str = arg.GetString();
            if (string.IsNullOrEmpty(str))
                return null;

            var ss = str.Split(new string[] { "?", "=", "&" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (ss?.Length == 4) 
            {
                string userName, password;
                if (ss[0].ToLowerInvariant() == "username" && ss[2].ToLowerInvariant() == "password")
                {
                    userName = ss[1];
                    password = ss[3];
                }
                else 
                {
                    userName = ss[3];
                    password = ss[1];
                }

                return await LoginAsync(userName, password);
            }

            return null;
        }
    }
}

