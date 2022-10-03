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
        runtime += Time.deltaTime;
        if (runtime >= 1f)
        {
            runtime = 0f;
            TickHand();
        }
    }

    private void TickHand()
    {
        currentSeconds++;
        if (currentSeconds >= 10)
        {
            currentSeconds = 0;
            SetColor();
        }
        RotateSecondHand();
    }

    void SetColor()
    {
        currentColor = (currentColor + 1) % colors.Length;
        var nextColor = colors[currentColor];
        var tween = new Tween<Color>(c => disc.ColorOuter = c, disc.ColorOuter, nextColor, PlayerManager.MOVE_DELAY, Color.Lerp);
        tween.Play();
    }
    
    private void RotateSecondHand()
    {
        var rotation = GetRotationAtSecond(currentSeconds);
        var tween = new Tween<Quaternion>(v => hand.transform.rotation = v, hand.transform.rotation, rotation, PlayerManager.MOVE_DELAY, Quaternion.Slerp);
        tween.Play();
    }

    Quaternion GetRotationAtSecond(int seconds)
    {
        var delta = 360f / 10;
        return Quaternion.Euler(new Vector3(0, 0, -delta * seconds));
    }
}
