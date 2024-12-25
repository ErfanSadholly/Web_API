namespace Web_Api.DTOs
{
	public class PhoneBookReadDto
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? ModifiedBy { get; set; }
		public bool AllowEdit { get; set; }
	}
}