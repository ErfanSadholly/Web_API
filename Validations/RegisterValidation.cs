using FluentValidation;
using Web_Api.Models.AuthModels;

namespace Web_Api.Validations
{
	public class RegisterValidation : AbstractValidator<RegisterModel>
	{
		public RegisterValidation() 
		{
			RuleFor(x => x.FirstName.Trim())
				.Must(firstname => firstname.All(char.IsLetter)).WithMessage("!اسم باید فقط شامل حروف باشد")
				.MinimumLength(3).WithMessage("!اسم شما باید حداقل 3 کاراکتر باشد")
				.MaximumLength(15).WithMessage("!اسم نمیتواند بیشتر از 15 کاراکتر باشد");

			//=============================================================================================================

			RuleFor(x => x.LastName.Trim()).NotEmpty().WithMessage("!فامیلی نمیتواند خالی باشد")
				.Must(lastname => lastname.All(char.IsLetter)).WithMessage("!فامیلی باید فقط شامل حروف باشد")
				.MinimumLength(3).WithMessage("!فامیلی شما باید حداقل 3 کاراکتر باشد")
				.MaximumLength(20).WithMessage("!فامیلی نمیتواند بیشتر از 20 کاراکتر باشد");

			//=============================================================================================================

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
