using Web_Api.DTOs.Base_Class;
using Web_Api.Enums;
using Web_Api.Models.DbModels;
using static Web_Api.Enums.SortEnums;

namespace Web_Api.DTOs
{
	public class PhoneBookRequestDto : PaginationDto
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
	}
}