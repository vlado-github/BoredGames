using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class Move
    {
        public string PlayerId;
        public string PlayerNickName;
        public string ActionType;
        public TilePosition SelectedTile;
    }

    [System.Serializable]
    public class TilePosition 
    {
        public int Row;
        public int Column;
    }
}