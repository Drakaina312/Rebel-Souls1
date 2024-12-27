using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using Zenject;

public class SlideHandler : MonoBehaviour
{

    [HideInInspector] public bool IsMainFlowActive;
    public bool IsInputActive { get; private set; } = true;

    [SerializeField] private NotationHandler _notationHandler;
    [SerializeField] private TextResizer _textResizer;
    [SerializeField] private ButtonsHandler _buttonsHandler;
    [SerializeField] private AudioSource _audioSourceForVoices;
    [SerializeField] private AudioSource _audioSourceForEffects;


    [SerializeField] private TextMeshProUGUI _textArea;
    [SerializeField] private Image _backGround;
    [SerializeField] private Image _heroLeft;
    [SerializeField] private Image _heroRight;
    [SerializeField] private float _tipingSpeed;


    private WaitForSeconds _sleepTime;
    private InGameDataBase _gameData;
    private InputSystem_Actions _input;
    private MasterSave _masterSave;
    private bool _isTipeTextComplete;
    private string _slideIndex;
    private Coroutine _tipeText;
    private StoryLine _storyLine;
    internal bool IsCirleChoise;
    private StatsBook _currentSaveStats;

    [Inject]
    private void Construct(InGameDataBase gameData, InputSystem_Actions input, MasterSave masterSave)
    {
        _gameData = gameData;
        _input = input;
        _storyLine = gameData.StoryLine;
        _masterSave = masterSave;
        _sleepTime = new WaitForSeconds(_tipingSpeed);
        input.Player.Attack.started += SwipeStory;
    }


    private void Start()
    {

        Debug.Log(_masterSave);
        Debug.Log(_gameData.ActStatistics);
        Debug.Log(_gameData.StoryLine.ChapterSortingCondition);


        _masterSave.CurrentProfile.SaveStatsForFirstLaunch(_gameData.ActStatistics, _gameData.StoryLine.ChapterSortingCondition);
        ///�������� ���� ������ ������ �� ������� ���������� ������ ����� ���������� ���������
        _currentSaveStats = _masterSave.CurrentProfile.FindChapterStatsFromSave(_gameData.StoryLine.ChapterSortingCondition);
        _storyLine = _gameData.StoryLine;
        if (_gameData.IsContiniueStory)
        {
            _slideIndex = _currentSaveStats.SavedIndexes.Last();
        }
        if (_gameData.IsRestartChapter)
        {
            _slideIndex = _storyLine.SlideData.FirstOrDefault().Key;
            _currentSaveStats.SavedIndexes = null;
        }
        _masterSave.SaveAllData();
        IsMainFlowActive = true;
        ShowSlide(_slideIndex);
    }

    public void CalculateSlideWork()
    {
        if (_isTipeTextComplete)
        {
            _notationHandler.DeActivaidNotation();
            if (!_storyLine.SlideData[_slideIndex].IsHaveChecking�ondition)
                _slideIndex = FindNextSlideToShow(_slideIndex);
            else if (_storyLine.SlideData[_slideIndex].IsHaveChecking�ondition)
            {
                bool isConditionGood = false;

                foreach (var allStats in _storyLine.SlideData[_slideIndex].ChekingConditions)
                {
                    if (CheckCondition(allStats, _currentSaveStats, ref isConditionGood))
                    {
                        Debug.Log("For Openning Using = " + allStats.SlideToOpen);
                        _slideIndex = allStats.SlideToOpen;
                        break;
                    }
                    else
                    {
                        Debug.Log("Else");
                    }
                }
            }
            ShowSlide(_slideIndex);
        }
        else
            StartCoroutine(TipeFullText(_slideIndex));
    }


    private bool CheckSlidePassing(ChekingMultiConditions chekingConditions, StatsBook statsBook)
    {
        if (chekingConditions.SlideCheck)
        {
            if (!_currentSaveStats.IsSlideIndexExistInSave(chekingConditions.StatName))
                return false;
        }

        return true;
    }

