using Tennis.Composers;
using Tennis.Models;
using Tennis.Rules;

namespace Tennis
{
  public class TennisGame
  {
    private GamePlayerScore m_playerScore1;
    private GamePlayerScore m_playerScore2;
    
    public IGameProgressEvaluator ProgressEvaluator = new TennisGameProgressEvaluator();
    public IGameResultComposer GameResultComposer = new EnglishTennisGameResultComposer();

    public TennisGame(string mPlayer1Name, string mPlayer2Name)
    {
      m_playerScore1 = new GamePlayerScore
      {
        PlayerName = mPlayer1Name,
        Score = 0
      };
      
      m_playerScore2 = new GamePlayerScore
      {
        PlayerName = mPlayer2Name,
        Score = 0
      };
    }

    public void WonPoint(string playerName)
    {
      if (playerName == m_playerScore1.PlayerName)
      {
        m_playerScore1.Score ++;
      }
      else if (playerName == m_playerScore2.PlayerName)
      {
        m_playerScore2.Score ++;
      }
    }

    public string GetScore()
    {
      GameResult result = ProgressEvaluator.GetProgress(m_playerScore1, m_playerScore2);
      return GameResultComposer.GetResultText(result);
    }
  }
}
