using System;
using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public event Action OnClockReset;
    
    [SerializeField] private Transform secondHand;

    private const int MAX_SECONDS = 10;
    
    private int secondsRemaining = MAX_SECONDS;

    public void MoveClock()
    {
        secondsRemaining -= 1;
        RotateSecondHand();
        
        if (secondsRemaining > 0) return;
        
        OnClockReset?.Invoke();
        secondsRemaining = MAX_SECONDS;
    }

    private void RotateSecondHand()
    {
        var delta = 360f / MAX_SECONDS;
        var eulerAngles = secondHand.eulerAngles;
        var tween = new Tween<Vector3>(v => secondHand.eulerAngles = v, eulerAngles, eulerAngles + new Vector3(0, 0, delta), PlayerManager.MOVE_DELAY, Vector3.Lerp);
        tween.Play();
    }
}
