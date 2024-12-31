﻿using Microsoft.EntityFrameworkCore;
using Web_Api.DTOs;
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

		public async Task<PagedResponseDto<PhoneBook>> GetAllAsync(string? FirstName, string? LastName, string? PhoneNumber, int PageIndex, int PageSize)
		{
			IQueryable<PhoneBook> query = _context.PhoneBooks;

			if (!string.IsNullOrEmpty(FirstName))
			{
				query = query.Where(x => x.FirstName == FirstName);
			}

			if (!string.IsNullOrEmpty(LastName))
			{
				query = query.Where(x => x.LastName.Contains(LastName));
			}

			if (!string.IsNullOrEmpty(PhoneNumber))
			{
				query = query.Where(x => x.PhoneNumber.Contains(PhoneNumber));
			}

			PageIndex = PageIndex < 0 ? 0 : PageIndex;
			PageSize = PageSize <= 0 ? 5 : PageSize;

			if (PageSize > 50)
			{
				PageSize = 50;
			}

			if (PageIndex == null || PageSize == null) 
			{
				PageIndex = 0;
				PageSize = 5;
			}

			int totalCount = await query.CountAsync();


			query = query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);

			 var data = await query
			.Skip((PageIndex) * PageSize) 
			.Take(PageSize)
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