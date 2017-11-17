namespace Tennis.Composers
{
  public class EnglishTennisGameResultComposer : TennisGameResultComposer
  {
    public EnglishTennisGameResultComposer()
    {
      this.nonDeuceScoreText.Add(0, "Love");
      this.nonDeuceScoreText.Add(1, "Fifteen");
      this.nonDeuceScoreText.Add(2, "Thirty");
      this.nonDeuceScoreText.Add(3, "Forty");

      this.AdvantageResultTextTemplate = "Advantage {LeaderName}";
      this.DeuceResultTextTemplate = "Deuce";
      this.LeadResultTextTemplate = "{Player1Score}-{Player2Score}";
      this.NotSupportedResultTextTemplate = "Sorry we have technical difficulties with the scores";
      this.TieResultTextTemplate = "{Player1Score}-All";
      this.WinResultTextTemplate = "Win for {LeaderName}";
    }
    
    public override string AdvantageResultTextTemplate { get; set; }
    public override string DeuceResultTextTemplate { get; set; }
    public override string LeadResultTextTemplate { get; set; }
    public override string NotSupportedResultTextTemplate { get; set; }
    public override string TieResultTextTemplate { get; set; }
    public override string WinResultTextTemplate { get; set; }
  }
}
