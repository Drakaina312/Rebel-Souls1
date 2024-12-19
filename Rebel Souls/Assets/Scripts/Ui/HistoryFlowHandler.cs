using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class HistoryFlowHandler : MonoBehaviour
{
    [HideInInspector] public bool CanWeGoNext;
    [HideInInspector] public bool CanWeGoNextFalseChoise;
    [HideInInspector] public bool IsFunnelChoiseActive;
    [HideInInspector] public bool IsMainFlowActive;


    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private TextResizer _textResizer;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private FunnelHandler _funnelHandler;
    [SerializeField] private AudioSource _audioSourceForVoices;
    [SerializeField] private AudioSource _audioSourceForEffects;



   [SerializeField] private TextMeshProUGUI _textArea;
    [SerializeField] private Image _backGround;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private float _tipingSpeed;


    private WaitForSeconds _sleepTime;
    private InGameDataBase _gameData;
    private MasterSave _masterSave;
    private bool _isTipeTextComplete;
    private int _dialogIndex;
    private Coroutine _tipeText;

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
        _masterSave.CurrentProfile.SaveStatsForFirstLaunch(_gameData.ActStatistics, _gameData.DIalogSequenceStart.ChapterSortingCondition);
        _masterSave.SaveAllData();
        IsMainFlowActive = true;
        ShowDialoge(_dialogIndex);
    }

    public void ChangeFlowHistory(DialogSequence historyPattern)
    {
        _gameData.DIalogSequenceStart = historyPattern;
        _masterSave.CurrentProfile.DialogIndex = 0;
        _masterSave.CurrentProfile.LastSaveChapterPath = historyPattern.PathToFile;
        IsMainFlowActive = true;
        ShowDialoge(0);
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {
        Debug.Log($"Swipe   1 = {IsFunnelChoiseActive} 2 = {IsMainFlowActive}");
        if (IsFunnelChoiseActive)
        {
            _funnelHandler.SwipeDialogWhenClicked();
            return;
        }

        if (!IsMainFlowActive)
            return;
        Debug.Log("Запуск из основного потока");
        MoveToNextDialog();
    }

    public void MoveToNextDialog()
    {
        if (_isTipeTextComplete)
        {
            _dialogIndex += 1;
            ShowDialoge(_dialogIndex);

            _notationHandler.DeActivaidNotation();
        }
        else
        {
            StartCoroutine(TipeFullText());
        }
    }

    private IEnumerator TipeFullText()
    {
        if (!_isTipeTextComplete)
        {
            StopCoroutine(_tipeText);
            _textResizer.UpdateSize(_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Text);
            _textArea.text = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Text;
            _isTipeTextComplete = true;
            yield return new WaitForSeconds(0.01f);

           
        }
    }
    private IEnumerator TypeText(string fullText)
    {
        _isTipeTextComplete = false;
        _textArea.text = "";
        Debug.Log(fullText);    
        _textResizer.UpdateSize(fullText);
        for (int i = 0; i < fullText.Length; i++)
        {
            _textArea.text += fullText[i];
           
            yield return _sleepTime;
        }

        _isTipeTextComplete = true;
    }

    private void ShowDialoge(int indexToShow)
    {
        Debug.Log("Index = " + indexToShow);
        _backGround.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].Background;

        ShowHeroyOnScene(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow]);

        for (int i = 0; i < _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].ButtonSetting.Count; i++)
            _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].ButtonSetting[i].WasChoised = false;



        _buttonsHandler.ActivedButtons(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow], this);


        if (_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].VoiceClip != null)
        {
            _audioSourceForVoices.clip = _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].VoiceClip;
            _audioSourceForVoices.Play();
        }
        else 
        { 
         _audioSourceForVoices.Stop();
        }

        if (_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].AudioEffectsClip != null)
        {
            _audioSourceForEffects.clip = _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].AudioEffectsClip;
            _audioSourceForEffects.Play();
        }
        else
        {
            _audioSourceForEffects.Stop();
        }

        if (_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].IsHaveNotation)
            StartCoroutine(_notationHandler.ActivaidNotation(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].Notation));

        _tipeText = StartCoroutine(TypeText(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].Text));


        if (_gameData.DIalogSequenceStart.StoryHierarhy.Count > indexToShow)
            _masterSave.CurrentProfile.DialogIndex = indexToShow;
        else
            _masterSave.CurrentProfile.DialogIndex = 0;

        _dialogIndex = indexToShow;
    }

    private void ShowHeroyOnScene(StoryHierarhy storyHierarhy)
    {
        _heroLeft.gameObject.SetActive(false);
        _heroRight.gameObject.SetActive(false);

        switch (storyHierarhy.HeroType)
        {
            case HeroType.HeroLeft:
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].HeroSprite;
                break;
            case HeroType.HeroRight:
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].HeroSprite;
                break;
        }
    }
    public void ShowHeroyOnScene(FunnelChoiseLine storyHierarhy)
    {
        _heroLeft.gameObject.SetActive(false);
        _heroRight.gameObject.SetActive(false);

        switch (storyHierarhy.HeroType)
        {
            case HeroType.HeroLeft:
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].HeroSprite;
                break;
            case HeroType.HeroRight:
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].HeroSprite;
                break;
        }
    }
    private void OnDestroy()
    {
        _masterSave.SaveAllData();
    }

    public void RepeatDialog()
    {
        IsMainFlowActive = false;
        Debug.Log("Повтор выборов");

        if (_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].ButtonSetting.FindAll(x => x.WasChoised == false).Count == 0)
        {
            Debug.Log("Все кнопки были выбраны");
            Debug.Log(_dialogIndex);
            IsMainFlowActive = true;
            _funnelHandler.IsCircleFunnel = false;
            IsFunnelChoiseActive = false;
            _isTipeTextComplete = true;
            MoveToNextDialog();
            return;
        }

        Debug.Log($"Включение под индексом  {_dialogIndex}");
        _backGround.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Background;

        ShowHeroyOnScene(_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex]);

        _buttonsHandler.ActivedButtons(_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex], this);


        if (_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].IsHaveNotation)
            StartCoroutine(_notationHandler.ActivaidNotation(_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Notation));

        _tipeText = StartCoroutine(TypeText(_gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Text));
        Debug.Log($"Статус основного потока {IsMainFlowActive}");
    }
}

