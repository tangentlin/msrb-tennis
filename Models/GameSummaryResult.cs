﻿namespace Tennis.Models
{
  public class GameSummaryResult
  {
    public GameProgressSummary ProgressSummary { get; set; }
    
    /// <summary>
    /// The leader of the game, if the there is no leader, the value would be null
    /// </summary>
    public GamePlayerScore Leader { get; set; }
  }
}