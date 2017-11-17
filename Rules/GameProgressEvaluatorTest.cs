using System.Collections;
using NUnit.Framework;
using Tennis.Models;

namespace Tennis.Rules
{
  [TestFixture(0, 0, GameProgressSummary.Tie, null)]
  [TestFixture( 1, 1, GameProgressSummary.Tie, null)]
  [TestFixture( 2, 2, GameProgressSummary.Tie, null)]
  [TestFixture( 3, 3, GameProgressSummary.Deuce, null)]
  [TestFixture( 4, 4, GameProgressSummary.Deuce, null)]
  [TestFixture( 1, 0, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 0, 1, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 2, 0, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 0, 2, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 3, 0, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 0, 3, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 4, 0, GameProgressSummary.Win, PLAYER_1)]
  [TestFixture( 0, 4, GameProgressSummary.Win, PLAYER_2)]
  [TestFixture( 2, 1, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 1, 2, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 3, 1, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 1, 3, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 4, 1, GameProgressSummary.Win, PLAYER_1)]
  [TestFixture( 1, 4, GameProgressSummary.Win, PLAYER_2)]
  [TestFixture( 3, 2, GameProgressSummary.Lead, PLAYER_1)]
  [TestFixture( 2, 3, GameProgressSummary.Lead, PLAYER_2)]
  [TestFixture( 4, 2, GameProgressSummary.Win, PLAYER_1)]
  [TestFixture( 2, 4, GameProgressSummary.Win, PLAYER_2)]
  [TestFixture( 4, 3, GameProgressSummary.Advantage, PLAYER_1)]
  [TestFixture( 3, 4, GameProgressSummary.Advantage, PLAYER_2)]
  [TestFixture( 5, 4, GameProgressSummary.Advantage, PLAYER_1)]
  [TestFixture( 4, 5, GameProgressSummary.Advantage, PLAYER_2)]
  [TestFixture( 15, 14, GameProgressSummary.Advantage, PLAYER_1)]
  [TestFixture( 14, 15, GameProgressSummary.Advantage, PLAYER_2)]
  [TestFixture( 6, 4, GameProgressSummary.Win, PLAYER_1)]
  [TestFixture( 4, 6, GameProgressSummary.Win, PLAYER_2)]
  [TestFixture( 16, 14, GameProgressSummary.Win, PLAYER_1)]
  [TestFixture( 14, 16, GameProgressSummary.Win, PLAYER_2)]
  [TestFixture( 14, 17, GameProgressSummary.NotSupported, null)]
  [TestFixture( 5, 0, GameProgressSummary.NotSupported, null)]
  [TestFixture( 5, 1, GameProgressSummary.NotSupported, null)]
  [TestFixture( 5, 2, GameProgressSummary.NotSupported, null)]
  public class GameProgressEvaluatorTest
  {
    private GamePlayerScore m_score1;
    private GamePlayerScore m_score2;
    private GameProgressSummary m_expectedSummary;
    private string m_expectedLeaderName;

    public const string PLAYER_1 = "p1";
    public const string PLAYER_2 = "p2";
    
    public GameProgressEvaluatorTest(int score1, int score2, GameProgressSummary expectedSummary,
      string expectedLeaderName)
    {
      m_score1 = new GamePlayerScore
      {
        PlayerName = PLAYER_1,
        Score = score1
      };

      m_score2 = new GamePlayerScore
      {
        PlayerName = PLAYER_2,
        Score = score2
      };

      m_expectedSummary = expectedSummary;
      m_expectedLeaderName = expectedLeaderName;
    }

    [Test]
    public void CheckEvaluation()
    {
      TennisGameProgressEvaluator evaluator = new TennisGameProgressEvaluator();

      GameSummaryResult result = evaluator.GetProgress(m_score1, m_score2);

      Assert.AreEqual(m_expectedSummary, result.ProgressSummary);

      string leaderName = (result.Leader == null) ? null : result.Leader.PlayerName;
      Assert.AreEqual(m_expectedLeaderName, leaderName);
    }
  }

  #region Individual condition tests
  
  [TestFixture]
  public class GameProgressEvaluatorIndividualFeatureTest
  {
    private TennisGameProgressEvaluator m_evaluator;

