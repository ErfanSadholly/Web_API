using FluentValidation;
using Web_Api.Account_Models;

namespace Web_Api.Validations
{
	public class LoginValidation : AbstractValidator<LoginModel> 
	{
		public LoginValidation() 
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(".ایمیل خود را وارد کنید")
				.EmailAddress().WithMessage(".ایمیل خود را به درستی وارد کنید");

			//================================================================================================

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage(".پسورد خود را وارد کنید");
		}
	}
}
