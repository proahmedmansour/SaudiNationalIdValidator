using System;
using System.ComponentModel.DataAnnotations;

namespace SaudiNationalIdValidator
{
    /// <summary>
    /// Validates that a property value is a valid Saudi National ID.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SaudiNationalIdAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow only citizen IDs (starting with 1).
        /// </summary>
        public bool CitizenOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow only resident IDs (starting with 2).
        /// </summary>
        public bool ResidentOnly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaudiNationalIdAttribute"/> class.
        /// </summary>
        public SaudiNationalIdAttribute()
        {
            ErrorMessage = "The {0} field is not a valid Saudi National ID.";
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of <see cref="ValidationResult"/>.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Null or empty values are considered valid (use [Required] for mandatory fields)
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            string id = value.ToString()!;
            SaudiIdType idType = ValidateSaudiNationalId.Validate(id);

            // Check if ID is valid
            if (idType == SaudiIdType.Invalid)
            {
                string memberName = validationContext?.MemberName ?? "field";
                string errorMessage = FormatErrorMessage(memberName);
                return new ValidationResult(errorMessage, validationContext?.MemberName != null 
                    ? new[] { validationContext.MemberName } 
                    : Array.Empty<string>());
            }

            // Check citizen-only constraint
            if (CitizenOnly && idType != SaudiIdType.Citizen)
            {
                string memberName = validationContext?.MemberName ?? "field";
                return new ValidationResult(
                    $"The {memberName} field must be a Saudi Citizen ID (starting with 1).",
                    validationContext?.MemberName != null 
                        ? new[] { validationContext.MemberName } 
                        : Array.Empty<string>());
            }

            // Check resident-only constraint
            if (ResidentOnly && idType != SaudiIdType.Resident)
            {
                string memberName = validationContext?.MemberName ?? "field";
                return new ValidationResult(
                    $"The {memberName} field must be a Resident ID (starting with 2).",
                    validationContext?.MemberName != null 
                        ? new[] { validationContext.MemberName } 
                        : Array.Empty<string>());
            }

            return ValidationResult.Success;
        }
    }
}
