using DogsHouse.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Application.Validation.Attributes;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class TailLengthPositiveValueAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        int tailLength = (int)value!;

        if (tailLength < 0)
            throw new ValidationException("Tail length can be only positive.");
        else
            return ValidationResult.Success;
    }
}
