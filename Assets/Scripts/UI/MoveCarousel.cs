using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCarousel : MonoBehaviour
{
    [SerializeField] private Image[] indicators;
    [SerializeField] private EnergyManager energy;
    [SerializeField] private ClockManager clock;

    void Start()
    {
        clock.CurrentSeconds.onValueUpdated += UpdateDisplay;
        
        UpdateDisplay();
    }
    
    void UpdateDisplay()
    {
        var currentColor = energy.GetColor();
        var nextColor = energy.GetColor(energy.NextState);
        var movesRemainingAsCurrentColor = clock.CurrentSeconds;
        for (var i = 0; i < indicators.Length; i++)
        {
            var current = indicators[i];
            var targetColor = i < movesRemainingAsCurrentColor ? currentColor : nextColor;
            current.color = targetColor;
        }
    }
    
    
}
