# Saudi National ID Validator

A comprehensive .NET library for validating Saudi National IDs (including both Citizen IDs and Resident/Iqama IDs) with support for data annotations and helper methods.

## Features

- ✅ Validates Saudi National ID format (10 digits)
- ✅ Verifies checksum using Luhn algorithm
- ✅ Identifies ID type (Citizen or Resident)
- ✅ Data annotation support for ASP.NET models
- ✅ Helper methods for programmatic validation
- ✅ .NET Standard 2.0 (compatible with .NET Framework 4.6.1+ and .NET Core 2.0+)

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package SaudiNationalIdValidator
```

Or via Package Manager Console:

```powershell
Install-Package SaudiNationalIdValidator
```

## Usage

### 1. Data Annotation (Recommended for ASP.NET Models)

Use the `[SaudiNationalId]` attribute to validate model properties:

```csharp
using SaudiNationalIdValidator;

public class UserRegistrationModel
{
    [Required]
    [SaudiNationalId(ErrorMessage = "Please enter a valid Saudi National ID")]
    public string NationalId { get; set; }
}
```

#### Advanced Options

**Validate Citizen IDs Only:**

```csharp
public class CitizenModel
{
    [SaudiNationalId(CitizenOnly = true)]
    public string CitizenId { get; set; }
}
```

**Validate Resident IDs Only:**

```csharp
public class ResidentModel
{
    [SaudiNationalId(ResidentOnly = true)]
    public string ResidentId { get; set; }
}
```

### 2. Helper Methods (Programmatic Validation)

Use the static helper methods for manual validation:

```csharp
using SaudiNationalIdValidator;

// Simple validation (returns true/false)
string id = "1234567890";
bool isValid = ValidateSaudiNationalId.IsValid(id);

if (isValid)
{
    Console.WriteLine("Valid ID");
}
else
{
    Console.WriteLine("Invalid ID");
}
```

**Get ID Type:**

```csharp
// Validate and get ID type
SaudiIdType idType = ValidateSaudiNationalId.Validate(id);

switch (idType)
{
    case SaudiIdType.Citizen:
        Console.WriteLine("Valid Saudi Citizen ID");
        break;
    case SaudiIdType.Resident:
        Console.WriteLine("Valid Resident/Iqama ID");
        break;
    case SaudiIdType.Invalid:
        Console.WriteLine("Invalid ID");
        break;
}
```

**Using Out Parameter:**

```csharp
// Validate and get ID type via out parameter
if (ValidateSaudiNationalId.Validate(id, out SaudiIdType type) != SaudiIdType.Invalid)
{
    Console.WriteLine($"Valid ID of type: {type}");
}
```

## Validation Rules

A valid Saudi National ID must:

1. Be exactly **10 digits**
2. Start with **1** (Citizen) or **2** (Resident)
3. Pass the **Luhn algorithm** checksum validation

### Examples

- `1234567890` - Valid if checksum passes (Citizen)
- `2234567890` - Valid if checksum passes (Resident)
- `3234567890` - Invalid (must start with 1 or 2)
- `123456789` - Invalid (must be 10 digits)
- `12345678AB` - Invalid (must be all digits)

## API Reference

### `SaudiIdType` Enum

| Value | Description |
|-------|-------------|
| `Invalid` | Invalid ID or validation failed (-1) |
| `Citizen` | Saudi citizen ID (starts with 1) |
| `Resident` | Resident/Iqama ID (starts with 2) |

### `ValidateSaudiNationalId` Class

| Method | Description |
|--------|-------------|
| `IsValid(string id)` | Returns `true` if the ID is valid |
| `Validate(string id)` | Returns the `SaudiIdType` |
| `Validate(string id, out SaudiIdType idType)` | Returns the type and outputs it via parameter |

### `SaudiNationalIdAttribute` Class

| Property | Description |
|----------|-------------|
| `CitizenOnly` | Only allow citizen IDs (starting with 1) |
| `ResidentOnly` | Only allow resident IDs (starting with 2) |
| `ErrorMessage` | Custom error message |

## License

MIT License

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
