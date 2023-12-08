namespace ContRev.Backend;

using System.ComponentModel.DataAnnotations;

public class TemplateRequest
{
    [Required]
    public bool Active { get; set; }
   
    [Required]
    public string Slug { get; set; }

    [Required]
    public string Purpose { get; set; }

    [Required]
    public string To { get; set; }

    [Required]
    public string From { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Html { get; set; }

}