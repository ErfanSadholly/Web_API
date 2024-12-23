using Web_Api.Models.Loggable;

namespace Web_Api.Services
{
	public class LoggableEntityService
	{
		public void SetLoggableEntity(LoggableEntity entity, int userId, bool IsNew)
		{
			if (IsNew)
			{
				entity.CreatedOn = DateTime.Now;
				entity.CreatedBy = userId;
			}
			else
			{
				entity.ModifiedOn = DateTime.Now;
				entity.ModifiedBy = userId;
			}
		}
	}
}
