using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCam : MonoBehaviour
{
    // Start is called before the first frame update

    Camera waterCam;
    Texture2D t2d;

    public SpriteRenderer sr;

    void Start()
    {
        
        waterCam = GetComponent<Camera>();

        t2d = new Texture2D(waterCam.targetTexture.width, waterCam.targetTexture.height, TextureFormat.ARGB32, false);

        StartCoroutine(CamRenderer());

        Debug.Log(waterCam);
    }


    WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

    IEnumerator CamRenderer()
    {
        while (true)
        {
            yield return waitForEnd;

            RenderTexture.active = waterCam.targetTexture;

            waterCam.Render();

            t2d.ReadPixels(new Rect(0, 0, waterCam.targetTexture.width, waterCam.targetTexture.height), 0, 0);
            t2d.Apply();

            sr.sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(.5f, .5f), t2d.width);
        }
    }
}
