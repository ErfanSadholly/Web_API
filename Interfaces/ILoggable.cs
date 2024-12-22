using Web_Api.Migrations;

namespace Web_Api.Interfaces
{
	public interface ILoggable
	{
		DateTime CreatedOn { get; set; }
		string CreatedBy { get; set; }
		DateTime? ModifiedOn { get; set; }
		string ModifiedBy { get; set; }
	}
}

