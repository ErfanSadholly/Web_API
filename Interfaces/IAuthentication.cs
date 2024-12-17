using Microsoft.AspNetCore.Identity;
using Web_Api.Account_Models;
using Web_Api.DbModels;
using Web_Api.DTOs;

namespace Web_Api.Interfaces
{
	public interface IAuthentication
	{
		Task<GeneralBasicResponseDto<User>> RegisterAsync(RegisterModel registerModel);
		Task<GeneralBasicResponseDto<string>> LoginAsync(LoginModel loginModel);
	}
}
