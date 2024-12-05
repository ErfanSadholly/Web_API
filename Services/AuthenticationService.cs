using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Web_Api.Account_Models;
using Web_Api.DbModels;
using Web_Api.Interfaces;

namespace Web_Api.Services
{
	public class AuthenticationService : IAuthentication 
	{
		private readonly UserManager<User> _userManager;
		private readonly JwtService _jwtService;

		public AuthenticationService(UserManager<User> userManager , JwtService jwtService)
		{
			_userManager = userManager;
			_jwtService = jwtService;
		}

		public async Task<IdentityResult> RegisterAsync(RegisterModel registerModel) 
		{

			var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
			if (existingUser != null)
			{
				throw new InvalidOperationException("ایمیل قبلاً ثبت شده است.");
			}

			var user = new User
			{
				Email = registerModel.Email,
				UserName = registerModel.Email,
			};

			return await _userManager.CreateAsync(user, registerModel.Password);
		}

		public async Task<string> LoginAsync(LoginModel loginModel) 
		{
			var contact = await _userManager.FindByEmailAsync(loginModel.Email);
			if (contact == null) 
			{
				throw new UnauthorizedAccessException(".کاربر یافت نشد");
			}

			var passIsValid = await _userManager.CheckPasswordAsync(contact, loginModel.Password);
			if (!passIsValid)
			{
				throw new UnauthorizedAccessException(".رمز عبور اشتباه است");
			}

			return _jwtService.GenerateWebToken(contact);			
		}
	}
}
