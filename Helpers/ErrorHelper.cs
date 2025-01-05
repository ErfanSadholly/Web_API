namespace Web_Api.Helpers
{
	public class ErrorHelper
	{
		public static class MessageHelper
		{
			public static string NoContactsFound = "!مخاطبی یافت نشد";

			public static string NotAllowEdit = "!شما اجازه ویرایش این مخاطب را ندارید";

			public static string NotAllowDelete = "!شما اجازه حذف این مخاطب را ندارید";

			public static string DuplicateEmail = ".ایمیل قبلاً ثبت شده است";

			public static string ErorrRegister = ".ثبت نام با خطا مواجه شد";

			public static string InvalidPassword = ".رمز عبور اشتباه است";

			public static string UserNotFound = ".کاربر یافت نشد";

			public static string Error500 = "خطای داخلی سرور";
		}
	}
}
