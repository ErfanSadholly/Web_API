using AutoMapper;
using Web_Api.DTOs;
using Web_Api.Models.DbModels;
using Web_Api.Models.Loggable;

namespace Web_Api.Mappings
{
	public class PhoneBookProfile : Profile
	{
		public PhoneBookProfile()
		{
			CreateMap<PhoneBook, PhoneBookReadDto>().ReverseMap()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

			CreateMap<PhoneBookReadDto, LoggableEntity>().ReverseMap()
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => $"{src.CreatedBy}"))
				.ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy.HasValue ? $"{src.ModifiedBy}" : "N/A"))
				.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.ModifiedOn));

			CreateMap<PhoneBook, PhoneBookWriteDTO>().ReverseMap();
		}
	}
}