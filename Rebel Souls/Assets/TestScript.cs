using Sirenix.OdinInspector;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [Button]
   public void GetScrinWithAndHight()
    {
        Debug.Log($"Dpi = {Screen.dpi}");
        Debug.Log($"W = {Screen.width}  H = {Screen.height} ");
        Debug.Log($"Resolutuin ${Screen.currentResolution}");



    }
}
