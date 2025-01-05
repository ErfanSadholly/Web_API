using static Web_Api.Enums.SortEnums;

namespace Web_Api.DTOs.Base_Class
{
	public class PaginationDto
	{
		public int PageIndex { get; set; } = 0;
		public int PageSize { get; set; } = 5;
		public SortKey sortKey { get; set; } 
		public SortType sortType { get; set; }
	}
}
