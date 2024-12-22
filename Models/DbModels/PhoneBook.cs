using Microsoft.AspNetCore.Http.HttpResults;
using Web_Api.Migrations;


namespace Web_Api.Models.DbModels
{
	public class PhoneBook : Loggable.LoggableEntity
	{
		public int ID { get; set; }
		public string? FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public bool Deleted { get; set; } = false;
	}
}
