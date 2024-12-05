using Microsoft.AspNetCore.Identity;
using Web_Api.Account_Models;

namespace Web_Api.Interfaces
{
	public interface IAuthentication
	{
		Task<IdentityResult> RegisterAsync(RegisterModel registerModel);
		Task<string> LoginAsync(LoginModel loginModel);
	}
}
