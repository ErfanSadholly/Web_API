using Web_Api.Models.Loggable;

namespace Web_Api.Services
{
	public class LoggableEntityService
	{
		public void SetLoggableEntity(LoggableEntity entity, string userId, bool IsNew)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("Email Null!", nameof(userId));
			}

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
