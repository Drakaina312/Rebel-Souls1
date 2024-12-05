using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FunnelHandler : MonoBehaviour
{

    [HideInInspector] public bool CanSwitchToNextDialog = true;

    [SerializeField] private TextMeshProUGUI _textArea;
    [SerializeField] private Image _backGround;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private float _tipingSpeed;


    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private TextResizer _textResizer;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private HistoryFlowHandler _historyFlowHandler;

    private int _dialogIndex = 0;
    private List<IFunnelStruct> _falseChoiseLine;
    private bool _isTipeTextComplete;
    private Coroutine _tipeText;
    private WaitForSeconds _sleepTime;


    private void Start()
    {
        _sleepTime = new WaitForSeconds(_tipingSpeed);
    }

    public void SwipeDialog()
    {
        if (!CanSwitchToNextDialog || _falseChoiseLine == null)
            return;


        if (_isTipeTextComplete)
        {
            _dialogIndex += 1;
            if (_dialogIndex >= _falseChoiseLine.Count)
            {
                _historyFlowHandler.IsMainFlowActive = true;
                _historyFlowHandler.IsFunneChoiseActive = false;
                _falseChoiseLine = null;
                _historyFlowHandler.MoveToNextDialog();
                return;
            }

            ShowFunnelChoiseDialog(_falseChoiseLine[_dialogIndex].Text, _falseChoiseLine[_dialogIndex].FalseChoiseButtons,
                _falseChoiseLine[_dialogIndex].BG);
        }
        else
        {
            StartCoroutine(TipeFullText(_falseChoiseLine[_dialogIndex].Text));
        }
    }

    public void ActivateFunnelChoiseLine(List<IFunnelStruct> falseChoiseLine)
    {
        _falseChoiseLine = falseChoiseLine;
        CanSwitchToNextDialog = true;
        _dialogIndex = 0;
        ShowFunnelChoiseDialog(falseChoiseLine[_dialogIndex].Text, falseChoiseLine[_dialogIndex].FalseChoiseButtons,
            falseChoiseLine[_dialogIndex].BG);
    }

    private void ShowFunnelChoiseDialog(string textToShow, List<FalseChoiseButtons> falseChoiseButtons, Sprite backGround)
    {
        _tipeText = StartCoroutine(TypeText(textToShow));
        _backGround.sprite = backGround;

        _buttonsHandler.ActivedButtonsForFalseChoise(falseChoiseButtons, _falseChoiseLine[_dialogIndex].IsHaveButtons);

    }

    #region TypingText
    private IEnumerator TipeFullText(string fullText)
    {
        if (!_isTipeTextComplete)
        {
            StopCoroutine(_tipeText);
            _textArea.text = fullText;
            _isTipeTextComplete = true;
            yield return new WaitForSeconds(0.01f);

            _textResizer.UpdateSize();
        }
    }
    private IEnumerator TypeText(string fullText)
    {
        _isTipeTextComplete = false;
        _textArea.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            _textArea.text += fullText[i];
            _textResizer.UpdateSize();
            yield return _sleepTime;
        }

        _isTipeTextComplete = true;
    }
    #endregion


}
