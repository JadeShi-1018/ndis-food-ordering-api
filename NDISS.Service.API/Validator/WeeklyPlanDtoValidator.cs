using FluentValidation;
using NDISS.Service.API.DTOs.WeeklyPlan;

namespace NDISS.Service.API.Validator
{
    public class WeeklyPlanDtoValidator : AbstractValidator<WeeklyPlanBaseDto>
    {
        public WeeklyPlanDtoValidator()
        {
            RuleFor(x => x.PlanName)
                .NotEmpty().WithMessage("PlanName is required.")
                .MaximumLength(100).WithMessage("PlanName cannot exceed 100 characters.");

            RuleFor(x => x.PlanPrice)
                .GreaterThanOrEqualTo(0).WithMessage("PlanPrice must be a non-negative number.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
