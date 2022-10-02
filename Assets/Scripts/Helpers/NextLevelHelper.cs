using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelHelper : MonoBehaviour
{
    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
