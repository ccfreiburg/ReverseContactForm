namespace ContRev.Backend {
public class EmailTemplate
  {
    public int Id { get; set; }    
    public bool Active { get; set; }    
    public string Purpose { get; set; }
    public string To { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Html { get; set; }
  }
}