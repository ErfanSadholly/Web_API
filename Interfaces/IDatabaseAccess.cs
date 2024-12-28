namespace Web_Api.Interfaces
{
	public interface IDatabaseAccess
	{
		Task<string> GetUserFullName(int UserId);
		Task<bool> CanEditPhoneBook(int userId, int phoneBookId);
		Task<string> GetUserFullNameWithEF(int UserId);
		Task<string> GetUserFullNameWithDapper(int UserId);
	}
}