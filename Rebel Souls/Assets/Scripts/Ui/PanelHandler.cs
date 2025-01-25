using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PanelHandler : MonoBehaviour
{
    [SerializeField] private Transform _panelToOpen;
    [SerializeField] private UnityEvent _onOpen;
    [SerializeField] private UnityEvent _onClose;

    public void OpenPanel()
    {
        if (_panelToOpen != null)
            _panelToOpen.DOScale(1, 0.3f);
        _onOpen?.Invoke();
    }

    public void ClosePanel()
    {
        if (_panelToOpen != null)
            _panelToOpen.DOScale(0, 0.3f);
        _onClose?.Invoke();
    }


}
