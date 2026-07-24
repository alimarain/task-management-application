using FluentValidation;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Validators.Task;
public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.ProjectId)
            .GreaterThan(0);

        RuleFor(x => x.AssignedToUserId)
            .GreaterThan(0);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}