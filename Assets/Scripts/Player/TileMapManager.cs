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
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap itemMap;
    
    [Header("Colored Tiles")] 
    [SerializeField] private Tile redTile;
    [SerializeField] private Tile greenTile;
    [SerializeField] private Tile blueTile;
    
    private Vector3 currentPosition;

    public bool MovePlayer(Vector2 input, ref Action undoAction)
    {
        var playerPosition = (Vector2) player.transform.position;
        var nextPosition = playerPosition + input;
        
        var intNextPosition = Vector3Int.FloorToInt(nextPosition);
        
        var nextTile = floorMap.GetTile(intNextPosition);

        if (nextTile == null || nextTile.name == "Black" || nextTile.name.Contains(energy.StateAfterMove.ToString())) return false;

        var item = itemMap.GetTile(intNextPosition);

        if(!CheckItem(item, intNextPosition, ref undoAction)) return false;

        var intCurrentPos = Vector3Int.FloorToInt(currentPosition);
        
        RecordUndo(floorMap, intCurrentPos, ref undoAction);
        
        floorMap.SetTile(intCurrentPos, GetTileColor());

        currentPosition = nextPosition;
        
        player.transform.TweenPosition(nextPosition, PlayerManager.MOVE_DELAY);

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
        
        if (item.name.Contains("Lock"))
        {
            return CheckLockColor(item, position, ref undoAction);
        }
         
        return false;
    }

    private bool CheckLockColor(TileBase item, Vector3Int position, ref Action undoAction)
    {
        var unlocks = item.name.Contains(energy.CurrentState.ToString());
        if (unlocks)
        {
            RecordUndo(itemMap, position, ref undoAction);
            itemMap.SetTile(position, null);
        }
        return unlocks;
    }

    private void RecordUndo(Tilemap tilemap, Vector3Int position, ref Action undoAction)
    {
        var currentTile = tilemap.GetTile(position);
        undoAction += () => tilemap.SetTile(position, currentTile);
    }

    public void MovePlayerToPosition(Vector2 position)
    {
        currentPosition = position;
        player.transform.TweenPosition(position, PlayerManager.MOVE_DELAY);
    }
    
    public EnergyManager.State GetFloorState()
    {
        var tile = floorMap.GetTile(Vector3Int.FloorToInt(currentPosition));
        
        if (tile.name.Contains("Red")) return EnergyManager.State.Red;
        if (tile.name.Contains("Green")) return EnergyManager.State.Green;
        if (tile.name.Contains("Blue")) return EnergyManager.State.Blue;
        
        return EnergyManager.State.White;
    }
}
