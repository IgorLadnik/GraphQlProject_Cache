using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(JsonElement arg)
        {
            string strJson = null;
            switch (arg.ValueKind) 
            {
                case JsonValueKind.String:
                    strJson = arg.GetString();
                    break;

                case JsonValueKind.Object:
                    strJson = arg.GetRawText();
                    break;
            }

            if (string.IsNullOrEmpty(strJson))
                return null;

            var jObj = JObject.Parse(strJson);
            var userName = $"{jObj["username"]}";
            var password = $"{jObj["password"]}";

            var result = await _authenticator.LoginAsync(userName, password);
            return _authenticator.IsOK ? Ok(result) : StatusCode(500, result);
        }
    }
}

