using System;
using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public event Action OnClockReset;

    public readonly Observable<int> CurrentSeconds = new(MAX_SECONDS);
    
    [SerializeField] private Transform secondHand;

    private const int MAX_SECONDS = 10;

    void Start()
    {
        CurrentSeconds.onValueUpdated += RotateSecondHand;
    }

    public void Set(int value)
    {
        CurrentSeconds.value = value;
    }

    public void MoveClock()
    {
        CurrentSeconds.value -= 1;

        if (CurrentSeconds.value > 0) return;
        
        OnClockReset?.Invoke();
        CurrentSeconds.value = MAX_SECONDS;
    }

    private void RotateSecondHand()
    {
        var rotation = GetRotationAtSecond(CurrentSeconds.value);
        var tween = new Tween<Quaternion>(v => secondHand.rotation = v, secondHand.rotation, rotation, PlayerManager.MOVE_DELAY, Quaternion.Slerp);
        tween.Play();
    }

    Quaternion GetRotationAtSecond(int seconds)
    {
        var delta = 360f / MAX_SECONDS;
        return Quaternion.Euler(new Vector3(0, 0, -delta * seconds));
    }
}
