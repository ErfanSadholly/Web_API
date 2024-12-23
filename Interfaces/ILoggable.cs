using Web_Api.Migrations;

namespace Web_Api.Interfaces
{
	public interface ILoggable
	{
		DateTime CreatedOn { get; set; }
		int CreatedBy { get; set; }
		DateTime? ModifiedOn { get; set; }
		int? ModifiedBy { get; set; }
	}
}