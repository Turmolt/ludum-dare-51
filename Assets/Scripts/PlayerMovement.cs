using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private Tilemap map;
    
    public bool Move(Vector2 input)
    {
        var playerPosition = (Vector2) player.transform.position;
        var nextPosition = playerPosition + input;
        var tile = map.GetTile(Vector3Int.FloorToInt(nextPosition));

        if (tile.name == "Black") return false;
        player.transform.TweenPosition(nextPosition, PlayerManager.MOVE_DELAY);
        
        return true;
    }
}
