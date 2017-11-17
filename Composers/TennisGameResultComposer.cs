﻿using System.Collections.Generic;
using SmartFormat;
using Tennis.Models;

namespace Tennis.Composers
{
  /// <summary>
  /// Game Result Composer takes the game result literally, and compose a human understandable
  /// description of the game result.
  /// </summary>
  public abstract class TennisGameResultComposer : IGameResultComposer
  {
    protected readonly Dictionary<int, string> nonDeuceScoreText;

    public TennisGameResultComposer()
    {
      nonDeuceScoreText = new Dictionary<int, string>();
      this.AdvantageResultTextTemplate = "";
      this.DeuceResultTextTemplate = "";
      this.LeadResultTextTemplate = "";
      this.NotSupportedResultTextTemplate = "";
      this.TieResultTextTemplate = "";
      this.WinResultTextTemplate = "";
    }

    public string GetResultText(GameSummaryResult result)
    {
      TennisGameResultViewModel vm = new TennisGameResultViewModel(this.nonDeuceScoreText, result);

      string textFormat = "";
      switch (result.ProgressSummary)
      {
        case GameProgressSummary.Advantage:
          textFormat = AdvantageResultTextTemplate;
          break;
        case GameProgressSummary.Deuce:
          textFormat = DeuceResultTextTemplate;
          break;
        case GameProgressSummary.Lead:
          textFormat = LeadResultTextTemplate;
          break;
        case GameProgressSummary.NotSupported:
          textFormat = NotSupportedResultTextTemplate;
          break;
        case GameProgressSummary.Tie:
          textFormat = TieResultTextTemplate;
          break;
        case GameProgressSummary.Win:
          textFormat = WinResultTextTemplate;
          break;
      }

      return Smart.Format(textFormat, vm);
    }

    public abstract string AdvantageResultTextTemplate { get; set; }
    public abstract string DeuceResultTextTemplate { get; set; }
    public abstract string LeadResultTextTemplate { get; set; }
    public abstract string NotSupportedResultTextTemplate { get; set; }
    public abstract string TieResultTextTemplate { get; set; }
    public abstract string WinResultTextTemplate { get; set; }
  }
  
  public class TennisGameResultViewModel
  {
    private readonly Dictionary<int, string> m_nonDeuceScoreText;
    public TennisGameResultViewModel(Dictionary<int, string> nonDeuceScoreText, GameSummaryResult result)
    {
      this.Result = result;
      this.m_nonDeuceScoreText = nonDeuceScoreText;
    }
    
    public GameSummaryResult Result { get; set; }

    public string GetScoreText(int score)
    {
      if (m_nonDeuceScoreText.ContainsKey(score))
      {
        return m_nonDeuceScoreText[score];
      }

      return score.ToString();
    }

    public string LeaderName
    {
      get { return Result.Leader?.PlayerName; }
    }
    
    public string Player1Score
    {
      get { return (Result.PlayerScore1 == null) ? null : GetScoreText(Result.PlayerScore1.Score); }
    }
    
    public string Player2Score
    {
      get { return (Result.PlayerScore2 == null) ? null : GetScoreText(Result.PlayerScore2.Score); }
    }
    
    public string Player1Name
    {
      get { return Result.PlayerScore1?.PlayerName; }
    }
    
    public string Player2Name
    {
      get { return Result.PlayerScore2?.PlayerName; }
    }
  }
}