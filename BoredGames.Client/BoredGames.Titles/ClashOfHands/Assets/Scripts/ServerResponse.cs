using Assets.Scripts.BoredGames.API;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ServerResponse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var apiClient = BoredGamesClient.GetInstance();
        StartCoroutine(apiClient.GetTitles((response) => {
            var scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
            scoreText.text = response.titles.First().name;
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
