using System.Security.Claims;

namespace Web_Api.Interfaces
{
	public interface IClaims
	{
		int GetUserIdFromClaims(ClaimsPrincipal user);
		string GetUserNameFromClaims(ClaimsPrincipal user);
		bool IsUserAdminFromClaims(ClaimsPrincipal user);
	}
}
