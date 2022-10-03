using System;
using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFader : MonoBehaviour
{
    [SerializeField] private Tilemap floorMap;
    private EnergyManager energy;

    private List<Tile> redTiles = new();
    private List<Tile> greenTiles = new();
    private List<Tile> blueTiles = new();

    [SerializeField] private Tile redTile;
    [SerializeField] private Tile greenTile;
    [SerializeField] private Tile blueTile;

    private bool initialized = false;
    
    void Start()
    {
        energy = FindObjectOfType<EnergyManager>();
        energy.CurrentState.onValueUpdated += () => RefreshTiles();
        ResetColors();
        
        floorMap.RefreshAllTiles();
        
        RefreshTiles(true);
        initialized = true;
    }

    Tile GetTile(EnergyManager.State state)
    {
        switch (state)
        {
            case EnergyManager.State.Red:
                return redTile;
            case EnergyManager.State.Green:
                return greenTile;
            case EnergyManager.State.Blue:
                return blueTile;
            default:
                return null;
        }
    }

    private void RefreshTiles(bool force = false)
    {
        var target = GetTile(energy.CurrentState);
        Fade(target, false, force);
        
        if (!initialized) return;
        
        var oldTarget = GetTile(energy.LastState);
        Fade(oldTarget, true, force);
        
        oldTarget = GetTile(energy.NextState);
        Fade(oldTarget, true, force);
    }

    private void Fade(Tile tile, bool state, bool force = false)
    {
        var color = tile.color;
        color.a = state ? 1f : 0.2f;
        if (force)
        {
            tile.color = color;
            floorMap.RefreshAllTiles();
            return;
        }
        
        var tween = new Tween<Color>(c =>
            {
                tile.color = c;
                floorMap.RefreshAllTiles();
            },
            tile.color, color, PlayerManager.MOVE_DELAY, Color.Lerp);
        tween.Play();
    }

    private void OnApplicationQuit()
    {
        ResetColors();
    }

    private void ResetColors()
    {
        var c = redTile.color;
        c.a = 1f;
        redTile.color = c;
        
        c = greenTile.color;
        c.a = 1f;
        greenTile.color = c;
        
        c = blueTile.color;
        c.a = 1f;
        blueTile.color = c;
    }
}
