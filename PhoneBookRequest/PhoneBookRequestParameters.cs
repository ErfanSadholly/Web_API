using Web_Api.DTOs;
using Web_Api.Enums;
using Web_Api.Models.DbModels;
using static Web_Api.Enums.SortEnums;

namespace Web_Api.PhoneBookRequest
{
	public class PhoneBookRequestParameters
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public SortKey sortKey { get; set; }
		public SortType sortType { get; set; }
	}
}