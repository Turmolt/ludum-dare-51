using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public const float MOVE_DELAY = 0.15f;

    [SerializeField] private ClockManager clock;
    [SerializeField] private EnergyManager energy;
    [SerializeField] private PlayerMovement movement;

    private float timeSinceLastMove = MOVE_DELAY;
    
    void Start()
    {
        clock.OnClockReset += ChangeEnergy;
    }

    void Update()
    {
        if (timeSinceLastMove < MOVE_DELAY)
        {
            timeSinceLastMove += Time.deltaTime;
            return;
        }
        
        var input = GetInput();
        if (input.magnitude != 0)
        {
            if (!movement.Move(input)) return;
            
            timeSinceLastMove = 0f;
            clock.MoveClock();
        }
    }

    private void ChangeEnergy()
    {
        energy.ChangeColor();
    }

    Vector2 GetInput()
    {
        var left = Input.GetKeyDown(KeyCode.LeftArrow);
        var right = Input.GetKeyDown(KeyCode.RightArrow);
        var up = Input.GetKeyDown(KeyCode.UpArrow);
        var down = Input.GetKeyDown(KeyCode.DownArrow);
        
        var input = new Vector2();

        if (right) input.x += 1;
        if (left) input.x -= 1;
        if (up) input.y += 1;
        if (down) input.y -= 1;

        return input;
    }
}
