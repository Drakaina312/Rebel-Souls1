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
    [HideInInspector] public bool IsFunneChoiseActive;


    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private TextResizer _textResizer;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private FunnelHandler _funnelHandler;


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
    internal bool IsMainFlowActive;

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
        ShowDialoge(_dialogIndex);
    }

    public void ChangeFlowHistory(DialogSequence historyPattern)
    {
        _gameData.DIalogSequenceStart = historyPattern;
        _masterSave.CurrentProfile.DialogIndex = 0;
        _masterSave.CurrentProfile.LastSaveChapterPath = historyPattern.PathToFile;

        ShowDialoge(0);
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {

        if (IsFunneChoiseActive)
        {
            _funnelHandler.SwipeDialog();
            return;
        }

        if (!IsMainFlowActive)
            return;

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
            _textArea.text = _gameData.DIalogSequenceStart.StoryHierarhy[_dialogIndex].Text;
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

    private void ShowDialoge(int indexToShow)
    {
        Debug.Log("Index = " + indexToShow);
        _backGround.sprite = _gameData.DIalogSequenceStart.StoryHierarhy[indexToShow].Background;

        ShowHeroyOnScene(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow]);

        _buttonsHandler.ActivedButtons(_gameData.DIalogSequenceStart.StoryHierarhy[indexToShow], this);


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
    private void OnDestroy()
    {
        _masterSave.SaveAllData();
    }
}

