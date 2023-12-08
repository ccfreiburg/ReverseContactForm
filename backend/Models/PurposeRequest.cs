namespace ContRev.Backend;

using System.ComponentModel.DataAnnotations;

public class PurposeRequest
{
    [Required]
    public string Purpose { get; set; }

    [Required]
    public string Slug { get; set; }

}