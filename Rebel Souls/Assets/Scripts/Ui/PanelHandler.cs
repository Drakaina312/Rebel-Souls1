using UnityEngine;  
using DG.Tweening;

public class PanelHandler : MonoBehaviour
{
    [SerializeField] private Transform _panelToOpen;
    public void OpenPanel()
    {
        _panelToOpen.DOScale(1, 0.3f);

    }

    public void ClosePanel()
    {
        _panelToOpen.DOScale(0, 0.3f);

    }


}
