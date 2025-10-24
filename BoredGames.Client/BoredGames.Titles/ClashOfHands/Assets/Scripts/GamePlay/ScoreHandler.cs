using UnityEngine;
using TMPro;
using Assets.Scripts.GamePlay;
using System.Linq;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _roundText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Instance.Score != null)
        {
            if (GameState.Instance.Score.PlayerScores != null && GameState.Instance.Score.PlayerScores.Any())
            {
                var playerScore = GameState.Instance.Score.PlayerScores.First(x => x.PlayerId == GameState.Instance.PlayerId);
                var opponentScore = GameState.Instance.Score.PlayerScores.First(x => x.PlayerId != GameState.Instance.PlayerId);

                var displayPlayerScore = $"{playerScore.PlayerNickName} {playerScore.RoundWins.Count()}";
                var displayOpponentScore = $"{opponentScore.RoundWins.Count()} {opponentScore.PlayerNickName}";

                _scoreText.text = $"{displayPlayerScore} : {displayOpponentScore}";
                _roundText.text = $"Round: {GameState.Instance.Score.CurrentRound}";
            }
        }
    }
}
