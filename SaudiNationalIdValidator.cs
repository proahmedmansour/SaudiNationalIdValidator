using System.Text.RegularExpressions;

namespace SaudiNationalIdValidator
{
    /// <summary>
    /// Represents the type of Saudi National ID.
    /// </summary>
    public enum SaudiIdType
    {
        /// <summary>
        /// Invalid ID or validation failed.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// Saudi citizen ID (starts with 1).
        /// </summary>
        Citizen = 1,

        /// <summary>
        /// Resident (Iqama) ID (starts with 2).
        /// </summary>
        Resident = 2
    }

    /// <summary>
    /// Provides helper methods for validating Saudi National IDs.
    /// </summary>
    public static class ValidateSaudiNationalId
    {
        /// <summary>
        /// Validates a Saudi National ID and returns whether it is valid.
        /// </summary>
        /// <param name="id">The ID to validate.</param>
        /// <returns>True if the ID is valid; otherwise, false.</returns>
        public static bool IsValid(string id)
        {
            return Validate(id, out _) != SaudiIdType.Invalid;
        }

        /// <summary>
        /// Validates a Saudi National ID and returns the ID type.
        /// </summary>
        /// <param name="id">The ID to validate.</param>
        /// <param name="idType">When this method returns, contains the type of ID if validation succeeds; otherwise, Invalid.</param>
        /// <returns>The type of ID if valid; otherwise, Invalid.</returns>
        public static SaudiIdType Validate(string id, out SaudiIdType idType)
        {
            idType = ValidateInternal(id);
            return idType;
        }

        /// <summary>
        /// Validates a Saudi National ID and returns the ID type.
        /// </summary>
        /// <param name="id">The ID to validate.</param>
        /// <returns>The type of ID if valid; otherwise, Invalid.</returns>
        public static SaudiIdType Validate(string id)
        {
            return ValidateInternal(id);
        }

        private static SaudiIdType ValidateInternal(string id)
        {
            // Check for null or whitespace
            if (string.IsNullOrWhiteSpace(id))
                return SaudiIdType.Invalid;

            // Trim and validate format (exactly 10 digits)
            id = id.Trim();
            if (!Regex.IsMatch(id, @"^\d{10}$"))
                return SaudiIdType.Invalid;

            // Check ID type (first digit must be 1 or 2)
            int type = id[0] - '0';
            if (type != 1 && type != 2)
                return SaudiIdType.Invalid;

            // Validate checksum using Luhn algorithm
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = id[i] - '0';

                // Double every other digit starting from the first
                if (i % 2 == 0)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
            }

            // Calculate expected check digit
            int expected = (10 - (sum % 10)) % 10;
            int actual = id[9] - '0';

            // Return type if checksum is valid
            return expected == actual ? (SaudiIdType)type : SaudiIdType.Invalid;
        }
    }
}
