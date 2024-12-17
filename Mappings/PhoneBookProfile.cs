using AutoMapper;
using Web_Api.DbModels;
using Web_Api.DTOs;

namespace Web_Api.Mappings
{
	public class PhoneBookProfile : Profile 
	{
		public PhoneBookProfile()
		{
			CreateMap<PhoneBook, PhoneBookDTO>().ReverseMap();
		}
	}
}
