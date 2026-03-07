using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToDoList.Model;

public class NoEmojiAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string str)
            return ValidationResult.Success;
        
        var emojiRegex = new Regex(@"\p{Cs}|\p{So}", RegexOptions.Compiled);

        if (emojiRegex.IsMatch(str))
        {
            return new ValidationResult(ErrorMessage ?? "Emojis are not allowed");
        }

        return ValidationResult.Success;
    }
}