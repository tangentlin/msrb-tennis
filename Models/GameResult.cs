namespace Tennis.Models
{
  public class GameResult
  {
    public GameProgressSummary ProgressSummary { get; set; }
    
    /// <summary>
    /// The leader of the game, if the there is no leader, the value would be null
    /// </summary>
    public GamePlayerScore Leader { get; set; }
    
    public GamePlayerScore PlayerScore1 { get; set; }
    public GamePlayerScore PlayerScore2 { get; set; }
  }
}
