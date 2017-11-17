using Tennis.Models;

namespace Tennis.Composers
{
  public interface IGameResultComposer
  {
    string GetResultText(GameSummaryResult result);
  }
}
