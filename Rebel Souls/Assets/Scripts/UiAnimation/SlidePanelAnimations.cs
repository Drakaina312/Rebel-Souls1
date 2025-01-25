using DG.Tweening;
using UnityEngine;

public class SlidePanelAnimations : MonoBehaviour
{
    [SerializeField] private RectTransform _mainProfile;
    [SerializeField] private SlideHandler _slideHandler;

    Tween tween;

    private bool _isClose = true;
    private bool _isBtnDiactivated;
    public void OpenMaimPanel()
    {
        if (_isBtnDiactivated)
            return;

        if (_isClose)
        {
            _slideHandler.BlockMainFlow();
            _isClose = false;
            tween = _mainProfile.DOAnchorPos(Vector2.zero, 0.5f).SetAutoKill(false);
        }
        else
        {
            _isClose = true;
            tween.PlayBackwards();
            _isBtnDiactivated = true;
            tween.OnRewind(() =>
            {
                _slideHandler.ActivateMainFlow();
                _isBtnDiactivated = false;
            });

        }
    }

    public void HideMainPanel()
    {
        tween.PlayBackwards();
        _isBtnDiactivated = true;
    }

    public void RefreshMainPanel()
    {
        tween = _mainProfile.DOAnchorPos(Vector2.zero, 0.5f).SetAutoKill(false);
        _isBtnDiactivated = false;

    }


}
