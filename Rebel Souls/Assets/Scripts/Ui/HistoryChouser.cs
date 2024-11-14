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
    [Inject]
    private void Construct(InGameDataBase gameData)
    { 
      _dataBase = gameData; 
    }    
    public void OpenHistory()
    { 
        _panelHandler.OpenPanel();
        _historyDisc.text = _dataToLoad.HistoryDisc;
        _historyBackground.sprite = _dataToLoad.Background;
        _dataBase.HistoryPattern = _historyPattern;
        _startHistory.onClick.RemoveAllListeners();
        _startHistory.onClick.AddListener(()=> OpenNewScene(_dataToLoad.NumberScene));

    }

    private void OpenNewScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
