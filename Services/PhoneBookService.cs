using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using Web_Api.DTOs;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;

namespace Web_Api.Services
{
	public class PhoneBookService
	{
		private readonly IPhoneBook _phoneBookRepository;
		private readonly IMapper _mapper;
		private readonly LoggableEntityService _loggableService;
		private readonly IDatabaseAccess _databaseAccess;
		private readonly ClaimService _claimService;
		private readonly IHttpContextAccessor _httpContext;

		public PhoneBookService(IPhoneBook phoneBookRepository, IMapper mapper, LoggableEntityService logService, IDatabaseAccess databaseAccess, ClaimService claimService, IHttpContextAccessor httpContext)
		{
			_phoneBookRepository = phoneBookRepository;
			_mapper = mapper;
			_loggableService = logService;
			_databaseAccess = databaseAccess;
			_claimService = claimService;
			_httpContext = httpContext;
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

			var dtos = new List<PhoneBookReadDto>();

			foreach (var phone in phoneBook)
			{
				var dto = _mapper.Map<PhoneBookReadDto>(phone);


				var createdByUser = await _databaseAccess.GetUserFullNameWithDapper(phone.CreatedBy);
				dto.CreatedBy = createdByUser;


				if (phone.ModifiedBy.HasValue)
				{
					var modifiedByUser = await _databaseAccess.GetUserFullNameWithDapper(phone.ModifiedBy.Value);
					dto.ModifiedBy = modifiedByUser;
				}
				else
				{
					dto.ModifiedBy = string.Empty;
				}

				var canEdit = await _databaseAccess.CanEditPhoneBook(phone.CreatedBy, phone.ID);

				var userId = _claimService.GetUserIdFromClaims(_httpContext.HttpContext.User);
				dto.AllowEdit = phone.CreatedBy == userId;

				dtos.Add(dto);
			}
				//	var MapphoneBookReadDto = _mapper.Map<List<PhoneBookReadDto>>(phoneBook);

				return new GeneralBasicResponseDto<List<PhoneBookReadDto>>
				{ 
					IsSuccess = true,
					Data = dtos
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

			// دریافت نام و نام خانوادگی کاربر ایجادکننده
			var createdByUser = await _databaseAccess.GetUserFullNameWithDapper(phoneBook.CreatedBy);
			entityDto.CreatedBy = createdByUser;

			// اگر اطلاعات مربوط به کاربر ویرایش‌کننده موجود است
			if (phoneBook.ModifiedBy.HasValue)
			{
				var modifiedByUser = await _databaseAccess.GetUserFullNameWithDapper(phoneBook.ModifiedBy.Value);
				entityDto.ModifiedBy = modifiedByUser;
			}
			else
			{
				entityDto.ModifiedBy = string.Empty; // اگر هیچ تغییری توسط کاربری انجام نشده باشد
			}

			var canEdit = _databaseAccess.CanEditPhoneBook(phoneBook.CreatedBy, phoneBook.ID);
			var UserId = _claimService.GetUserIdFromClaims(_httpContext.HttpContext.User);
			if (UserId == -1)
			{
				return new GeneralBasicResponseDto<PhoneBookReadDto>
				{
					IsSuccess = false,
					Message = "کاربر شناسایی نشد",
					Data = null
				};
			}

			entityDto.AllowEdit = phoneBook.CreatedBy == UserId;

			return new GeneralBasicResponseDto<PhoneBookReadDto>()
			{
				IsSuccess = true,
				Data = entityDto
			};
		}
		
		public async Task<GeneralBasicResponseDto<PhoneBook>> CreateAsync(PhoneBookWriteDTO writeDTO, int userId)
		{
			var phonebook = _mapper.Map<PhoneBook>(writeDTO);

			_loggableService.SetLoggableEntity(phonebook, userId, true);


			await _phoneBookRepository.CreateAsync(phonebook);

			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Data = new PhoneBook { ID = phonebook.ID }
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> UpdateAsync(int id, PhoneBookWriteDTO phoneBookDTO, int userId)
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

			if (phonebook.CreatedBy != userId)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = "!شما اجازه ویرایش این مخاطب را ندارید",
					Data = null
				};
			}

			_loggableService.SetLoggableEntity(phonebook, userId, false);

			_mapper.Map(phoneBookDTO, phonebook);

			await _phoneBookRepository.UpdateAsync(phonebook);

			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Data = new PhoneBook { ID = phonebook.ID }
			};
		}

		public async Task<GeneralBasicResponseDto<PhoneBook>> DeleteAsync(int id, int userId)
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

			if (contact.CreatedBy != userId)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = "!شما اجازه حذف این مخاطب را ندارید",
					Data = null
				};
			}

			await _phoneBookRepository.DeleteAsync(id);
			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Data = new PhoneBook { ID = id }
			};
		}
	}
}