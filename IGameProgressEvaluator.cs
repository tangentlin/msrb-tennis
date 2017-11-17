using Tennis.Models;

namespace Tennis
{
  public interface IGameProgressEvaluator
  {
    GameSummaryResult GetProgress(GamePlayerScore score1, GamePlayerScore score2);
  }
}
