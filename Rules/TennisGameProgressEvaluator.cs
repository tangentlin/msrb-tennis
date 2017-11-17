using Tennis.Models;

namespace Tennis.Rules
{
  public class TennisGameProgressEvaluator : IGameProgressEvaluator
  {
    private int m_nonDeuceWinningScore = 4;
    private int m_minimumDeuceScore = 3;
    private int m_dueceToWinDifference = 2;
    
    public GameResult GetProgress(GamePlayerScore score1, GamePlayerScore score2)
    {
      GameResult result = new GameResult();
      result.PlayerScore1 = score1;
      result.PlayerScore2 = score2;
      
      // First, take care of the bad scoring situation
      // This is unlikely through regular score keep
      // but this class does not assume good score keeping
      if (HasNegativeScore(score1, score2))
      {
        result.ProgressSummary = GameProgressSummary.NotSupported;
        return result;
      }

      LeadAndTrail<GamePlayerScore> leadAndTrail = GetLeadAndTrail(score1, score2);

      if (HasImpossibleScore(leadAndTrail))
      {
        result.ProgressSummary = GameProgressSummary.NotSupported;
        return result;
      }
      
      // Second, take care of the tie/deuce situation
      if (IsTie(leadAndTrail))
      {
        result.ProgressSummary = GameProgressSummary.Tie;
        return result;
      }

      if (IsDeuce(leadAndTrail))
      {
        result.ProgressSummary = GameProgressSummary.Deuce;
        return result;
      }

      // Third, we now have a leader
      // Let's figure out the leader's position
      result.Leader = leadAndTrail.Leader;
      if (IsAdvantage(leadAndTrail))
      {
        result.ProgressSummary = GameProgressSummary.Advantage;
        return result;
      }

      if (HasWinner(leadAndTrail))
      {
        result.ProgressSummary = GameProgressSummary.Win;
        return result;
      }

      result.ProgressSummary = GameProgressSummary.Lead;
      return result;
    }

    public LeadAndTrail<GamePlayerScore> GetLeadAndTrail(GamePlayerScore score1, GamePlayerScore score2)
    {
      LeadAndTrail<GamePlayerScore> result = new LeadAndTrail<GamePlayerScore>();

      if (score1.Score > score2.Score)
      {
        result.Leader = score1;
        result.Trailer = score2;
      }
      else
      {
        result.Leader = score2;
        result.Trailer = score1;
      }

      result.IsTie = (score1.Score == score2.Score);
      return result;
    }

    public bool HasNegativeScore(GamePlayerScore score1, GamePlayerScore score2)
    {
      return (score1.Score < 0 || score2.Score < 0);
    }
    
    public bool HasImpossibleScore(LeadAndTrail<GamePlayerScore> leadAndTrail)
    {
      int leadScore = leadAndTrail.Leader.Score;
      int trailScore = leadAndTrail.Trailer.Score;

      return leadScore > m_nonDeuceWinningScore && (leadScore - trailScore) > m_dueceToWinDifference;
    }

    public bool IsDeuce(LeadAndTrail<GamePlayerScore> leadAndTrail)
    {
      return leadAndTrail.IsTie && leadAndTrail.Leader.Score >= m_minimumDeuceScore;
    }

    public bool IsTie(LeadAndTrail<GamePlayerScore> leadAndTrail)
    {
      return leadAndTrail.IsTie && leadAndTrail.Leader.Score < m_minimumDeuceScore;
    }

    public bool IsAdvantage(LeadAndTrail<GamePlayerScore> leadAndTrail)
    {
      if (IsDeuce(leadAndTrail) || IsTie(leadAndTrail))
      {
        return false;
      }

      return (leadAndTrail.Leader.Score - leadAndTrail.Trailer.Score < m_dueceToWinDifference &&
              leadAndTrail.Trailer.Score >= m_minimumDeuceScore);
    }

    public bool HasWinner(LeadAndTrail<GamePlayerScore> leadAndTrail)
    {
      if (IsDeuce(leadAndTrail) || IsTie(leadAndTrail) || IsAdvantage(leadAndTrail))
      {
        return false;
      }
      
      if (leadAndTrail.Leader.Score >= m_nonDeuceWinningScore && leadAndTrail.Trailer.Score < m_minimumDeuceScore)
      {
        return true;
      }

      if (leadAndTrail.Trailer.Score >= m_minimumDeuceScore &&
          leadAndTrail.Leader.Score - leadAndTrail.Trailer.Score == m_dueceToWinDifference)
      {
        return true;
      }

      return false;
    }
  }
}
