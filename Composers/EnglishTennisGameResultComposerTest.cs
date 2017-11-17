using System.Collections;
using NUnit.Framework;
using Tennis.Models;

namespace Tennis.Composers
{
  [TestFixture]
  public class EnglishTennisGameResultComposerTest
  {
    private EnglishTennisGameResultComposer m_composer;

    [TestFixtureSetUp]
    public void TestFixtureSetUp()
    {
      m_composer = new EnglishTennisGameResultComposer();
    }
    
    GameSummaryResult GetResultModel(GameProgressSummary progressSummary, int score1, int score2,
      int? leaderIndex = null)
    {
      GamePlayerScore playerScore1 = new GamePlayerScore
      {
        PlayerName = "player1",
        Score = score1
      };
      
      GamePlayerScore playerScore2 = new GamePlayerScore
      {
        PlayerName = "player2",
        Score = score2
      };

      GamePlayerScore leader = null;

      if (leaderIndex == 0)
      {
        leader = playerScore1;
      }
      else if (leaderIndex == 1)
      {
        leader = playerScore2;
      }

      return new GameSummaryResult
      {
        ProgressSummary = progressSummary,
        Leader = leader,
        PlayerScore1 = playerScore1,
        PlayerScore2 = playerScore2
      };
    }

    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.TieTestCases))]
    public string TieTest(int score1, int score2)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.Tie, score1, score2));
    }
    
    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.DeuceTestCases))]
    public string DeuceTest(int score1, int score2)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.Deuce, score1, score2));
    }
    
    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.AdvantageTestCases))]
    public string AdvantageTest(int score1, int score2, int leaderIndex)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.Advantage, score1, score2, leaderIndex));
    }
    
    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.LeadTestCases))]
    public string LeadTest(int score1, int score2, int leaderIndex)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.Lead, score1, score2, leaderIndex));
    }
    
    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.WinTestCases))]
    public string WinTest(int score1, int score2, int leaderIndex)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.Win, score1, score2, leaderIndex));
    }
    
    [Test,
     TestCaseSource(typeof(EnglishTennisGameResultComposerTestCases),
       nameof(EnglishTennisGameResultComposerTestCases.NotSupportedScenarioTestCases))]
    public string NotSupportedScenarioTest(int score1, int score2, int leaderIndex)
    {
      return m_composer.GetResultText(GetResultModel(GameProgressSummary.NotSupported, score1, score2, leaderIndex));
    }
  }

  class EnglishTennisGameResultComposerTestCases
  {
    public static IEnumerable TieTestCases
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns("Love-All");
        yield return new TestCaseData(1, 1).Returns("Fifteen-All");
        yield return new TestCaseData(2, 2).Returns("Thirty-All");
        yield return new TestCaseData(3, 3).Returns("Forty-All");
      }
    }
    
    public static IEnumerable DeuceTestCases
    {
      get
      {
        yield return new TestCaseData(4, 4).Returns("Deuce");
        yield return new TestCaseData(5, 5).Returns("Deuce");
        yield return new TestCaseData(6, 6).Returns("Deuce");
      }
    }
    
    public static IEnumerable AdvantageTestCases
    {
      get
      {
        yield return new TestCaseData(5, 4, 0).Returns("Advantage player1");
        yield return new TestCaseData(4, 5, 1).Returns("Advantage player2");
        yield return new TestCaseData(107, 106, 0).Returns("Advantage player1");
      }
    }
    
    public static IEnumerable LeadTestCases
    {
      get
      {
        yield return new TestCaseData(1, 0, 0).Returns("Fifteen-Love");
        yield return new TestCaseData(2, 0, 0).Returns("Thirty-Love");
        yield return new TestCaseData(3, 0, 0).Returns("Forty-Love");
        yield return new TestCaseData(2, 1, 0).Returns("Thirty-Fifteen");
        yield return new TestCaseData(3, 1, 0).Returns("Forty-Fifteen");
        yield return new TestCaseData(3, 2, 0).Returns("Forty-Thirty");
        
        yield return new TestCaseData(0, 1, 1).Returns("Love-Fifteen");
        yield return new TestCaseData(0, 2, 1).Returns("Love-Thirty");
        yield return new TestCaseData(0, 3, 1).Returns("Love-Forty");
        yield return new TestCaseData(1, 2, 1).Returns("Fifteen-Thirty");
        yield return new TestCaseData(1, 3, 1).Returns("Fifteen-Forty");
        yield return new TestCaseData(2, 3, 1).Returns("Thirty-Forty");
      }
    }
    
    public static IEnumerable WinTestCases
    {
      get
      {
        yield return new TestCaseData(4, 0, 0).Returns("Win for player1");
        yield return new TestCaseData(4, 1, 0).Returns("Win for player1");
        yield return new TestCaseData(4, 2, 0).Returns("Win for player1");
        yield return new TestCaseData(0, 4, 1).Returns("Win for player2");
        yield return new TestCaseData(2, 4, 1).Returns("Win for player2");
        yield return new TestCaseData(5, 7, 1).Returns("Win for player2");
      }
    }
    
    public static IEnumerable NotSupportedScenarioTestCases
    {
      get
      {
        yield return new TestCaseData(5, 0, 0).Returns("Sorry we have technical difficulties with the scores");
        yield return new TestCaseData(6, 12, 1).Returns("Sorry we have technical difficulties with the scores");
      }
    }
  }
}
