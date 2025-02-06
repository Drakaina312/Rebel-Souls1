using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

public class PhotoCamera : MonoBehaviour
{
    [Header("Настройки захвата")]
    public Camera cameraToCapture; 
    public int captureWidth = 1920;
    public int captureHeight = 1080;


    [Button]
    public Sprite CaptureAndSave(string filePath)
    {

        cameraToCapture.clearFlags = CameraClearFlags.SolidColor;
        cameraToCapture.backgroundColor = new Color(0, 0, 0, 0);


        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24, RenderTextureFormat.ARGB32);
        cameraToCapture.targetTexture = rt;

        Texture2D capturedTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);

        cameraToCapture.Render();
        RenderTexture.active = rt;

        capturedTexture.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        capturedTexture.Apply();

        cameraToCapture.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);



        Sprite capturedSprite = Sprite.Create(capturedTexture, new Rect(0, 0, capturedTexture.width, capturedTexture.height), new Vector2(0.5f, 0.5f),100,0,SpriteMeshType.FullRect,Vector4.zero,true);



       byte[] pngData = capturedTexture.EncodeToPNG();
        Debug.Log($"Изображение сохранено по пути: {filePath}");
        File.WriteAllBytes(filePath, pngData);
        return capturedSprite;
        SaveSpriteAsPNG(capturedSprite, Application.dataPath, "TestSprite");

    }

    public void SaveSpriteAsPNG(Sprite spriteToSave, string folderPath, string fileName)
    {
        if (spriteToSave == null)
        {
            Debug.LogWarning("Sprite не назначен!");
            return;
        }
        Texture2D texture = spriteToSave.texture;
        if (texture == null)
        {
            Debug.LogError("Не удалось получить текстуру из спрайта.");
            return;
        }

        //// Если спрайт имеет обрезанный прямоугольник (Rect) от исходной текстуры,
        //// и вам нужна именно эта часть, можно извлечь нужные пиксели:
        //Rect spriteRect = spriteToSave.rect;
        //Texture2D spriteTexture = new Texture2D((int)spriteRect.width, (int)spriteRect.height, texture.format, false);
        //Color[] pixels = texture.GetPixels(
        //    (int)spriteRect.x,
        //    (int)spriteRect.y,
        //    (int)spriteRect.width,
        //    (int)spriteRect.height
        //);
        //spriteTexture.SetPixels(pixels);
        //spriteTexture.Apply();

        // Кодируем текстуру в PNG

        Debug.Log("FFFFFFFFFFF");
        byte[] pngData = texture.EncodeToPNG();

        if (pngData != null)
        {
            string filePath = Path.Combine(folderPath, fileName + ".png");
            File.WriteAllBytes(filePath, pngData);
            Debug.Log("Спрайт сохранён: " + filePath);
        }
        else
        {
            Debug.LogError("Не удалось получить PNG данные из текстуры!");
        }
    }

}