    private bool CheckCondition(ChekingConditions allStats, StatsBook savedStats, ref bool isConditionGood)
    {
        foreach (var statToChek in allStats.Stat)
        {

            if (statToChek.SlideCheck)
            {
                Debug.Log("�������� ������");
                if (!CheckSlidePassing(statToChek, savedStats))
                    return false;
                else
                    isConditionGood = true;
            }
            else
                switch (statToChek.Cnd)
                {
                    case ChekingEnums.More:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount > statToChek.StatValue)
                        {
                            isConditionGood = true;
                        }
                        else
                        {
                            isConditionGood = false;
                            return false;
                        }
                        break;
                    case ChekingEnums.Less:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount < statToChek.StatValue)
                        {
                            isConditionGood = true;
                        }
                        else
                        {
                            isConditionGood = false;
                            return false;
                        }
                        break;
                    case ChekingEnums.Equal:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount == statToChek.StatValue)
                        {
                            isConditionGood = true;
                        }
                        else
                        {
                            isConditionGood = false;
                            return false;
                        }
                        break;
                    case ChekingEnums.MoreOrEqual:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount >= statToChek.StatValue)
                        {
                            isConditionGood = true;
                        }
                        else
                        {
                            isConditionGood = false;
                            return false;
                        }
                        break;
                    case ChekingEnums.LessOrEqual:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount <= statToChek.StatValue)
                        {
                            isConditionGood = true;
                        }
                        else
                        {
                            isConditionGood = false;
                            return false;
                        }
                        break;
                }
        }
        return isConditionGood;
    }

    public void ShowSlide(string slideIndex)
    {
        _backGround.sprite = _storyLine.SlideData[slideIndex].Background;

        List<string> openedSlides = _currentSaveStats.SavedIndexes?.ToList();
        if (openedSlides == null)
        {
            openedSlides = new List<string>() { slideIndex };
            _currentSaveStats.SavedIndexes = openedSlides.ToArray();
        }
        else
        {
            openedSlides.Add(slideIndex);
            _currentSaveStats.SavedIndexes = openedSlides.ToArray();
        }

        ShowHeroyOnScene(_storyLine.SlideData[slideIndex]);

        if (_storyLine.SlideData[slideIndex].IsHaveButtons && !IsCirleChoise)
            for (int i = 0; i < _storyLine.SlideData[slideIndex].ButtonSetting.Count; i++)
                _storyLine.SlideData[slideIndex].ButtonSetting[i].WasChoised = false;

        if (_buttonsHandler.ActivateButtons(_storyLine.SlideData[slideIndex], this) == false)
            return;



        if (_storyLine.SlideData[slideIndex].VoiceClip != null)
        {
            _audioSourceForVoices.clip = _storyLine.SlideData[slideIndex].VoiceClip;
            _audioSourceForVoices.Play();
        }
        else
        {
            _audioSourceForVoices.Stop();
        }

        if (_storyLine.SlideData[slideIndex].AudioEffectsClip != null)
        {
            _audioSourceForEffects.clip = _storyLine.SlideData[slideIndex].AudioEffectsClip;
            _audioSourceForEffects.Play();
        }
        else
        {
            _audioSourceForEffects.Stop();
        }

        if (_storyLine.SlideData[slideIndex].IsHaveNotation)
            _notationHandler.ActivaidNotation(_storyLine.SlideData[slideIndex].Notation);

        _tipeText = StartCoroutine(TypeText(_storyLine.SlideData[slideIndex].Text));
    }

    private string FindNextSlideToShow(string currentSlideIndex)
    {
        if (!_storyLine.SlideData[currentSlideIndex].IsHaveChecking�ondition)
            return _storyLine.SlideData[currentSlideIndex].NextSlideToOpen;
        else
        {
            // �������� ������� ���� ����
            return null;
        }
    }

    private void SwipeStory(InputAction.CallbackContext context)
    {
        if (!IsMainFlowActive)
            return;

        CalculateSlideWork();
    }

    private void ShowHeroyOnScene(SlideData slideData)
    {
        _heroLeft.gameObject.SetActive(false);
        _heroRight.gameObject.SetActive(false);

        switch (slideData.HeroType)
        {
            case HeroType.HeroLeft:
                _heroLeft.gameObject.SetActive(true);
                _heroLeft.sprite = slideData.HeroSprite;
                break;
            case HeroType.HeroRight:
                _heroRight.gameObject.SetActive(true);
                _heroRight.sprite = slideData.HeroSprite;
                break;
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

    private IEnumerator TipeFullText(string slideIndex)
    {
        if (!_isTipeTextComplete)
        {
            StopCoroutine(_tipeText);
            _textResizer.UpdateSize(_storyLine.SlideData[slideIndex].Text);
            _textArea.text = _storyLine.SlideData[slideIndex].Text;
            _isTipeTextComplete = true;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public async UniTaskVoid ActivateInputDelay()
    {
        IsInputActive = false;
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        IsInputActive = true;
    }


    private void OnDestroy()
    {
        _masterSave.SaveAllData();
        _input.Player.Attack.started -= SwipeStory;
    }

    public void ChangeStoryLineIfCan(SlideButtonsData buttonData)
    {
        if (buttonData.IsCircleChoise)
        {
            IsCirleChoise = true;
            buttonData.WasChoised = true;
            _slideIndex = buttonData.NextSlideKey;
            ShowSlide(_slideIndex);
            IsMainFlowActive = true;

        }
        else
        {
            _slideIndex = buttonData.NextSlideKey;
            ShowSlide(_slideIndex);
            IsMainFlowActive = true;
        }
    }
}

