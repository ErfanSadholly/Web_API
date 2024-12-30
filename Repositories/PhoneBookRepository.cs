using Microsoft.EntityFrameworkCore;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;

namespace WebApi.Repositories
{
	public class PhoneBookRepository : IPhoneBook
	{
		private readonly IAppDbContext _context;

		public PhoneBookRepository(IAppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<PhoneBook>> GetAllAsync(string? FirstName, string? LastName, string? PhoneNumber)
		{

			if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(PhoneNumber))
			{
				return await _context.PhoneBooks.ToListAsync();
			}
			else
			{
				return _context.PhoneBooks.Where(x => x.LastName == LastName || x.FirstName == FirstName || x.PhoneNumber == PhoneNumber);
			}

			//if(query == FirstName)

			//if (!string.IsNullOrEmpty(LastName))
			//{
			//	return _context.PhoneBooks.Where(x => x.LastName == LastName); 
			//}
			
			//if (!string.IsNullOrEmpty(PhoneNumber))
			//{
			//	return _context.PhoneBooks.Where(x => x.PhoneNumber == PhoneNumber);
			//}

			//return await _context.PhoneBooks.ToListAsync();
		}

		public async Task<PhoneBook> GetByIdAsync(int id)
		{
			return await _context.PhoneBooks.FirstOrDefaultAsync(p => p.ID == id);
		}

		public async Task CreateAsync(PhoneBook contact)
		{
			_context.PhoneBooks.Add(contact);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(PhoneBook contact)
		{
			_context.PhoneBooks.Update(contact);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var phoneBook = await _context.PhoneBooks.FindAsync(id);
			if (phoneBook != null)
			{
				phoneBook.Deleted = true;
				await _context.SaveChangesAsync();
			}
		}
	}
}