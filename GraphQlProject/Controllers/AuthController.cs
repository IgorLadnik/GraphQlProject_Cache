using GraphQlProject.DTO;
using GraphQlProject.Models;
using GraphQlProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQlProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private AuthenticationService _authService;

        public AuthController(AuthenticationService authService)
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
                var jwt = await _authService.Login(new UserDTO { UserName = userName, Password = password });
                return Ok(jwt);
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
