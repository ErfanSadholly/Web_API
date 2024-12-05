using Web_Api.DbModels;

namespace Web_Api.Interfaces
{
	public interface IJwt
	{
		string GenerateWebToken(User user);
	}
}
