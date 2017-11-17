using Tennis.Models;

namespace Tennis.Rules
{
  public interface IGameProgressEvaluator
  {
    GameResult GetProgress(GamePlayerScore score1, GamePlayerScore score2);
  }
}
