using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelHelper : MonoBehaviour
{
    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
