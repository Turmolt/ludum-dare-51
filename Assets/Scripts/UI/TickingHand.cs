using System.Collections;
using System.Collections.Generic;
using Kickflip;
using Shapes;
using UnityEngine;

public class TickingHand : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Transform hand;
    [SerializeField] private Disc disc;

    private int currentColor = 0;

    private float runtime = 0f;
    private int currentSeconds = 0;
    
    void Update()
    {
        runtime = Mathf.Clamp01(runtime + Time.deltaTime/10f);
        hand.eulerAngles = new Vector3(0, 0, -360f * runtime);
        if (runtime >= 1f)
        {
            runtime = 0f;
            SetColor();
        }
    }

    void SetColor()
    {
        currentColor = (currentColor + 1) % colors.Length;
        disc.ColorOuter = colors[currentColor];
    }
}
