using Assets.Scripts.GamePlay;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _waitingMessage;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] string _tagToHide;

    private void Awake()
    {
        CheckGameStatus();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _waitingMessage.gameObject.SetActive(true);

        _score.gameObject.SetActive(false);
        GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_tagToHide);
        foreach (GameObject objectToHide in objectsToHideAtStart)
        {
            objectToHide.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckGameStatus()
    {
        if (GameState.Instance.Status == Assets.Scripts.GameStatus.WaitingForPlayer)
        {
            _score.gameObject.SetActive(false);
            GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_tagToHide);
            foreach (GameObject objectToHide in objectsToHideAtStart)
            {
                objectToHide.SetActive(false);
            }
        }
    }

}
