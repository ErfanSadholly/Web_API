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
			try
			{		
				var result = await _authentication.RegisterAsync(registerModel);
	
				if (!result.IsSuccess)
				{
					return BadRequest(result.Message);
				}
		
				return Ok(result.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"خطای داخلی سرور: {ex.Message}");
			}
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			try
			{
				var result = await _authentication.LoginAsync(loginModel);
				if (!result.IsSuccess)
				{
					return Unauthorized(result.Message); 
				}

				return Ok(new { message = "ورود موفقیت‌آمیز", token = result.Data });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(ex.Message);  
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"خطای داخلی سرور: {ex.Message}"); 
			}
		}
	}
}
			
