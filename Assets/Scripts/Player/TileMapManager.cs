using System;
using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private EnergyManager energy;
    [SerializeField] private KeyManager keys;
    
    [Header("Tilemaps")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap itemMap;
    
    [Header("Colored Tiles")] 
    [SerializeField] private Tile redTile;
    [SerializeField] private Tile greenTile;
    [SerializeField] private Tile blueTile;
    
    private NextMoveFailsIndicator failsIndicator;
    private Vector3 currentPosition;

    void Start()
    {
        failsIndicator = FindObjectOfType<NextMoveFailsIndicator>();
        currentPosition = player.transform.position;
    }

    public bool MovePlayer(Vector2 input, ref Action undoAction, Action onComplete = null)
    {
        var playerPosition = (Vector2) player.transform.position;
        var nextPosition = playerPosition + input;
        
        var intNextPosition = Vector3Int.FloorToInt(nextPosition);
        
        var nextTile = floorMap.GetTile(intNextPosition);

        if (nextTile == null || nextTile.name == "Black") return false;
        if (nextTile.name.Contains(energy.StateAfterMove.ToString()))
        {
            failsIndicator.Show();
            return false;
        }

        var item = itemMap.GetTile(intNextPosition);
        
        if(!CheckItem(item, intNextPosition, ref undoAction)) return false;

        var intCurrentPos = Vector3Int.FloorToInt(currentPosition);
        
        RecordUndo(floorMap, intCurrentPos, ref undoAction);
        
        floorMap.SetTile(intCurrentPos, GetTileColor());

        currentPosition = nextPosition;
        
        player.transform.TweenPosition(nextPosition, PlayerManager.MOVE_DELAY, onComplete);

        if (nextTile.name.Contains("Goal"))
        {
            player.LevelComplete();
        }

        return true;
    }

    private Tile GetTileColor()
    {
        switch (energy.CurrentState.value)
        {
            case EnergyManager.State.Red:
                return redTile;
            case EnergyManager.State.Green:
                return greenTile;
            case EnergyManager.State.Blue:
                return blueTile;
        }

        return null;
    }

    private bool CheckItem(TileBase item, Vector3Int position, ref Action undoAction)
    {
        if (item == null) return true;
        
        if (item.name.Contains("Key") && keys != null)
        {
            var lockPosition = keys.GetLockPosition(position);
            if (lockPosition != null) Unlock(position, lockPosition.Value, ref undoAction);
            return true;
        }
         
        return false;
    }

    private void Unlock(Vector3Int position,  Vector3Int lockPosition, ref Action undoAction)
    {
        RecordUndo(itemMap, position, ref undoAction);
        RecordUndo(itemMap, lockPosition, ref undoAction);
        
        itemMap.SetTile(position, null);
        itemMap.SetTile(lockPosition, null);
    }

    private void RecordUndo(Tilemap tilemap, Vector3Int position, ref Action undoAction)
    {
        var currentTile = tilemap.GetTile(position);
        undoAction += () => tilemap.SetTile(position, currentTile);
    }

    public void MovePlayerToPosition(Vector2 position, Action onComplete = null)
    {
        currentPosition = position;
        player.transform.TweenPosition(position, PlayerManager.MOVE_DELAY, onComplete);
    }
}
