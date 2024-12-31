﻿using Web_Api.DTOs;
using Web_Api.Models.DbModels;

namespace Web_Api.Interfaces
{
	public interface IPhoneBook
	{
		Task<PagedResponseDto<PhoneBook>> GetAllAsync(string? FirstName, string? LastName, string? PhoneNumber, int PageIndex, int PageSize);
		Task<PhoneBook> GetByIdAsync(int id);		
		Task CreateAsync(PhoneBook contact);
		Task UpdateAsync(PhoneBook contact);
		Task DeleteAsync(int id); // Soft Delete
	}
}
