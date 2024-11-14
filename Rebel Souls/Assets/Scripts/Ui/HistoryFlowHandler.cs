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
    private bool _canWeGoNext;
    private bool _isTipeTextComplete;
    private int _index;
    private Coroutine _tipeText;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private ButtonsHandler _buttonsHandler;

    [Inject]
    private void Construct(InGameDataBase gameData, InputSystem_Actions input)
    {
        _gameData = gameData;
        _sleepTime = new WaitForSeconds(_tipingSpeed);
        input.Player.Attack.started += SwipeStory;
    }

    private void Start()
    {
        StartCoroutine(ControlingHistoryFlowCoroutine());
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {
        if (_isTipeTextComplete)
        {
            _canWeGoNext = true;
        }
        else 
        { 
            TipeFullText(); 
        }

    }

    private void TipeFullText()
    {
        StopCoroutine(_tipeText);
        _textArea.text = _gameData.HistoryPattern.StoryHierarhy[_index].Text;
        _isTipeTextComplete = true;
    }
    private IEnumerator TypeText(string fullText)
    {
        _isTipeTextComplete = false;
        _textArea.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            _textArea.text += fullText[i];
            yield return _sleepTime;
        }

        _isTipeTextComplete = true;
    }

    private IEnumerator ControlingHistoryFlowCoroutine()
    {
        _index = 0;
        foreach (var storyLine in _gameData.HistoryPattern.StoryHierarhy)
        {
            _canWeGoNext = false;
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

            if (storyLine.ButtonsAmound == 0)
            {
                _buttonsHandler.DeActivatedButtons();

            }

            else
            {
                _buttonsHandler.ActivedButtons(storyLine.ButtonsAmound, new List<string>());
                    
             }
          
            _tipeText = StartCoroutine(TypeText(storyLine.Text));
            _backGround.sprite = storyLine.Background;
            yield return new WaitWhile(() => _canWeGoNext == false);
            _heroLeft.gameObject.SetActive(false);
            _heroRight.gameObject.SetActive(false);
            _index++;
        }


    }
}

