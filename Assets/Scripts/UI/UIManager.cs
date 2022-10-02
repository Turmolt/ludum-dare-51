using System.Collections;
using System.Collections.Generic;
using Kickflip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text numberMovesLabel;
    [SerializeField] private CanvasGroup winScreen;
    
    [Header("References")]
    [SerializeField] private PlayerManager manager;
    
    void Start()
    {
        manager.NumberMoves.onValueUpdated += SetNumberMoves;
        SetNumberMoves();
    }

    private void SetNumberMoves()
    {
        numberMovesLabel.text = $"{manager.NumberMoves.value}";
    }

    public void ShowWinScreen()
    {
        winScreen.interactable = true;
        winScreen.blocksRaycasts = true;
        var tween = new Tween<float>(f => winScreen.alpha = f, winScreen.alpha, 1f, 0.5f, Mathf.Lerp);
        tween.Play();
    }
}
