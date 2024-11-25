using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class HistoryChouser : MonoBehaviour
{
    
    [SerializeField] private ActsChoosingData _actToChoose;
    [SerializeField] private PanelHandler _panelHandler;
    [SerializeField] private TextMeshProUGUI _historyDisc;
    [SerializeField] private Image _historyBackground;
    [SerializeField] private Button _startHistory;
    private InGameDataBase _dataBase;
    private MasterSave _masterSave;
    [SerializeField] private ActsChanger _actsChanger;
    [Inject]
    private void Construct(InGameDataBase gameData, MasterSave testSave)
    { 
        _dataBase = gameData; 
        _masterSave = testSave;
        testSave.LoadAllData();
        
    }    
    public void OpenHistory()
    { 
        
        _panelHandler.OpenPanel();
        _actsChanger.ChangePanel(_actToChoose.ActsInfo);
        //_historyDisc.text = _dataToLoad.HistoryDisc;
        _historyBackground.sprite = _actToChoose.Background;
        _startHistory.onClick.RemoveAllListeners();
        _startHistory.onClick.AddListener(()=> OpenNewScene(_actToChoose.NumberScene));
        //_masterSave.CurrentProfile.AddStatistic(_dataActs.MassiveStatistics);
        _masterSave.SaveAllData();

    }

    private void OpenNewScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
