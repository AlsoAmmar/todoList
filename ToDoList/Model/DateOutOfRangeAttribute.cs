using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Model;

public class DateOutOfRangeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var today = DateTime.Now - TimeSpan.FromMinutes(5);
        var date = (DateTime)value!;

        if (date < today)
        {
            return new ValidationResult(ErrorMessage ?? "Invalid date");
        }
        
        return ValidationResult.Success;
    }
}