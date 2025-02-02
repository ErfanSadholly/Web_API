﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Globalization;
using Web_Api.DTOs;
using Web_Api.Helpers;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;
using static Web_Api.Enums.SortEnums;

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

		public PhoneBookService(IPhoneBook phoneBookRepository, IMapper mapper, LoggableEntityService logService, IDatabaseAccess databaseAccess, ClaimService claimService, IHttpContextAccessor httpContext, IAppDbContext dbcontext)
		{
			_phoneBookRepository = phoneBookRepository;
			_mapper = mapper;
			_loggableService = logService;
			_databaseAccess = databaseAccess;
			_claimService = claimService;
			_httpContext = httpContext;
		}

		public async Task<GeneralBasicResponseDto<PagedResponseDto<PhoneBookReadDto>>> GetAllAsync([FromQuery] PhoneBookRequestDto phoneBookRequest)
		{
			var PhoneBook_Paged_ResponseDto = await _phoneBookRepository.GetAllAsync(phoneBookRequest);

			phoneBookRequest.PageSize = phoneBookRequest.PageSize > 50 ? 50 : phoneBookRequest.PageSize;

			if (!PhoneBook_Paged_ResponseDto.Data.Any())
			{
				return new GeneralBasicResponseDto<PagedResponseDto<PhoneBookReadDto>>
				{
					IsSuccess = false,
					Message = ErrorHelper.MessageHelper.NoContactsFound,
					Data = null
				};
			}

			var dtos = new List<PhoneBookReadDto>();

			foreach (var phone in PhoneBook_Paged_ResponseDto.Data)
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

			var pagedResponseDto = new PagedResponseDto<PhoneBookReadDto>(dtos, PhoneBook_Paged_ResponseDto.TotalCount);

			return new GeneralBasicResponseDto<PagedResponseDto<PhoneBookReadDto>>
			{
				IsSuccess = true,
				Data = pagedResponseDto
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
					Message = ErrorHelper.MessageHelper.NoContactsFound,
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
					Message = ErrorHelper.MessageHelper.NoContactsFound,
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
					Message = ErrorHelper.MessageHelper.NoContactsFound,
					Data = null
				};
			}

			if (phonebook.CreatedBy != userId)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = ErrorHelper.MessageHelper.NotAllowEdit,
					Data = null
				};
			}

			_loggableService.SetLoggableEntity(phonebook, userId, false);

			_mapper.Map(phoneBookDTO, phonebook);

			await _phoneBookRepository.UpdateAsync(phonebook);

			return new GeneralBasicResponseDto<PhoneBook>
			{
				IsSuccess = true,
				Data = phonebook
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
					Message = ErrorHelper.MessageHelper.NoContactsFound,
					Data = null
				};
			}

			if (contact.CreatedBy != userId)
			{
				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Message = ErrorHelper.MessageHelper.NotAllowEdit,
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
		public async Task<GeneralBasicResponseDto<PhoneBook>> DeleteByIds(List<int> ids, int userId)
		{
			try
			{
				// فراخوانی متد Repository برای حذف
				await _phoneBookRepository.DeleteByIds(ids, userId);

				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = true,
					Data = new PhoneBook { ID = ids[0] }
				};
			}
			catch (Exception ex)
			{
				// بررسی نوع استثنا و تنظیم پیام مناسب
				string errorMessage = ex.Message;

				return new GeneralBasicResponseDto<PhoneBook>
				{
					IsSuccess = false,
					Data = null,
					Message = errorMessage
				};
			}
		}
	}
}