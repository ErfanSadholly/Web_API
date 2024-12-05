using Microsoft.EntityFrameworkCore;
using Web_Api.AppData;
using Web_Api.DbModels;
using Web_Api.Interfaces;

namespace WebApi.Repositories
{
	public class PhoneBookRepository : IPhoneBook
	{
		private readonly AppDbContext _context;

		public PhoneBookRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<PhoneBook>> GetAllAsync()
		{
			return await _context.PhoneBooks.ToListAsync();
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
			var phoneBook = await _context.PhoneBooks.FirstOrDefaultAsync(x => x.ID == id);
			if (phoneBook != null)
			{
				phoneBook.Deleted = true;
				await _context.SaveChangesAsync();
			}
		}
	}
}