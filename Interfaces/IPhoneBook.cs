using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Web_Api.DTOs;
using Web_Api.Models.DbModels;
using static Web_Api.Enums.SortEnums;

namespace Web_Api.Interfaces
{
	public interface IPhoneBook
	{
		Task<PagedResponseDto<PhoneBook>> GetAllAsync(PhoneBookRequestDto phoneBookRequest);
		Task<PhoneBook> GetByIdAsync(int id);		
		Task CreateAsync(PhoneBook contact);
		Task UpdateAsync(PhoneBook contact);
		Task DeleteAsync(int id); // Soft Delete
		Task DeleteByIds(List<int> ids, int userId);
	}
}
