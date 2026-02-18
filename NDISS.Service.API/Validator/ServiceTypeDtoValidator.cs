using FluentValidation;
using NDISS.Service.API.DTOs.ServiceType;

namespace NDISS.Service.API.Validator
{
    public class ServiceTypeDtoValidator : AbstractValidator<ServiceTypeBaseDto>
    {
        public ServiceTypeDtoValidator()
        {
            RuleFor(x => x.ServiceTypeName)
                .NotEmpty().WithMessage("Service type name is required.")
                .MaximumLength(100);

            RuleFor(x => x.ServiceDescription)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ServiceDescription));
        }
    }
}
