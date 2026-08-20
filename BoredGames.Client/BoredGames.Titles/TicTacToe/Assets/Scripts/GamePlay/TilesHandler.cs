using System;
using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class TilesHandler : MonoBehaviour
{
    [SerializeField] private Image[] tiles;
    [SerializeField] private Sprite exSprite;
    [SerializeField] private Sprite oxSprite;
    private Sprite emptySprite;
    
    public static TilesHandler Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void Rerender()
    {
        if (tiles == null)
        {
            return;
        }
        foreach (var move in GameState.Instance.Moves)
        {
            var index = ToIndex(move.SelectedTile.Row, move.SelectedTile.Column);
            if (move.ActionType.Equals("x", StringComparison.OrdinalIgnoreCase))
            {
                tiles[index].sprite = exSprite;
            }
            else
            {
                tiles[index].sprite = oxSprite;
            }
        }
    }

    public void Reset()
    {
        try
        {
            if (tiles == null)
            {
                return;
            }

            foreach (var tile in tiles)
            {
                tile.sprite = emptySprite;
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("No tiles found");
        }
    }

    private int ToIndex(int row, int column)
    {
        return 2 * row + row + column;
    }
}

