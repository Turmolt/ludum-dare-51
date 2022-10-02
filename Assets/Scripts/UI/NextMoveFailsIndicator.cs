using System.Collections;
using System.Collections.Generic;
using Kickflip;
using UnityEngine;
using UnityEngine.UI;

public class NextMoveFailsIndicator : MonoBehaviour
{
    [SerializeField] private Image arrow;
    [SerializeField] private CanvasGroup canvasGroup;

    private const float SHOW_TIME = 1f;
    private Vector3 _startPosition;

    public bool Showing => showing;
    private bool showing = false;

    void Start()
    {
        _startPosition = arrow.transform.localPosition;
        canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        showing = true;
        var tween = new Tween<float>(f => canvasGroup.alpha = f, canvasGroup.alpha, 1f, PlayerManager.MOVE_DELAY, Mathf.Lerp);
        tween.OnComplete = () =>
        {
            var tween = new Tween<float>(f => canvasGroup.alpha = f, canvasGroup.alpha, 0f, PlayerManager.MOVE_DELAY, Mathf.Lerp);
            tween.Delay = SHOW_TIME;
            tween.OnComplete = () => { showing = false; };
            tween.Play();
        };
        tween.Play();
    }

    void Update()
    {
        if (!showing) return;
        arrow.transform.localPosition = _startPosition + new Vector3(0, 10f, 0) * Mathf.Sin(Time.time * 3f);
    }
}
