using FluentValidation;
using NDISS.Service.API.DTOs.ProviderService;

namespace NDISS.Service.API.Validator
{
    public class ProviderServiceDtoValidator : AbstractValidator<ProviderServiceBaseDto>
    {
        public ProviderServiceDtoValidator()
        {
            RuleFor(x => x.ProviderId)
                .NotEmpty().WithMessage("ProviderId is required.");

            RuleFor(x => x.ServiceTypeId)
                .NotEmpty().WithMessage("ServiceTypeId is required.");
        }
    }
}
