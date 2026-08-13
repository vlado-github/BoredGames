using Assets.Scripts.BoredGames.API;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameOver
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _winnerText;

        void Start()
        {
            StartCoroutine(BoredGamesAPIClient.Instance.GetWinners((response) => {
                if (response.winners != null && response.winners.Any()) 
                {
                    var winners = string.Join(",", response.winners.Select(x => x.nickName));
                    _winnerText.text = $"Winner: {winners}";
                }
            }));
            
        }
    }
}