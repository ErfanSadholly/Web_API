using AutoMapper;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Web_Api.DTOs;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;

namespace Web_Api.Services
{
	public class PhoneBookService
	{
		private readonly IPhoneBook _phoneBookRepository;
		private readonly IMapper _mapper;
		private readonly LoggableEntityService _logService;

		public PhoneBookService (IPhoneBook phoneBookRepository, IMapper mapper, LoggableEntityService logService)
		{
			_phoneBookRepository = phoneBookRepository;
			_mapper = mapper;
			_logService = logService;
		}

		public async Task<GeneralBasicResponseDto<List<PhoneBookReadDto>>> GetAllAsync()
		{
			var phoneBook = await _phoneBookRepository.GetAllAsync();
			if (!phoneBook.Any())
			{
				return new GeneralBasicResponseDto<List<PhoneBookReadDto>>
				{
					IsSuccess = false,
					Message = ".هیچ مخاطبی یافت نشد",
					Data = null
				};
			}

			//var phoneBookDto = phoneBook
			//	.Where(pb => !pb.Deleted) // حذف موارد حذف‌شده
			//	.Select(pb => _mapper.Map<PhoneBookReadDto>(pb)) // تبدیل به DTO
			//	.ToList();

			var MapphoneBookReadDto = _mapper.Map<List<PhoneBookReadDto>>(phoneBook);

			return new GeneralBasicResponseDto<List<PhoneBookReadDto>>
			{
				IsSuccess = true,
				Data = MapphoneBookReadDto
			};
		}
		public async Task<GeneralBasicResponseDto<PhoneBookReadDto>> GetByIdAsync(int id)
		{
			var phoneBook = await _phoneBookRepository.GetByIdAsync(id);
			if (phoneBook == null || phoneBook.Deleted)
			{
				return new GeneralBasicResponseDto<PhoneBookReadDto>
				{
					IsSuccess = false,
					Message = "!مخاطب مورد نظر یافت نشد",
					Data = null
				};
			}

			var entityDto = _mapper.Map<PhoneBookReadDto>(phoneBook);

			return new GeneralBasicResponseDto<PhoneBookReadDto>()
			{
				IsSuccess = true,
				Data = entityDto
			};
		}
		public async Task<GeneralBasicResponseDto<PhoneBook>> CreateAsync(PhoneBookWriteDTO phoneBookDTO, int userId)
		{
			var phonebook = _mapper.Map<PhoneBook>(phoneBookDTO);

			_logService.SetLoggableEntity(phonebook, userId, true);

			await _phoneBookRepository.CreateAsync(phonebook);

			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Data = phonebook
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> UpdateAsync(int id, PhoneBookWriteDTO phoneBookDTO , int userId)
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

			_logService.SetLoggableEntity(phonebook, userId, false);

			_mapper.Map(phoneBookDTO, phonebook);

			await _phoneBookRepository.UpdateAsync(phonebook);
			
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
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
				Data = null
			};
		}
	}
}