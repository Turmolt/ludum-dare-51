using System.Collections;
using System.Collections.Generic;
using Kickflip;
using Shapes;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private Disc background;
    [SerializeField] private Color red;
    [SerializeField] private Color green;
    [SerializeField] private Color blue;

    private enum State { Red, Green, Blue }

    private State currentState;
    
    public void ChangeColor()
    {
        NextState();
    }

    private void NextState()
    {
        currentState = (State) (((int)currentState + 1) % 3);
        var nextColor =  GetColor();
        var tween = new Tween<Color>(c => background.ColorOuter = c, background.ColorOuter, nextColor, PlayerManager.MOVE_DELAY, Color.Lerp);
        tween.Play();
    }

    private Color GetColor()
    {
        return currentState switch
        {
            State.Green => green,
            State.Blue => blue,
            _ => red
        };
    }
}
