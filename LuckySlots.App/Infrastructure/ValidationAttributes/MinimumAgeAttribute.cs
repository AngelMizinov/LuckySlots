namespace LuckySlots.App.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private const string DateTimeFormat = "dd/MM/yyyy";
        private readonly int minAge;

        public MinimumAgeAttribute(int minAgeInput)
        {
            this.minAge = minAgeInput;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value >= DateTime.Now.AddYears(-minAge))
            {
                return new ValidationResult(this.FormatErrorMessage($"{minAge}"));
            }

            return ValidationResult.Success;
        }
    }
}
