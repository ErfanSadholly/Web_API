using Web_Api.Models.DbModels;

namespace Web_Api.Interfaces
{
	public interface IJwt
	{
		string GenerateWebToken(User user);
	}
}
