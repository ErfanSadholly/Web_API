using AutoMapper;
using Web_Api.DTOs;
using Web_Api.Models.DbModels;

namespace Web_Api.Mappings
{
	public class PhoneBookProfile : Profile 
	{
		public PhoneBookProfile()
		{
			// Mapping برای GetAll
			// Mapping برای GetById
			CreateMap<PhoneBook, PhoneBookReadDto>().ReverseMap();
				
			//Mapping برای Create
			//Mapping برای Update
			CreateMap<PhoneBook, PhoneBookWriteDTO>().ReverseMap();
		}
	}
}
