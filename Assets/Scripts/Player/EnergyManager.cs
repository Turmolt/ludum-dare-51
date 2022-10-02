using System.Collections;
using System.Collections.Generic;
using Kickflip;
using Shapes;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public Observable<State> CurrentState = new(State.Red);
    public State NextState => (State)(((int)CurrentState.value + 1) % 3); 
    public State LastState => (State)(((int)CurrentState.value - 1 + 3) % 3); 
    public State StateAfterMove => clock.CurrentSeconds <= 1 ? NextState : CurrentState;

    [SerializeField] private ClockManager clock;
    [SerializeField] private Disc background;
    [SerializeField] private Color red;
    [SerializeField] private Color green;
    [SerializeField] private Color blue;

    public enum State
    {
        White = -1,
        Red = 0,
        Green = 1,
        Blue = 2
    }

    public void ChangeColor()
    {
        ChangeToNextState();
    }

    private void ChangeToNextState()
    {
        CurrentState.value = NextState;
        UpdateColor();
    }

    private void UpdateColor()
    {
        var nextColor =  GetColor();
        
        if (nextColor == background.ColorOuter) return;
        
        var tween = new Tween<Color>(c => background.ColorOuter = c, background.ColorOuter, nextColor, PlayerManager.MOVE_DELAY, Color.Lerp);
        tween.Play();
    }

    public Color GetColor()
    {
        return GetColor(CurrentState.value);
    }
    
    public Color GetColor(State state)
    {
        return state switch
        {
            State.Green => green,
            State.Blue => blue,
            _ => red
        };
    }

    public void Set(State state)
    {
        CurrentState.value = state;
        UpdateColor();
    }
}
