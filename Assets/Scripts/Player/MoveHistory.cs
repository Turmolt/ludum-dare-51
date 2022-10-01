using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHistory
{
    public class Move
    {
        public int Health { get; set; }
        public Vector2 Position { get; set; }
        public int Seconds { get; set; }
        public EnergyManager.State State { get; set; }
        public int MoveNumber { get; set; }
        public Action UndoAction { get; set; }
    }

    private Stack<Move> _history = new();

    public void Push(Move move)
    {
        _history.Push(move);
    }

    public Move Pop()
    {
        return !_history.TryPop(out var result) ? null : result;
    }
}