    [SetUp]
    public void SetUp()
    {
      m_evaluator = new TennisGameProgressEvaluator();
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.TieTestCases))]
    public bool IsTieTest(int score1, int score2)
    {
      return m_evaluator.IsTie(WrapScoreAsLeadAndTrail(score1, score2));
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.DeuceTestCases))]
    public bool IsDeuceTest(int score1, int score2)
    {
      return m_evaluator.IsDeuce(WrapScoreAsLeadAndTrail(score1, score2));
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.AdvantageTestCases))]
    public bool IsAdvantageTest(int score1, int score2)
    {
      return m_evaluator.IsAdvantage(WrapScoreAsLeadAndTrail(score1, score2));
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.HasWinnerTestCases))]
    public bool HasWinnerTest(int score1, int score2)
    {
      return m_evaluator.HasWinner(WrapScoreAsLeadAndTrail(score1, score2));
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.HasNegativeScore))]
    public bool HasNegativeScoreTest(int score1, int score2)
    {
      var leadAndTrail = WrapScoreAsLeadAndTrail(score1, score2);
      return m_evaluator.HasNegativeScore(leadAndTrail.Leader, leadAndTrail.Trailer);
    }
    
    [Test, TestCaseSource(typeof(IndividualFeatureTestCases), nameof(IndividualFeatureTestCases.HasImpossibleScoreTestCases))]
    public bool HasImpossibleScoreTest(int score1, int score2)
    {
      return m_evaluator.HasImpossibleScore(WrapScoreAsLeadAndTrail(score1, score2));
    }

    LeadAndTrail<GamePlayerScore> WrapScoreAsLeadAndTrail(int score1, int score2)
    {
      GamePlayerScore s1 = new GamePlayerScore
      {
        PlayerName = "p1",
        Score = score1
      };

      GamePlayerScore s2 = new GamePlayerScore
      {
        PlayerName = "p2",
        Score = score2
      };

      return m_evaluator.GetLeadAndTrail(s1, s2);
    }
  }

  class IndividualFeatureTestCases
  {
    public static IEnumerable TieTestCases
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns(true);
        yield return new TestCaseData(1, 1).Returns(true);
        yield return new TestCaseData(2, 2).Returns(true);
        yield return new TestCaseData(3, 3).Returns(false);
        yield return new TestCaseData(103, 103).Returns(false);
        yield return new TestCaseData(1, 0).Returns(false);
        yield return new TestCaseData(0, 1).Returns(false);
      }
    }
    
    public static IEnumerable DeuceTestCases
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns(false);
        yield return new TestCaseData(1, 1).Returns(false);
        yield return new TestCaseData(2, 2).Returns(false);
        yield return new TestCaseData(3, 3).Returns(true);
        yield return new TestCaseData(103, 103).Returns(true);
        yield return new TestCaseData(104, 103).Returns(false);
        yield return new TestCaseData(105, 103).Returns(false);
        yield return new TestCaseData(1, 0).Returns(false);
        yield return new TestCaseData(0, 1).Returns(false);
      }
    }
    
    public static IEnumerable AdvantageTestCases
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns(false);
        yield return new TestCaseData(1, 1).Returns(false);
        yield return new TestCaseData(2, 2).Returns(false);
        yield return new TestCaseData(3, 3).Returns(false);
        yield return new TestCaseData(103, 103).Returns(false);
        yield return new TestCaseData(104, 103).Returns(true);
        yield return new TestCaseData(105, 103).Returns(false);
        yield return new TestCaseData(5, 4).Returns(true);
        yield return new TestCaseData(3, 4).Returns(true);
        yield return new TestCaseData(1, 0).Returns(false);
        yield return new TestCaseData(0, 1).Returns(false);
      }
    }
    
    public static IEnumerable HasWinnerTestCases
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns(false);
        yield return new TestCaseData(2, 0).Returns(false);
        yield return new TestCaseData(3, 0).Returns(false);
        yield return new TestCaseData(4, 0).Returns(true);
        yield return new TestCaseData(4, 1).Returns(true);
        yield return new TestCaseData(4, 2).Returns(true);
        yield return new TestCaseData(4, 3).Returns(false);
        yield return new TestCaseData(1, 1).Returns(false);
        yield return new TestCaseData(2, 2).Returns(false);
        yield return new TestCaseData(3, 3).Returns(false);
        yield return new TestCaseData(5, 3).Returns(true);
        yield return new TestCaseData(103, 103).Returns(false);
        yield return new TestCaseData(104, 103).Returns(false);
        yield return new TestCaseData(105, 103).Returns(true);
        yield return new TestCaseData(5, 4).Returns(false);
        yield return new TestCaseData(3, 4).Returns(false);
        yield return new TestCaseData(6, 4).Returns(true);
        yield return new TestCaseData(1, 0).Returns(false);
        yield return new TestCaseData(0, 1).Returns(false);
      }
    }
    
    public static IEnumerable HasNegativeScore
    {
      get
      {
        yield return new TestCaseData(0, 0).Returns(false);
        yield return new TestCaseData(2, 0).Returns(false);
        yield return new TestCaseData(3, 0).Returns(false);
        yield return new TestCaseData(4, 3).Returns(false);
        yield return new TestCaseData(1, 1).Returns(false);
        yield return new TestCaseData(2, 2).Returns(false);
        yield return new TestCaseData(3, 3).Returns(false);
        yield return new TestCaseData(-1, 3).Returns(true);
        yield return new TestCaseData(-1, -3).Returns(true);
        yield return new TestCaseData(0, -3).Returns(true);
      }
    }
    
    public static IEnumerable HasImpossibleScoreTestCases
    {
      get
      {
        yield return new TestCaseData(5, 0).Returns(true);
        yield return new TestCaseData(5, 1).Returns(true);
        yield return new TestCaseData(5, 2).Returns(true);
        yield return new TestCaseData(6, 3).Returns(true);
        yield return new TestCaseData(3, 6).Returns(true);
        yield return new TestCaseData(2, 5).Returns(true);
        yield return new TestCaseData(0, 0).Returns(false);
        yield return new TestCaseData(2, 0).Returns(false);
        yield return new TestCaseData(3, 0).Returns(false);
        yield return new TestCaseData(4, 0).Returns(false);
        yield return new TestCaseData(4, 1).Returns(false);
        yield return new TestCaseData(4, 2).Returns(false);
        yield return new TestCaseData(4, 3).Returns(false);
        yield return new TestCaseData(1, 1).Returns(false);
        yield return new TestCaseData(2, 2).Returns(false);
        yield return new TestCaseData(3, 3).Returns(false);
        yield return new TestCaseData(5, 3).Returns(false);
        yield return new TestCaseData(103, 103).Returns(false);
        yield return new TestCaseData(104, 103).Returns(false);
        yield return new TestCaseData(105, 103).Returns(false);
        yield return new TestCaseData(5, 4).Returns(false);
        yield return new TestCaseData(3, 4).Returns(false);
        yield return new TestCaseData(6, 4).Returns(false);
        yield return new TestCaseData(1, 0).Returns(false);
        yield return new TestCaseData(0, 1).Returns(false);
      }
    }
  }
  
  #endregion
}
