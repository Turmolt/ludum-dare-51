using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveHistory;

public class PlayerManager : MonoBehaviour
{
    public const float MOVE_DELAY = 0.15f;
    public Observable<int> NumberMoves = new(0);
    public Observable<int> NumberUndos = new(0);

    [SerializeField] private ClockManager clock;
    [SerializeField] private EnergyManager energy;
    [SerializeField] private TileMapManager map;
    [SerializeField] private LifeManager life;
    [SerializeField] private UIManager ui;


    private MoveHistory _moveHistory = new();

    private float _timeSinceLastMove = MOVE_DELAY;

    private bool canMove = true;
    private bool moving = false;

    void Start()
    {
        clock.OnClockReset += energy.ChangeColor;
        life.Health.onValueUpdated += CheckHealth;
    }

    private void CheckHealth()
    {
        if(life.Health.value <= 0) Debug.Log("DEATH");
    }

    void Update()
    {
        if (!canMove) return;
        if (_timeSinceLastMove < MOVE_DELAY)
        {
            _timeSinceLastMove += Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UndoMove();
            return;
        }
        
        if (moving) return;
        
        var input = GetInput();
        
        if (input.magnitude == 0) return;


        Action undoAction = null;
        
        var lastMove = CreateLastMove();

        if (!map.MovePlayer(input, ref undoAction, () => moving = false)) return;
        
        moving = true;
        
        lastMove.UndoAction = undoAction;

        _moveHistory.Push(lastMove);

        _timeSinceLastMove = 0f;
        
        clock.MoveClock();
        
        NumberMoves.value++;
    }

    private Move CreateLastMove()
    {
        return new Move()
        {
            Health = life.Health.value,
            MoveNumber = NumberMoves.value,
            Position = transform.position,
            Seconds = clock.CurrentSeconds,
            State = energy.CurrentState
        };
    }

    public void UndoMove()
    {
        if (moving || !canMove) return;
        var previous = _moveHistory.Pop();
        
        if (previous == null) return;
        NumberUndos.value++;
        moving = true;
        energy.Set(previous.State);
        map.MovePlayerToPosition(previous.Position, () => moving = false);
        NumberMoves.value = previous.MoveNumber;
        clock.Set(previous.Seconds);
        previous.UndoAction?.Invoke();
    }


    Vector2 GetInput()
    {
        var left = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        var right = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        var up = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        var down = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

        // only move vertically or horizontally, not diagonally
        up = up && !left && !right;
        down = down && !left && !right;
        
        var input = new Vector2();

        if (right) input.x += 1;
        if (left) input.x -= 1;
        if (up) input.y += 1;
        if (down) input.y -= 1;
        
        return input;
    }

    public void LevelComplete()
    {
        canMove = false;
        ui.ShowWinScreen();
    }
}
