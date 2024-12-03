using Cysharp.Threading.Tasks;
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
    private MasterSave _masterSave;
    [HideInInspector] public bool CanWeGoNext;
    [HideInInspector] public bool CanWeGoNextFalseChoise;

    private bool _isTipeTextComplete;
    private int _index;
    private Coroutine _tipeText;
    public Coroutine HistoryFlow;
    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private TextResizer _textResizer;
    private bool _isWeHaveButtons;

    [Inject]
    private void Construct(InGameDataBase gameData, InputSystem_Actions input, MasterSave masterSave)
    {
        _gameData = gameData;
        _masterSave = masterSave;
        _sleepTime = new WaitForSeconds(_tipingSpeed);
        input.Player.Attack.started += SwipeStory;
    }

    private void Start()
    {

        HistoryFlow = StartCoroutine(ControlingHistoryFlowCoroutine());
        _masterSave.CurrentProfile.SaveStatsForFirstLaunch(_gameData.ActStatistics, _gameData.DIalogSequenceStart.ChapterSortingCondition);
        _masterSave.SaveAllData();
    }

    public void ChangeFlowHistory(DialogSequence historyPattern)
    {
        _gameData.DIalogSequenceStart = historyPattern;
        _masterSave.CurrentProfile.DialogIndex = 0;
        _masterSave.CurrentProfile.LastSaveChapterPath = historyPattern.PathToFile;
        StopCoroutine(HistoryFlow);
        HistoryFlow = StartCoroutine(ControlingHistoryFlowCoroutine());

    }

    public async UniTask FalseChoiseAsync(List<StoryHierarhy> storyHierarhies)
    {
        _index = 0;
        Debug.Log("False Chaoise Async");
        for (int i = _index; i < storyHierarhies.Count; i++)
        {
            CanWeGoNext = false;
            Debug.Log(_index);
            if (storyHierarhies[_index].HeroType == HeroType.HeroLeft)
            {
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = storyHierarhies[_index].HeroSprite;
            }

            if (storyHierarhies[_index].HeroType == HeroType.HeroRight)
            {
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = storyHierarhies[_index].HeroSprite;
            }

            if (storyHierarhies[_index].ButtonSetting.Count > 0)
            {
                await _buttonsHandler.ActivedButtonsAsync(storyHierarhies[_index].ButtonSetting, this);
                CanWeGoNextFalseChoise = false;
            }
            else if (storyHierarhies[_index].ButtonSetting.Count == 0)
            {
                _buttonsHandler.DeActivatedButtons();
            }

            if (storyHierarhies[_index].Notation.Length > 0)
            {
                StartCoroutine(_notationHandler.ActivaidNotation(storyHierarhies[_index].Notation));
            }


            _tipeText = StartCoroutine(TypeText(storyHierarhies[_index].Text));
            Debug.Log(_tipeText);
            _backGround.sprite = storyHierarhies[_index].Background;


            await UniTask.WaitWhile(() => CanWeGoNextFalseChoise == false);

            _heroLeft.gameObject.SetActive(false);
            _heroRight.gameObject.SetActive(false);
            if (storyHierarhies[_index].IsFalseChoiseFinish)
            {
                _gameData.DialogIndex = storyHierarhies[_index].IndexToStartFlow;
                HistoryFlow = StartCoroutine(ControlingHistoryFlowCoroutine());
                //CanWeGoNext = true;
            }
            _index++;
            //if (storyHierarhies.Count > _index)
            //    _masterSave.CurrentProfile.DialogIndex = _index;
            //else
            //    _masterSave.CurrentProfile.DialogIndex = 0;

            // Сохранение и загрузка воронки
        }
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {
        if (_isTipeTextComplete)
        {
            if (CanWeGoNextFalseChoise)
                CanWeGoNext = true;
            CanWeGoNextFalseChoise = true;
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
        _textArea.text = _gameData.DIalogSequenceStart.StoryHierarhy[_index].Text;
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

    private void OnDestroy()
    {
        _masterSave.SaveAllData();
    }

    private IEnumerator ControlingHistoryFlowCoroutine()
    {
        _index = 0;
        if (_gameData.DialogIndex != 0)
        {
            _index = _gameData.DialogIndex;
        }
        Debug.Log("Загрузка индекса " + _index);
        for (int i = _index; i < _gameData.DIalogSequenceStart.StoryHierarhy.Count; i++)
        {
            CanWeGoNext = false;
            if (_gameData.DIalogSequenceStart.StoryHierarhy[_index].HeroType == HeroType.HeroLeft)
            {
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_index].HeroSprite;
            }

            if (_gameData.DIalogSequenceStart.StoryHierarhy[_index].HeroType == HeroType.HeroRight)
            {
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_index].HeroSprite;
            }
            if (_gameData.DIalogSequenceStart.StoryHierarhy[_index].ButtonSetting.Count > 0)
            {
                _buttonsHandler.ActivedButtons(_gameData.DIalogSequenceStart.StoryHierarhy[_index].ButtonSetting, this);
                CanWeGoNext = false;

            }

            else if (_gameData.DIalogSequenceStart.StoryHierarhy[_index].ButtonSetting.Count == 0)
            {
                _buttonsHandler.DeActivatedButtons();
            }
            if (_gameData.DIalogSequenceStart.StoryHierarhy[_index].Notation.Length > 0)
            {
                StartCoroutine(_notationHandler.ActivaidNotation(_gameData.DIalogSequenceStart.StoryHierarhy[_index].Notation));
            }


            _tipeText = StartCoroutine(TypeText(_gameData.DIalogSequenceStart.StoryHierarhy[_index].Text));
            Debug.Log(_gameData.DIalogSequenceStart.StoryHierarhy[_index].Text);
            _backGround.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_index].Background;
            yield return new WaitWhile(() => CanWeGoNext == false);
            _heroLeft.gameObject.SetActive(false);
            _heroRight.gameObject.SetActive(false);
            _index++;
            if (_gameData.DIalogSequenceStart.StoryHierarhy.Count > _index)
                _masterSave.CurrentProfile.DialogIndex = _index;
            else
                _masterSave.CurrentProfile.DialogIndex = 0;
        }
    }
}

