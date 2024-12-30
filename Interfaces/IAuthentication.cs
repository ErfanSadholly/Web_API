using Web_Api.DTOs;
using Web_Api.Models.AuthModels;
using Web_Api.Models.DbModels;

namespace Web_Api.Interfaces
{
	public interface IAuthentication
	{
		Task<GeneralBasicResponseDto<User>> RegisterAsync(RegisterModel registerModel);
		Task<GeneralBasicResponseDto<string>> LoginAsync(LoginModel loginModel);
	}
}
