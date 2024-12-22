using Web_Api.Models.DbModels;

namespace Web_Api.Interfaces
{
	public interface IPhoneBook
	{	
		Task<IEnumerable<PhoneBook>> GetAllAsync();
		Task<PhoneBook> GetByIdAsync(int id);		
		Task CreateAsync(PhoneBook contact);
		Task UpdateAsync(PhoneBook contact);
		Task DeleteAsync(int id); // Soft Delete
	}
}
