using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] healthDisplay;
    [SerializeField] private TMP_Text numberMovesLabel;
    
    [Header("References")]
    [SerializeField] private LifeManager life;
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
}
