using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public const int MAX_HEALTH = 3;
    public Observable<int> Health = new(MAX_HEALTH);

    public void ResetHealth()
    {
        Health.value = MAX_HEALTH;
    }
    
    public void SetHealth(int value)
    {
        Health.value = value;
    }

    public void LoseHealth()
    {
        Health.value = Math.Max(0, Health.value - 1);
    }

    public void GainHealth()
    {
        Health.value = Math.Min(Health.value + 1, MAX_HEALTH);
    }
}
