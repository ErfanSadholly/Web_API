using AutoMapper;
using Web_Api.Account_Models;
using Web_Api.DbModels;

namespace Web_Api.Mappings
{
	public class AuthenticationProfile : Profile
	{
		public AuthenticationProfile() 
		{
			CreateMap<RegisterModel, User>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // تطابق با UserName
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));  // تطابق با Email
		}
	}
}
