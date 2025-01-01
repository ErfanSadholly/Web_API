using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using Web_Api.DTOs;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;
using Web_Api.PhoneBookRequest;
using static Web_Api.Enums.SortEnums;


namespace WebApi.Repositories
{
	public class PhoneBookRepository : IPhoneBook
	{
		private readonly IAppDbContext _context;

		public PhoneBookRepository(IAppDbContext context)
		{
			_context = context;
		}

		public async Task<PagedResponseDto<PhoneBook>> GetAllAsync(PhoneBookRequestParameters phoneBookRequest)
		{
			IQueryable<PhoneBook> query = _context.PhoneBooks;

			if (!string.IsNullOrEmpty(phoneBookRequest.FirstName))
			{
				query = query.Where(x => x.FirstName == phoneBookRequest.FirstName);
			}

			if (!string.IsNullOrEmpty(phoneBookRequest.LastName))
			{
				query = query.Where(x => x.LastName.Contains(phoneBookRequest.LastName));
			}

			if (!string.IsNullOrEmpty(phoneBookRequest.PhoneNumber))
			{
				query = query.Where(x => x.PhoneNumber.Contains(phoneBookRequest.PhoneNumber));
			}

			phoneBookRequest.PageIndex = phoneBookRequest.PageIndex < 0 ? 0 : phoneBookRequest.PageIndex;
			phoneBookRequest.PageSize = phoneBookRequest.PageSize <= 0 ? 5 : phoneBookRequest.PageSize;

			if (phoneBookRequest.PageSize > 50)
			{
				phoneBookRequest.PageSize = 50;
			}

			if (phoneBookRequest.PageIndex == null || phoneBookRequest.PageSize == null)
			{
				phoneBookRequest.PageIndex = 0;
				phoneBookRequest.PageSize = 5;
			}


			query = phoneBookRequest.sortKey switch
			{
			
				Web_Api.Enums.SortEnums.SortKey.Id => phoneBookRequest.sortType == SortType.Desc
					? query.OrderByDescending(x => x.ID)
					: query.OrderBy(x => x.ID),

				Web_Api.Enums.SortEnums.SortKey.FirstName => phoneBookRequest.sortType == SortType.Desc
					? query.OrderByDescending(x => x.FirstName)
					: query.OrderBy(x => x.FirstName),

				Web_Api.Enums.SortEnums.SortKey.LastName => phoneBookRequest.sortType == SortType.Desc
					? query.OrderByDescending(x => x.LastName)
					: query.OrderBy(x => x.LastName),

			   	_ => query.OrderBy(x => x.LastName)
						  .ThenBy(x => x.FirstName)
						  .ThenBy(x => x.ID),
			};


			int totalCount = await query.CountAsync();


			//if (SortKey == "id")
			//{
			//	query = SortType.ToLower() == "desc" ? query.OrderByDescending(x => x.ID) : query.OrderBy(x => x.ID);
			//}
			//else if (SortKey == "FirstName")
			//{
			//	query = SortType.ToLower() == "desc" ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName);
			//}
			//else if (SortKey == "LastName")
			//{
			//	query = SortType.ToLower() == "desc" ? query.OrderByDescending(x => x.LastName) : query.OrderBy(x => x.LastName);
			//}
			//else if (string.IsNullOrEmpty(SortKey) && string.IsNullOrEmpty(SortType))
			//{
			//	query = query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.ID);
			//};
		

			var data = await query
			.Skip((phoneBookRequest.PageIndex) * phoneBookRequest.PageSize)
			.Take(phoneBookRequest.PageSize)
			.ToListAsync();

			bool isSuccess = data.Any(); 

			return new PagedResponseDto<PhoneBook>(data, totalCount, isSuccess);
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