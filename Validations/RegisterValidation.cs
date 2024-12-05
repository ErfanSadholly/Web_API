using FluentValidation;
using Web_Api.Account_Models;

namespace Web_Api.Validations
{
	public class RegisterValidation : AbstractValidator<RegisterModel>
	{
		public RegisterValidation() 
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(".ایمیل خود را وارد کنید")
				.EmailAddress().WithMessage(".ایمیل خود را به درستی وارد کنید");

			//================================================================================================

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage(".پسورد خود را وارد کنید")
				.MinimumLength(5).WithMessage(".رمز عبور شما باید حداقل 5 کاراکتر باشد")
				.MaximumLength(20).WithMessage(".رمز عبور شما باید حداکثر 20 کاراکتر باشد")
				.Must(password => password.Any(char.IsUpper) && password[0] == char.ToUpper(password[0])).WithMessage("پسورد باید با یک حرف بزرگ شروع شود.")
				.Must(password => password.Any(char.IsLetter) && password.Any(char.IsDigit)).WithMessage("پسورد باید شامل حروف و اعداد باشد.");

			//================================================================================================

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage(".پسورد خود را مجددا وارد کنید")
				.Equal(x => x.Password).WithMessage("!پسورد ها یکسان نیست");
		}
	}
}
