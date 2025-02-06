using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TestScript : MonoBehaviour
{
    public Image _img;
    private InGameDataBase _data;

    [Inject] 
    private void Construct(InGameDataBase data)
    {
        _data = data;
    }

    [Button]
    public void GetScrinWithAndHight()
    {
        Debug.Log($"Dpi = {Screen.dpi}");
        Debug.Log($"W = {Screen.width}  H = {Screen.height} ");
        Debug.Log($"Resolutuin ${Screen.currentResolution}");

    }

    [Button]
    public void SetImage()
    {

        _img.sprite = Sprite.Create(Resources.Load<Texture2D>("CapturedImage"), new Rect(0, 0, 700, 1600), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, true);

    }
}
