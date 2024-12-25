using Microsoft.AspNetCore.Identity;

namespace Web_Api.Models.DbModels
{
	public class User : IdentityUser<int>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }		
	}
}
