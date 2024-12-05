using Microsoft.EntityFrameworkCore;
using Web_Api.DbModels;
using Web_Api.DTOs;
using Web_Api.Interfaces;

namespace Web_Api.Services
{
	public class PhoneBookService
	{
		private readonly IPhoneBook _phoneBookRepository;

		public PhoneBookService (IPhoneBook phoneBookRepository) 
		{
			_phoneBookRepository = phoneBookRepository;
		}

		public async Task<IEnumerable<PhoneBook>> GetAllAsync()
		{
			var phoneBook = await _phoneBookRepository.GetAllAsync();
			if (!phoneBook.Any()) 
			{
				throw new KeyNotFoundException(".هیچ مخاطبی یافت نشد");
			}

			return phoneBook;
		}

		public async Task<PhoneBook> GetByIdAsync(int id) 
		{
			var phoneBook = await _phoneBookRepository.GetByIdAsync(id);
			if (phoneBook == null || phoneBook.Deleted)
			{
				throw new KeyNotFoundException("!مخاطب مورد نظر یافت نشد");	
			}

			return phoneBook;
		}

		public async Task<PhoneBook> CreateAsync(PhoneBookDTO phoneBookDTO) 
		{
			var phonebook = new PhoneBook()
			{
				FirstName = phoneBookDTO.FirstName,
				LastName = phoneBookDTO.LastName,
				PhoneNumber = phoneBookDTO.PhoneNumber,
				Deleted = false
			};

			await _phoneBookRepository.CreateAsync(phonebook);
			return phonebook;
		}

		public async Task<PhoneBook> UpdateAsync(int id, PhoneBookDTO phoneBookDTO)
		{
			var phonebook = await _phoneBookRepository.GetByIdAsync(id);
			if (phonebook == null || phonebook.Deleted)
			{
				throw new KeyNotFoundException("!مخاطب مورد نظر یافت نشد");
			}

			phonebook.FirstName = phoneBookDTO.FirstName;
			phonebook.LastName = phoneBookDTO.LastName;
			phonebook.PhoneNumber = phoneBookDTO.PhoneNumber;

			await _phoneBookRepository.UpdateAsync(phonebook);
			return phonebook;
		}

		public async Task DeleteAsync(int id) 
		{
			var contact = await _phoneBookRepository.GetByIdAsync(id);

			if (contact == null) 
			{
				throw new KeyNotFoundException("!مخاطب مورد نظر یافت نشد");
			}
			if (contact.Deleted) 
			{
				throw new KeyNotFoundException("!مخاطب مورد نظر قبلا حذف شده است");
			}

			await _phoneBookRepository.DeleteAsync(id);
		}
	}
}
