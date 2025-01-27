using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Sprite _heroLeftThinking;
    [SerializeField] private Sprite _heroLeftTalking;
    [SerializeField] private Sprite _heroRightThinking;
    [SerializeField] private Sprite _heroRightTalking;
    [SerializeField] private Sprite _defolt;
    [SerializeField] private Image _textImage;
    [SerializeField] private TextMeshProUGUI _NameHeroLeft;
    [SerializeField] private TextMeshProUGUI _NameHeroRight;
    [SerializeField] private TMP_InputField _textField;



    private WaitForSeconds _sleepTime;
    private InGameDataBase _gameData;
    private InputSystem_Actions _input;
    private MasterSave _masterSave;
    private bool _isTipeTextComplete;
    private int _slideIndex;
    private Coroutine _tipeText;
    private Coroutine _mainFlowCoroutine;
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
        input.Player.Attack.started += DetectPlayerClick;
    }

    private void DetectPlayerClick(InputAction.CallbackContext context)
    {
        StartCoroutine(SwipeStory());
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
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
            _slideIndex = _storyLine.SlideDataList.FirstOrDefault().SlideIndex;
            _currentSaveStats.SavedIndexes = null;
        }
        _masterSave.SaveAllData();
        IsMainFlowActive = true;
        ShowSlide(_slideIndex);
    }

    public void BlockMainFlow() => IsMainFlowActive = false;
    public void ActivateMainFlow()
    {
        IsMainFlowActive = true;
        Debug.Log("Main Flow Active");
    }


    public void CalculateSlideWork()
    {
        if (_isTipeTextComplete)
        {
            if (!_storyLine.SlideDataList[_slideIndex].IsHaveCheckingÑondition)
                _slideIndex = FindNextSlideToShow(_slideIndex);
            else if (_storyLine.SlideDataList[_slideIndex].IsHaveCheckingÑondition)
            {
                bool isConditionGood = false;

                foreach (var allStats in _storyLine.SlideDataList[_slideIndex].ChekingConditions)
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
        if (chekingConditions.ConditionEnums == ConditionsEnums.CheckSlideIndexPassing)
        {
            foreach (var chapterSaves in _masterSave.CurrentProfile.BooksStat.Where(x => x.ChapterSortingConditions.BookName == statsBook.ChapterSortingConditions.BookName))
            {
                if (chapterSaves.IsSlideIndexExistInSave(chekingConditions.IndexToCheck))
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

            if (statToChek.ConditionEnums == ConditionsEnums.CheckSlideIndexPassing)
            {
                if (!CheckSlidePassing(statToChek, savedStats))
                    return false;
                else
                    isConditionGood = true;
            }
            else if (statToChek.ConditionEnums == ConditionsEnums.CheckStatWithValue)
            {
                switch (statToChek.Cnd)
                {
                    case ChekingEnums.More:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount > statToChek.CheckingValue)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount < statToChek.CheckingValue)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount == statToChek.CheckingValue)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount >= statToChek.CheckingValue)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount <= statToChek.CheckingValue)
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
            else if (statToChek.ConditionEnums == ConditionsEnums.CheckStatWithAnotherStat)
            {
                switch (statToChek.Cnd)
                {
                    case ChekingEnums.More:
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount > savedStats.FindStat(statToChek.ChekingStatName).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount < savedStats.FindStat(statToChek.ChekingStatName).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount == savedStats.FindStat(statToChek.ChekingStatName).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount >= savedStats.FindStat(statToChek.ChekingStatName).StatisticCount)
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
                        if (savedStats.FindStat(statToChek.StatName).StatisticCount <= savedStats.FindStat(statToChek.ChekingStatName).StatisticCount)
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
            else if (statToChek.ConditionEnums == ConditionsEnums.CheckMoreBigStat)
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
            else if (statToChek.ConditionEnums == ConditionsEnums.CheckMoreBigFavorite)
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

    public void ShowSlide(int slideIndex)
    {
        _backGround.sprite = _storyLine.SlideDataList[slideIndex].Background;

        if (_storyLine.SlideDataList[slideIndex].IsTextWritingSlide)
        {
            IsMainFlowActive = false;
            _textField.gameObject.SetActive(true);
        }

        SavePassedSlideToProfile(slideIndex);

        ShowHeroyOnScene(_storyLine.SlideDataList[slideIndex]);

        //ShowAchievement(_storyLine.SlideData[slideIndex]);

        if (_storyLine.SlideDataList[slideIndex].IsHaveButtons && !IsCirleChoise)
            for (int i = 0; i < _storyLine.SlideDataList[slideIndex].ButtonSetting.Count; i++)
                _storyLine.SlideDataList[slideIndex].ButtonSetting[i].WasChoised = false;

        if (_buttonsHandler.ActivateButtons(_storyLine.SlideDataList[slideIndex], this) == false)
            return;

        ActivateAudioEffects(slideIndex);

        ActivateNotation(slideIndex);

        TypeTextIfCan(slideIndex);
    }

    private void SavePassedSlideToProfile(int slideIndex)
    {
        List<int> openedSlides = _currentSaveStats.SavedIndexes?.ToList();
        if (openedSlides == null)
        {
            openedSlides = new List<int>() { slideIndex };
            _currentSaveStats.SavedIndexes = openedSlides.ToArray();
        }
        else
        {
            if (!openedSlides.Contains(slideIndex))
                openedSlides.Add(slideIndex);
            _currentSaveStats.SavedIndexes = openedSlides.ToArray();
        }
    }

    private void TypeTextIfCan(int slideIndex)
    {
        if (!_storyLine.SlideDataList[slideIndex].IsHaveText)
        {
            _textArea.transform.parent.gameObject.SetActive(false);
            _isTipeTextComplete = true;

            return;
        }
        else
        {
            _textArea.transform.parent.gameObject.SetActive(true);
            _tipeText = StartCoroutine(TypeText(_storyLine.SlideDataList[slideIndex].Text));
        }

    }

    private void ActivateNotation(int slideIndex)
    {
        _notationHandler.ActivaidNotation(_masterSave.CurrentProfile.DifficultyType, _storyLine.SlideDataList[slideIndex].IsHaveSystemNotation, _storyLine.SlideDataList[slideIndex].IsHaveAuthorNotation,
            _storyLine.SlideDataList[slideIndex].SystemNotation, _storyLine.SlideDataList[slideIndex].AuthorNotation);
    }

    public void GetWritedText(string text)
    {
        if (_storyLine.SlideDataList[_slideIndex].IsTextWritingSlide && _storyLine.SlideDataList[_slideIndex].MainHeroTextBox)
            _currentSaveStats.MainHeroName = text;

        if (_storyLine.SlideDataList[_slideIndex].IsTextWritingSlide && _storyLine.SlideDataList[_slideIndex].FunTextBox)
            _currentSaveStats.FunTextBox = text;

        if (_storyLine.SlideDataList[_slideIndex].IsTextWritingSlide && _storyLine.SlideDataList[_slideIndex].HorrorTextBox)
            _currentSaveStats.HorrorTextBox = text;

        if (_storyLine.SlideDataList[_slideIndex].IsTextWritingSlide && _storyLine.SlideDataList[_slideIndex].GameTextBox)
            _currentSaveStats.GameTextBox = text;

        _textField.gameObject.SetActive(false);
        IsMainFlowActive = true;

        _masterSave.SaveAllData();
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

    private void ActivateAudioEffects(int slideIndex)
    {
        if (_storyLine.SlideDataList[slideIndex].VoiceClip != null)
        {
            _audioSourceForVoices.clip = _storyLine.SlideDataList[slideIndex].VoiceClip;
            _audioSourceForVoices.Play();
        }
        else
        {
            _audioSourceForVoices.Stop();
        }

        if (_storyLine.SlideDataList[slideIndex].AudioEffectsClip != null)
        {
            _audioSourceForEffects.clip = _storyLine.SlideDataList[slideIndex].AudioEffectsClip;
            _audioSourceForEffects.Play();
        }
        else
        {
            _audioSourceForEffects.Stop();
        }
    }

    private int FindNextSlideToShow(int currentSlideIndex)
    {
        if (!_storyLine.SlideDataList[currentSlideIndex].IsHaveCheckingÑondition)
            return _storyLine.SlideDataList[currentSlideIndex].NextSlideToOpen;
        else
        {
            // Ïðîâåðêà óñëîâèÿ åñëè åñòü
            throw new System.Exception("ÍÅ íàø¸ë íóæíûé ñëàéä äëÿ îòêðûòèÿ");
        }
    }

    private IEnumerator SwipeStory()
    {
        yield return new WaitForSeconds(0.2f);

        if (IsMainFlowActive)
            CalculateSlideWork();
    }

    private void ShowHeroyOnScene(SlideData slideData)
    {
        _heroLeft.gameObject.SetActive(false);
        _heroRight.gameObject.SetActive(false);





        switch (slideData.HeroType)
        {
            case HeroType.HeroLeft:
                _NameHeroRight.gameObject.SetActive(false);
                _NameHeroLeft.gameObject.SetActive(true);
                _NameHeroLeft.text = slideData.InterpritationName;
                if (slideData.IsThinking)
                {
                    _textImage.sprite = _heroLeftThinking;
                }
                else _textImage.sprite = _heroLeftTalking;

                _heroLeft.gameObject.SetActive(true);

                if (slideData.IsMainHero)
                    _heroLeft.sprite = Resources.Load<Sprite>(_currentSaveStats.MainHeroSpritePath);

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
                _NameHeroLeft.gameObject.SetActive(false);
                _NameHeroRight.gameObject.SetActive(true);
                _NameHeroRight.text = slideData.InterpritationName;

                if (slideData.IsThinking)
                {
                    _textImage.sprite = _heroRightThinking;
                }
                else _textImage.sprite = _heroRightTalking;
                _heroRight.gameObject.SetActive(true);

                if (slideData.IsMainHero)
                    _heroRight.sprite = Resources.Load<Sprite>(_currentSaveStats.MainHeroSpritePath);

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

            case HeroType.NoHero:
                _NameHeroRight.gameObject.SetActive(false);
                _NameHeroLeft.gameObject.SetActive(false);
                _textImage.sprite = _defolt;
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
            if (fullText[i] == '%')
            {
                _textArea.text += $" {_currentSaveStats.MainHeroName} ";
            }
            else
                _textArea.text += fullText[i];


            yield return _sleepTime;
        }

        _isTipeTextComplete = true;
    }

    private IEnumerator TipeFullText(int slideIndex)
    {

        if (_storyLine.SlideDataList[slideIndex].IsHaveText)
            if (!_isTipeTextComplete)
            {
                StopCoroutine(_tipeText);
                _textResizer.UpdateSize(_storyLine.SlideDataList[slideIndex].Text);
                _textArea.text = _storyLine.SlideDataList[slideIndex].Text;
                _isTipeTextComplete = true;
                yield return new WaitForSeconds(0.01f);
            }
            else
                _isTipeTextComplete = true;

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
        _input.Player.Attack.started -= DetectPlayerClick;
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

