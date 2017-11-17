using System;

namespace Tennis
{
  public class TennisGame
  {
    private int m_score1 = 0;
    private int m_score2 = 0;
    private string m_player1Name;
    private string m_player2Name;

    public TennisGame(string mPlayer1Name, string mPlayer2Name)
    {
      this.m_player1Name = mPlayer1Name;
      this.m_player2Name = mPlayer2Name;
    }

    public void WonPoint(string playerName)
    {
      if (playerName == m_player1Name)
      {
        m_score1 += 1;
      }
      else if (playerName == m_player2Name)
      {
        m_score2 += 1;
      }
    }

    public string GetScore()
    {
      String score = "";
      int tempScore = 0;
      if (m_score1 == m_score2)
      {
        switch (m_score1)
        {
          case 0:
            score = "Love-All";
            break;
          case 1:
            score = "Fifteen-All";
            break;
          case 2:
            score = "Thirty-All";
            break;
        }
      }
      else if (m_score1 >= 4 || m_score2 >= 4)
      {
        int minusResult = m_score1 - m_score2;
        if (minusResult >= 2) score = "Win for player1";
        else score = "Win for player2";
      }
      else
      {
        for (int i = 1; i < 3; i++)
        {
          if (i == 1) tempScore = m_score1;
          else
          {
            score += "-";
            tempScore = m_score2;
          }
          switch (tempScore)
          {
            case 0:
              score += "Love";
              break;
            case 1:
              score += "Fifteen";
              break;
            case 2:
              score += "Thirty";
              break;
            case 3:
              score += "Forty";
              break;
          }
        }
      }
      return score;
    }
  }
}
