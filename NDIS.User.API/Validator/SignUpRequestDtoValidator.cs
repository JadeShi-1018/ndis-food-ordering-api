using FluentValidation;
using NDIS.User.API.DTOs;

namespace NDIS.User.API.Validator

{
  public class SignUpRequestDtoValidator : AbstractValidator<SignUpRequestDto>
  {
    public SignUpRequestDtoValidator()
    {
      RuleFor(x => x.UserName).NotEmpty().WithMessage("User name can not be empty");
      RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("invalid email address");
      RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Password should be at least 8 letters");

    }
  }
}
