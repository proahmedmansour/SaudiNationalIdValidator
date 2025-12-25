using System;
using System.ComponentModel.DataAnnotations;
using SaudiNationalIdValidator;

namespace TestApp
{
    // Test model with data annotations
    public class UserModel
    {
        [Required]
        [SaudiNationalId(ErrorMessage = "Please provide a valid Saudi National ID")]
        public string NationalId { get; set; }
    }

    public class CitizenModel
    {
        [SaudiNationalId(CitizenOnly = true)]
        public string CitizenId { get; set; }
    }

    public class ResidentModel
    {
        [SaudiNationalId(ResidentOnly = true)]
        public string ResidentId { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Saudi National ID Validator Test ===\n");

            // Test 1: Helper method - IsValid
            Console.WriteLine("Test 1: Helper Method - IsValid()");
            TestIsValid("1234567890");
            TestIsValid("2345678901");
            TestIsValid("3456789012");
            TestIsValid("123456789");
            TestIsValid("12345678AB");
            TestIsValid("");
            TestIsValid(null);
            Console.WriteLine();

            // Test 2: Helper method - Validate with type
            Console.WriteLine("Test 2: Helper Method - Validate() with Type");
            TestValidateWithType("1234567890");
            TestValidateWithType("2345678901");
            TestValidateWithType("3456789012");
            Console.WriteLine();

            // Test 3: Data Annotation - General validation
            Console.WriteLine("Test 3: Data Annotation - General Validation");
            TestDataAnnotation(new UserModel { NationalId = "1234567890" });
            TestDataAnnotation(new UserModel { NationalId = "2345678901" });
            TestDataAnnotation(new UserModel { NationalId = "invalid" });
            TestDataAnnotation(new UserModel { NationalId = "" });
            Console.WriteLine();

            // Test 4: Data Annotation - Citizen only
            Console.WriteLine("Test 4: Data Annotation - Citizen Only");
            TestCitizenOnly(new CitizenModel { CitizenId = "1234567890" });
            TestCitizenOnly(new CitizenModel { CitizenId = "2345678901" });
            Console.WriteLine();

            // Test 5: Data Annotation - Resident only
            Console.WriteLine("Test 5: Data Annotation - Resident Only");
            TestResidentOnly(new ResidentModel { ResidentId = "2345678901" });
            TestResidentOnly(new ResidentModel { ResidentId = "1234567890" });
            Console.WriteLine();

            Console.WriteLine("=== All Tests Completed ===");
        }

        static void TestIsValid(string id)
        {
            bool isValid = ValidateSaudiNationalId.IsValid(id);
            Console.WriteLine($"  IsValid(\"{id ?? "null"}\"): {isValid}");
        }

        static void TestValidateWithType(string id)
        {
            SaudiIdType type = ValidateSaudiNationalId.Validate(id);
            Console.WriteLine($"  Validate(\"{id}\"): {type}");
        }

        static void TestDataAnnotation(UserModel model)
        {
            var context = new ValidationContext(model);
            var results = new System.Collections.Generic.List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            
            Console.WriteLine($"  ID: \"{model.NationalId}\" - Valid: {isValid}");
            if (!isValid)
            {
                foreach (var error in results)
                {
                    Console.WriteLine($"    Error: {error.ErrorMessage}");
                }
            }
        }

        static void TestCitizenOnly(CitizenModel model)
        {
            var context = new ValidationContext(model);
            var results = new System.Collections.Generic.List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            
            Console.WriteLine($"  Citizen ID: \"{model.CitizenId}\" - Valid: {isValid}");
            if (!isValid)
            {
                foreach (var error in results)
                {
                    Console.WriteLine($"    Error: {error.ErrorMessage}");
                }
            }
        }

        static void TestResidentOnly(ResidentModel model)
        {
            var context = new ValidationContext(model);
            var results = new System.Collections.Generic.List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            
            Console.WriteLine($"  Resident ID: \"{model.ResidentId}\" - Valid: {isValid}");
            if (!isValid)
            {
                foreach (var error in results)
                {
                    Console.WriteLine($"    Error: {error.ErrorMessage}");
                }
            }
        }
    }
}
