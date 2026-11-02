using System.ComponentModel.DataAnnotations;

namespace CollectPoint.Core.DTO.Request;

public class CreateCustomerRequest
{
    [Required, MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "AccountNumber must be 10 digits.")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required, RegularExpression("^(USD|GBP|EUR)$", ErrorMessage = "Currency must be USD, GBP or EUR.")]
    public string Currency { get; set; } = string.Empty;
}
