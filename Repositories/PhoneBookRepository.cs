using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Web_Api.DTOs;
using Web_Api.Helpers;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
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

		public async Task<PagedResponseDto<PhoneBook>> GetAllAsync(PhoneBookRequestDto phoneBookRequest)
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

			if (phoneBookRequest.PageSize > 50)
			{
				phoneBookRequest.PageSize = 50;
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

		public async Task UpdateAsync(PhoneBook
			contact)
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

		public async Task DeleteByIds(List<int> ids, int userId)
		{
			if (ids == null || !ids.Any() || ids.Any(id => id <= -1)) // چک کردن IDs نامعتبر
			{
				throw new Exception(ErrorHelper.MessageHelper.NoContactsFound);
			}

			var phoneBooks = await _context.PhoneBooks
				.Where(pb => ids.Contains(pb.ID) && !pb.Deleted)
				.ToListAsync();

			if (!phoneBooks.Any())
			{
				throw new Exception(ErrorHelper.MessageHelper.NoContactsFound);
			}

			// بررسی رکوردهایی که متعلق به کاربر نیستند
			var unauthorizedRecords = phoneBooks.Where(pb => pb.CreatedBy != userId).ToList();

			// اگر هیچ‌کدام از مخاطب‌ها متعلق به کاربر نباشند
			if (unauthorizedRecords.Any() && phoneBooks.Count == unauthorizedRecords.Count)
			{
				throw new Exception(ErrorHelper.MessageHelper.NotAllowDelete);
			}

			// حذف مخاطب‌هایی که متعلق به کاربر هستند
			foreach (var phoneBook in phoneBooks)
			{
				if (phoneBook.CreatedBy == userId) // فقط حذف مخاطب‌هایی که متعلق به کاربر هستند
				{
					phoneBook.Deleted = true;
					phoneBook.ModifiedOn = DateTime.UtcNow;
					phoneBook.ModifiedBy = userId;
				}
			}
			await _context.SaveChangesAsync();
		}
	}
}