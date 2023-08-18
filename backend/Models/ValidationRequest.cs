namespace ContRev.Backend;

using System.ComponentModel.DataAnnotations;

public class ValidationRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Purpose { get; set; }
    public string Language { get; set; }
}