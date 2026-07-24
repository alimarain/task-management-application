using FluentValidation;
using TaskManagementApi.DTOs.Auth;

namespace TaskManagementApi.Validators.Auth;

public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}