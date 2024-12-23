using System.Security.Claims;
using Web_Api.Interfaces;

namespace Web_Api.Services
{
	public class ClaimService : IClaims
	{
		public int GetUserIdFromClaims(ClaimsPrincipal user) 
		{
			var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (int.TryParse(userIdString, out var userId))
			{
				return userId;
			}

			throw new InvalidOperationException("UserId not found or invalid.");
		}

		public string GetUserNameFromClaims(ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name)?.Value;
		}

		public bool IsUserAdminFromClaims(ClaimsPrincipal user)
		{
			var isAdminString = user.FindFirst("IsAdmin")?.Value;
			return bool.TryParse(isAdminString, out var isAdmin) && isAdmin;
		}
	}
}
