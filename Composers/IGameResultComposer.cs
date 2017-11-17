using Tennis.Models;

namespace Tennis.Composers
{
  public interface IGameResultComposer
  {
    string GetResultText(GameResult result);
  }
}
