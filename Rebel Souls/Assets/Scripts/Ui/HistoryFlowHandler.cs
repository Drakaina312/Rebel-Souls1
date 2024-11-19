using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class HistoryFlowHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textArea;
    [SerializeField] private Image _backGround;
    [SerializeField] private float _tipingSpeed;
    private WaitForSeconds _sleepTime;
    private InGameDataBase _gameData;
    [HideInInspector]public bool CanWeGoNext;
    private bool _isTipeTextComplete;
    private int _index;
    private Coroutine _tipeText;
    private Coroutine _historyFlow;
    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private TextResizer _textResizer;
    private bool _isWeHaveButtons;

    [Inject]
    private void Construct(InGameDataBase gameData, InputSystem_Actions input)
    {
        _gameData = gameData;
        _sleepTime = new WaitForSeconds(_tipingSpeed);
        input.Player.Attack.started += SwipeStory;
    }

    private void Start()
    {

        _historyFlow = StartCoroutine(ControlingHistoryFlowCoroutine());
    }

    public void ChangeFlowHistory(HistoryPattern historyPattern)
    { 
        _gameData.HistoryPattern = historyPattern;
        StopCoroutine(_historyFlow);
        _historyFlow = StartCoroutine(ControlingHistoryFlowCoroutine());
        
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {
        if (_isTipeTextComplete)
        {
            CanWeGoNext = true;
            _notationHandler.DeActivaidNotation();
        }
        else 
        { 
            StartCoroutine(TipeFullText()); 
        }
        

    }

    private IEnumerator TipeFullText()
    {
        StopCoroutine(_tipeText);
        _textArea.text = _gameData.HistoryPattern.StoryHierarhy[_index].Text;
        _isTipeTextComplete = true;
        Debug.Log("конец печати");
        yield return new WaitForSeconds(0.01f);

        _textResizer.UpdateSize();
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

    private IEnumerator ControlingHistoryFlowCoroutine()
    {
        _index = 0;
        foreach (var storyLine in _gameData.HistoryPattern.StoryHierarhy)
        {
            CanWeGoNext = false;
            if (storyLine.HeroType == HeroType.HeroLeft) 
            {
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = storyLine.HeroSprite;
            }

            if (storyLine.HeroType == HeroType.HeroRight)
            {
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = storyLine.HeroSprite;
            }
            if (storyLine.ButtonSetting.Count > 0)
            {
                _buttonsHandler.ActivedButtons(storyLine.ButtonSetting, this);
                CanWeGoNext = false;

            }

            else if (storyLine.ButtonSetting.Count == 0)
            {
                _buttonsHandler.DeActivatedButtons();
            }
            if (storyLine.Notation.Length > 0) 
            {
                StartCoroutine(_notationHandler.ActivaidNotation(storyLine.Notation));
            }

            
            _tipeText = StartCoroutine(TypeText(storyLine.Text));
            _backGround.sprite = storyLine.Background;
            yield return new WaitWhile(() => CanWeGoNext == false);
            _heroLeft.gameObject.SetActive(false);
            _heroRight.gameObject.SetActive(false);
            _index++;
        }


    }
}

