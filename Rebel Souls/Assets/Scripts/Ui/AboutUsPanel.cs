using DG.Tweening;
using UnityEngine;

public class AboutUsPanel : MonoBehaviour
{
    [SerializeField] private Transform _aboutUsPanelToOpen;
    public void OpenPanel()
    {
        _aboutUsPanelToOpen.DOScale(1, 0.3f);

    }

    public void ClosePanel()
    {
        _aboutUsPanelToOpen.DOScale(0, 0.3f);

    }


}
