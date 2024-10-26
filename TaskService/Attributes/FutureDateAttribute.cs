using System.ComponentModel.DataAnnotations;

namespace TaskService.Attributes;

public class FutureDateAttribute : ValidationAttribute
{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTimeOffset date)
        {
            if (date > DateTimeOffset.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date must be in the future");
            }
        }

        return new ValidationResult("Invalid date format");
    }
}