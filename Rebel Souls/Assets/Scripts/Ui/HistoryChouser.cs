using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoryChouser : MonoBehaviour
{
    [SerializeField] private HistoryData _dataToLoad;
    [SerializeField] private PanelHandler _panelHandler;
    [SerializeField] private TextMeshProUGUI _historyDisc;
    [SerializeField] private Image _historyBackground;
    [SerializeField] private Button _startHistory;
    public void OpenHistory()
    { 
        _panelHandler.OpenPanel();
        _historyDisc.text = _dataToLoad.HistoryDisc;
        _historyBackground.sprite = _dataToLoad.Background;
        _startHistory.onClick.RemoveAllListeners();
        _startHistory.onClick.AddListener(()=> OpenNewScene(_dataToLoad.NumberScene));

    }

    private void OpenNewScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
