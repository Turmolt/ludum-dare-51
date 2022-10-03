using System.Collections;
using System.Collections.Generic;
using Kickflip;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text numberMovesLabel;
    [SerializeField] private TMP_Text numberMovesLabelWin;
    [SerializeField] private TMP_Text numberUndoLabelWin;
    [SerializeField] private CanvasGroup winScreen;
    
    private PlayerManager _player;
    
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _player.NumberMoves.onValueUpdated += SetNumberMoves;
        SetNumberMoves();
    }

    private void SetNumberMoves()
    {
        numberMovesLabel.text = $"{_player.NumberMoves.value}";
    }

    public void ShowWinScreen()
    {
        winScreen.interactable = true;
        winScreen.blocksRaycasts = true;
        numberMovesLabelWin.text = $"{_player.NumberMoves.value}";
        numberUndoLabelWin.text = $"{_player.NumberUndos.value}";
        var tween = new Tween<float>(f => winScreen.alpha = f, winScreen.alpha, 1f, 0.5f, Mathf.Lerp);
        tween.Play();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
