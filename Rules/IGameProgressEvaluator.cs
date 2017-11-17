using Tennis.Models;

namespace Tennis.Rules
{
  public interface IGameProgressEvaluator
  {
    GameSummaryResult GetProgress(GamePlayerScore score1, GamePlayerScore score2);
  }
}
