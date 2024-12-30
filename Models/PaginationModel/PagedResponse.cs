namespace Web_Api.Models.PaginationModel
{
	public class PagedResponse<T>
	{
		public List<T> Data { get; set; }
		public int TotalCount { get; set; } 

		public PagedResponse(List<T> data, int totalCount)
		{
			Data = data;
			TotalCount = totalCount;
		}
	}
}