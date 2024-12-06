using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FunnelHandler : MonoBehaviour
{

    [HideInInspector] public bool CanSwitchToNextDialog = true;
    [HideInInspector] public bool IsCircleFunnel { get; set; }
    [HideInInspector] public bool IsFindChoise;
    [HideInInspector] public bool IsRightChoiseFinded;

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
    private List<FunnelChoiseLine> _funnelChoiseLine;
    private FunnelChoiseLine _findChoiseLine;
    private bool _isTipeTextComplete;
    private Coroutine _tipeText;
    private WaitForSeconds _sleepTime;

    private void Start()
    {
        _sleepTime = new WaitForSeconds(_tipingSpeed);
    }

    public void SwipeDialogWhenClicked()
    {
        if (!CanSwitchToNextDialog)
            return;

        if (CanSwitchToNextDialog && IsFindChoise)
        {
            if (!IsRightChoiseFinded)
            {
                IsFindChoise = false;
                _historyFlowHandler.IsMainFlowActive = true;
                _historyFlowHandler.IsFunnelChoiseActive = false;
                CanSwitchToNextDialog = false;
                _historyFlowHandler.RepeatDialog();
                return;
            }
            else
            {
                IsFindChoise = false;
                _historyFlowHandler.IsMainFlowActive = true;
                _historyFlowHandler.IsFunnelChoiseActive = false;
                CanSwitchToNextDialog = false;
                _historyFlowHandler.MoveToNextDialog();
                return;
            }
        }

        if (_isTipeTextComplete)
        {
            _dialogIndex += 1;
            if (_dialogIndex >= _funnelChoiseLine.Count)
            {
                Debug.Log(IsCircleFunnel + " = Circle?");
                if (IsCircleFunnel)
                {
                    _dialogIndex = 0;
                    _historyFlowHandler.RepeatDialog();
                    return;
                }

                if (IsFindChoise)
                {

                }

                _historyFlowHandler.IsMainFlowActive = true;
                _historyFlowHandler.IsFunnelChoiseActive = false;
                _funnelChoiseLine = null;
                Debug.Log("ЗАпуск из FunnelHandler");
                _historyFlowHandler.MoveToNextDialog();
                return;
            }

            ShowFunnelChoiseDialog(_funnelChoiseLine[_dialogIndex].Text, _funnelChoiseLine[_dialogIndex].FunnelChoiseButtons,
                _funnelChoiseLine[_dialogIndex].Background);
        }
        else
        {
            StartCoroutine(TipeFullText(_funnelChoiseLine[_dialogIndex].Text));
        }
    }

    public void ActivateFunnelChoiseLine(List<FunnelChoiseLine> falseChoiseLine)
    {
        _funnelChoiseLine = falseChoiseLine;
        CanSwitchToNextDialog = true;
        _dialogIndex = 0;
        ShowFunnelChoiseDialog(falseChoiseLine[_dialogIndex].Text, falseChoiseLine[_dialogIndex].FunnelChoiseButtons,
            falseChoiseLine[_dialogIndex].Background);
    }
    public void ActivateFunnelChoiseLine(FunnelChoiseLine funnelChoise)
    {
        _funnelChoiseLine = null;
        _findChoiseLine = funnelChoise;
        CanSwitchToNextDialog = true;
        _dialogIndex = 0;
        ShowFunnelChoiseDialog(funnelChoise.Text, funnelChoise.FunnelChoiseButtons,
            funnelChoise.Background, false, true);
    }

    private void ShowFunnelChoiseDialog(string textToShow, List<FunnelChoiseButtons> falseChoiseButtons, Sprite backGround,
        bool isCircleChoise = false, bool isFindChjoise = false)
    {
        _historyFlowHandler.StopAllCoroutines();
        StopAllCoroutines();
        _tipeText = StartCoroutine(TypeText(textToShow));
        _backGround.sprite = backGround;
        if (!isFindChjoise)
            _buttonsHandler.ActivedButtonsForFunnelChoise(falseChoiseButtons, _funnelChoiseLine[_dialogIndex].IsHaveButtons);
        else
            _buttonsHandler.ActivedButtonsForFunnelChoise(falseChoiseButtons, _findChoiseLine.IsHaveButtons);

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
