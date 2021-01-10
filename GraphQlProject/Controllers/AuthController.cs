using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraphQlProject.Services;

namespace GraphQlProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private AuthService _authService;

        public AuthController(AuthService authService)
        {
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
                return Ok(await _authService.Login(userName, password));
            }
            //catch (UserNotFoundException e)
            //{
            //    return NotFound(e.Message);
            //}
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
