namespace Web_Api.DTOs
{
	public class PagedResponseDto<T>
	{
		public bool IsSuccess { get; set; }
		public int TotalCount { get; set; }
		public List<T> Data { get; set; }

		public PagedResponseDto(List<T> data, int totalCount,bool isSuccess = true)
		{
			IsSuccess = isSuccess;
			TotalCount = totalCount;
			Data = data;
		}
	}
}