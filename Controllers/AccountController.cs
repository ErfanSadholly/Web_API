using Microsoft.AspNetCore.Mvc;
using Web_Api.Account_Models;
using Web_Api.DbModels;
using Web_Api.Interfaces;
using Web_Api.Services;

namespace Web_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		private readonly IAuthentication _authentication;

		public AccountController(IAuthentication authentication)
		{
			_authentication = authentication;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
		{
			var user = new User
			{
				UserName = registerModel.Email,
				Email = registerModel.Email
			};

			try
			{
				var result = await _authentication.RegisterAsync(registerModel);
				if (!result.Succeeded)
				{
					return BadRequest(result.Errors);
				}

				return Ok("ثبت نام موفقیت‌آمیز بود");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			} 
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			try
			{
				var token = await _authentication.LoginAsync(loginModel);
				return Ok(new { token });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
			
