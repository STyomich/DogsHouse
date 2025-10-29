using DogsHouse.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Application.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class UniqueDogNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dogName = value as string;

        if (string.IsNullOrEmpty(dogName))
        {
            throw new ValidationException("Dog name cannot be empty.");
        }

        var unitOfWork = validationContext.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
        bool exists = unitOfWork!.DogsRepository.ExistsByNameAsync(dogName, cancellationToken: default).Result;

        return exists ? new ValidationResult($"Dog with name '{dogName}' already exists.") : ValidationResult.Success;
    }
}