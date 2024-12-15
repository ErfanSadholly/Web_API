namespace Web_Api.DTOs
{
	public class GeneralBasicResponseDto<T> : BasicResponseDto
	{
		public T Data { get; set; }
	}
}
