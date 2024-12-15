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

		public async Task<GeneralBasicResponseDto<List<PhoneBook>>> GetAllAsync()
		{
			var phoneBook = await _phoneBookRepository.GetAllAsync();
			if (!phoneBook.Any())
			{
				return new GeneralBasicResponseDto<List<PhoneBook>>
				{
					IsSuccess = false,
					Message = ".هیچ مخاطبی یافت نشد",
					Data = null

				};
			}
			return new GeneralBasicResponseDto<List<PhoneBook>>
			{
				IsSuccess = true,
				Message = ".مخاطبین با موفقیت بازیابی شدند",
				Data = phoneBook.ToList()
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> GetByIdAsync(int id)
		{
			var phoneBook = await _phoneBookRepository.GetByIdAsync(id);
			if (phoneBook == null || phoneBook.Deleted)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = "!مخاطب مورد نظر یافت نشد",
					Data = null
				};
			}
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Message = ".مخاطب با موفقیت بازیابی شد",
				Data = phoneBook
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> CreateAsync(PhoneBookDTO phoneBookDTO) 
		{
			var phonebook = new PhoneBook()
			{
				FirstName = phoneBookDTO.FirstName,
				LastName = phoneBookDTO.LastName,
				PhoneNumber = phoneBookDTO.PhoneNumber,
				Deleted = false
			};

			await _phoneBookRepository.CreateAsync(phonebook);
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Message = ".مخاطب جدید با موفقیت ایجاد شد",
				Data = phonebook
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> UpdateAsync(int id, PhoneBookDTO phoneBookDTO)
		{
			var phonebook = await _phoneBookRepository.GetByIdAsync(id);
			if (phonebook == null || phonebook.Deleted)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = "!مخاطب مورد نظر یافت نشد",
					Data = null
				};
			}

			phonebook.FirstName = phoneBookDTO.FirstName;
			phonebook.LastName = phoneBookDTO.LastName;
			phonebook.PhoneNumber = phoneBookDTO.PhoneNumber;

			await _phoneBookRepository.UpdateAsync(phonebook);
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Message = ".مخاطب با موفقیت به‌روز رسانی شد",
				Data = phonebook
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> DeleteAsync(int id) 
		{
			var contact = await _phoneBookRepository.GetByIdAsync(id);

			if (contact == null) 
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = "!مخاطب مورد نظر یافت نشد",
					Data = null
				};
			}

			await _phoneBookRepository.DeleteAsync(id);
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Message = ".مخاطب با موفقیت حذف شد",
				Data = null
			};
		}
	}
}
