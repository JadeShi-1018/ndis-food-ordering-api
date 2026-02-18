using FluentValidation;
using NDISS.Service.API.DTOs.Category;

namespace NDISS.Service.API.Validator
{
    public class CategoryDtoValidator : AbstractValidator<CategoryBaseDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100);

            RuleFor(x => x.CategoryDescription)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.CategoryDescription));
        }
    }
}
