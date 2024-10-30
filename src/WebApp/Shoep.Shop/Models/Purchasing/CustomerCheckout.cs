using System.ComponentModel.DataAnnotations;

namespace Shoep.Shop.Models.Purchasing;

public class CustomerCheckout
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string? Lastname { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string? AddressLine { get; set; }
    public string Country { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
    public string? Note { get; set; }
    public string CardName { get; set; } = default!;

    [Required(ErrorMessage = "Card number is required.")]
    public string CardNumber { get; set; } = default!;

    [Required(ErrorMessage = "Expiration date is required.")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Expiration date must be in MM/YY format.")]
    public string Expiration { get; set; } = default!;

    [Required(ErrorMessage = "CVV is required.")]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
    public string Cvv { get; set; } = default!;
}