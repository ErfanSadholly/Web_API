using Web_Api.Interfaces;

namespace Web_Api.Models.Loggable
{
	public class LoggableEntity : ILoggable
	{
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public string CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? ModifiedBy { get; set; }
	}
}
