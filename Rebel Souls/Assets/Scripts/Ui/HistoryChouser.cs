using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class HistoryChouser : MonoBehaviour
{
    
    [SerializeField] private HistoryData _dataToLoad;
    [SerializeField] private PanelHandler _panelHandler;
    [SerializeField] private TextMeshProUGUI _historyDisc;
    [SerializeField] private Image _historyBackground;
    [SerializeField] private Button _startHistory;
    private InGameDataBase _dataBase;
    [SerializeField] private HistoryPattern _historyPattern;
    private MasterSave _test;
    [Inject]
    private void Construct(InGameDataBase gameData, MasterSave testSave)
    { 
        _dataBase = gameData; 
        _test = testSave;
        testSave.LoadAllData();
        
    }    
    public void OpenHistory()
    { 
        _panelHandler.OpenPanel();
        _historyDisc.text = _dataToLoad.HistoryDisc;
        _historyBackground.sprite = _dataToLoad.Background;
        _dataBase.HistoryPattern = _historyPattern;
        _startHistory.onClick.RemoveAllListeners();
        _startHistory.onClick.AddListener(()=> OpenNewScene(_dataToLoad.NumberScene));
        _test.SaveAllData();

    }

    private void OpenNewScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
