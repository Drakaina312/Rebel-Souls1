using Cysharp.Threading.Tasks;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

    [SerializeField] private TextMeshProUGUI _achievemnetText;
    [SerializeField] private Image _achievemnetImage;


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
        ///Èçìåíèòü åÑËÈ ÍÀ×ÀÒÜ ÇÀÍÎÂÎ ÒÎ ÑÒÈÐÀÅÌ ÏÐÎÉÄÅÍÍÛÅ ÑËÀÉÄÛ ÈÍÀ×Å ÏÐÎÄÎËÆÀÅÌ ÄÎÁÀÂËßÒÜ
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
            if (!_storyLine.SlideData[_slideIndex].IsHaveCheckingÑondition)
                _slideIndex = FindNextSlideToShow(_slideIndex);
            else if (_storyLine.SlideData[_slideIndex].IsHaveCheckingÑondition)
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
            foreach (var chapterSaves in _masterSave.CurrentProfile.BooksStat.Where(x => x.ChapterSortingConditions.BookName == statsBook.ChapterSortingConditions.BookName))
            {
                if (chapterSaves.IsSlideIndexExistInSave(chekingConditions.StatName))
                {
                    return true;
                }
            }
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
                if (!CheckSlidePassing(statToChek, savedStats))
                    return false;
                else
                    isConditionGood = true;
            }
            else if (statToChek.Var1 == false && statToChek.SlideCheck == false && statToChek.IsBigStat == false && statToChek.IsBigFavorite == false)
            {
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
            else if (statToChek.Var1)
            {
                switch (statToChek.Cnd)
                {
                    case ChekingEnums.More:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount > savedStats.FindStat(statToChek.StatName2).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount < savedStats.FindStat(statToChek.StatName2).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount == savedStats.FindStat(statToChek.StatName2).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount >= savedStats.FindStat(statToChek.StatName2).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount <= savedStats.FindStat(statToChek.StatName2).StatisticCount)
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
            else if (statToChek.IsBigStat)
            {
                var statToCompare = savedStats.FindStat(statToChek.StatName, out string statChapter);
                var statToCompareCount = statToCompare.StatisticCount;

                foreach (var item in savedStats.SavedStats[statChapter])
                {
                    if (item.StatisticName == statToCompare.StatisticName)
                        continue;

                    if (!item.IsRelationship)
                        if (statToCompareCount <= item.StatisticCount)
                        {
                            isConditionGood = false;
                            return false;
                        }
                        else
                            isConditionGood = true;
                }
            }
            else if (statToChek.IsBigFavorite)
            {
                var statToCompare = savedStats.FindStat(statToChek.StatName, out string statChapter).StatisticCount;
                foreach (var item in savedStats.SavedStats[statChapter])
                {
                    if (item.IsRelationship)
                        if (statToCompare <= item.StatisticCount)
                        {
                            isConditionGood = false;
                            return false;
                        }
                        else
                            isConditionGood = true;
                }
            }

        }
        Debug.Log("ôÈÍÀËÜÍÀß ïðîâåðêà = " + isConditionGood);
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
            if (!openedSlides.Contains(slideIndex))
                openedSlides.Add(slideIndex);
            _currentSaveStats.SavedIndexes = openedSlides.ToArray();
        }

        ShowHeroyOnScene(_storyLine.SlideData[slideIndex]);

        ShowAchievement(_storyLine.SlideData[slideIndex]);

        if (_storyLine.SlideData[slideIndex].IsHaveButtons && !IsCirleChoise)
            for (int i = 0; i < _storyLine.SlideData[slideIndex].ButtonSetting.Count; i++)
                _storyLine.SlideData[slideIndex].ButtonSetting[i].WasChoised = false;

        if (_buttonsHandler.ActivateButtons(_storyLine.SlideData[slideIndex], this) == false)
            return;

        ActivateAudioEffects(slideIndex);

        if (_storyLine.SlideData[slideIndex].IsHaveNotation)
        {
            if (_storyLine.SlideData[slideIndex].IsTipNotation)
                if (_masterSave.CurrentProfile.DifficultyType == DifficultyType.Easy || _masterSave.CurrentProfile.DifficultyType == DifficultyType.Medium)
                    _notationHandler.ActivaidNotation(_storyLine.SlideData[slideIndex].Notation);
                else
                    _notationHandler.ActivaidNotation(_storyLine.SlideData[slideIndex].Notation);

        }

        if (!_storyLine.SlideData[slideIndex].IsHaveText)
        {
            _textArea.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            _textArea.transform.parent.gameObject.SetActive(true);
        }

        _tipeText = StartCoroutine(TypeText(_storyLine.SlideData[slideIndex].Text));
    }

    private void ShowAchievement(SlideData slideData)
    {
        //Âêëþ÷èòü ïàíåëü à÷èâêè ñ àíèìàöèåé èëè áåç
        _achievemnetText.text = slideData.AchievementText;
        _achievemnetImage.sprite = slideData.AchievemntSprite;

        if (slideData.IsAchievemntGiveGift)
        {
            var favoriteToGivePrisent = _currentSaveStats.FindStat(slideData.FavoriteNameForPrisent);
            favoriteToGivePrisent.AddNewScinForFavoriteScin(slideData.SpritePathToGive);
        }

        //Ñêîëüêî æäàòü 
        // Âûêëþ÷èòü à÷èâêó
    }

    private void ActivateAudioEffects(string slideIndex)
    {
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
    }

    private string FindNextSlideToShow(string currentSlideIndex)
    {
        if (!_storyLine.SlideData[currentSlideIndex].IsHaveCheckingÑondition)
            return _storyLine.SlideData[currentSlideIndex].NextSlideToOpen;
        else
        {
            // Ïðîâåðêà óñëîâèÿ åñëè åñòü
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
                if (!slideData.IsFavorite)
                    _heroLeft.sprite = slideData.HeroSprite;
                else
                {
                    if (slideData.IsImportantScin)
                        _heroLeft.sprite = slideData.HeroSprite;
                    else
                    {
                        var loadedSprite = Resources.Load<Sprite>(_currentSaveStats.FindStat(slideData.FavoriteName).PathToFavoriteScin);
                        if (loadedSprite != null)
                            _heroLeft.sprite = loadedSprite;
                        else
                            _heroLeft.sprite = slideData.HeroSprite;
                    }
                }
                break;
            case HeroType.HeroRight:
                _heroRight.gameObject.SetActive(true);
                if (!slideData.IsFavorite)
                    _heroRight.sprite = slideData.HeroSprite;
                else
                {
                    if (slideData.IsImportantScin)
                        _heroRight.sprite = slideData.HeroSprite;
                    else
                    {
                        var loadedSprite = Resources.Load<Sprite>(_currentSaveStats.FindStat(slideData.FavoriteName).PathToFavoriteScin);
                        if (loadedSprite != null)
                            _heroRight.sprite = loadedSprite;
                        else
                            _heroRight.sprite = slideData.HeroSprite;
                    }
                }
                break;
        }
    }

    private IEnumerator TypeText(string fullText)
    {
        _isTipeTextComplete = false;
        _textArea.text = "";
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

