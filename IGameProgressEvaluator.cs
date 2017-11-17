using Tennis.Models;

namespace Tennis
{
  public interface IGameProgressEvaluator
  {
    GameSummaryResult GetProgress(int score1, int score2);
  }
}
