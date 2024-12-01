using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoverInfoHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionComponent;
    [SerializeField] private Image _image;


    public void ChangeLoverPanel(string loverDiscription, Sprite loverImage)
    {
        _image.sprite = loverImage;
        _descriptionComponent.text = loverDiscription;
    }
}
