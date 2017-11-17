namespace Tennis.Models
{
  public class LeadAndTrail<T>
  {
    public T Leader { get; set; }
    public T Trailer { get; set; }
    public bool IsTie { get; set; }
  }
}
