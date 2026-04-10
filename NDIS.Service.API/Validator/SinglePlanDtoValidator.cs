using FluentValidation;
using NDIS.Service.API.DTOs.SinglePlan;

namespace NDIS.Service.API.Validator
{
    public class SinglePlanDtoValidator : AbstractValidator<SinglePlanCreateDto>
    {
        public SinglePlanDtoValidator()
        {
            RuleFor(x => x.MenuId)
                .NotEmpty().WithMessage("MenuId is required.");

            RuleFor(x => x.WeeklyPlanId)
                .NotEmpty().WithMessage("WeeklyPlanId is required.");

            RuleFor(x => x.WeekDayId)
                .NotEmpty().WithMessage("WeekDayId is required.");
        }
    }
}
