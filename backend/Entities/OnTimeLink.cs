namespace ContRev.Backend {
public class OneTimeLink
  {
    public int Id { get; set; }    
    public string Slug { get; set; }
    public string Email { get; set; }
    public string Purpose { get; set; }
    public string ValidTo { get; set; }
  }
}