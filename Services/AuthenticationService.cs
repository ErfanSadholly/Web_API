using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;
using Web_Api.DTOs;
using Web_Api.Interfaces;
using Web_Api.Models.AuthModels;
using Web_Api.Models.DbModels;

namespace Web_Api.Services
{
	public class AuthenticationService : IAuthentication
	{
		private readonly UserManager<User> _userManager;
		private readonly JwtService _jwtService;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _contextAccessor;

		public AuthenticationService(UserManager<User> userManager, JwtService jwtService, IMapper mapper, IHttpContextAccessor contextAccessor)
		{
			_userManager = userManager;
			_jwtService = jwtService;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
		}

		public async Task<GeneralBasicResponseDto<User>> RegisterAsync(RegisterModel registerModel) 
		{

			var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
			if (existingUser != null)
			{
				return new GeneralBasicResponseDto<User>()
				{
					IsSuccess = false,
					Message = ".ایمیل قبلاً ثبت شده است", 
					Data = null
				};
			}

			var user = _mapper.Map<User>(registerModel);

			var result = await _userManager.CreateAsync(user, registerModel.Password);

			if (result.Succeeded)
			{
				return new GeneralBasicResponseDto<User>
				{
					IsSuccess = true,
					Data = user
				};
			}
			else
			{
				return new GeneralBasicResponseDto<User>
				{
					IsSuccess = false,
					Message = ".ثبت نام با خطا مواجه شد",
					Data = null
				};
			}
		}

		public async Task<GeneralBasicResponseDto<string>> LoginAsync(LoginModel loginModel) 
		{
			var contact = await _userManager.FindByEmailAsync(loginModel.Email);
			if (contact == null) 
			{
				return new GeneralBasicResponseDto<string>
				{
					IsSuccess = false,
					Message = ".کاربر یافت نشد",
					Data = null
				};
			}

			var passIsValid = await _userManager.CheckPasswordAsync(contact, loginModel.Password);
			if (!passIsValid)
			{
				return new GeneralBasicResponseDto<string>
				{
					IsSuccess = false,
					Message = ".رمز عبور اشتباه است",
					Data = null
				};
			}

			var token = _jwtService.GenerateWebToken(contact);
			return new GeneralBasicResponseDto<string>
			{
				IsSuccess = true,
				Data = token
			};
		}
	}
}