namespace ContRev.Backend;

using System.ComponentModel.DataAnnotations;

public class MessageRequest
{
    [Required]
    public string Slug { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Message { get; set; }
}