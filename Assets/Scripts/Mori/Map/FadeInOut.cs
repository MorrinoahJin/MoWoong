using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;

public class FadeInOut : MonoBehaviour
{
    public Image blackImage;
    float fadeSpeed = 1f, time = 0;



    public void StartFadeInOut()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        blackImage.gameObject.SetActive(true);
        Color tempColor = blackImage.color;

        time = 0;
        while (tempColor.a < 1f)
        {
            time += Time.deltaTime / fadeSpeed;
            tempColor.a = Mathf.Lerp(0, 1, time);
            blackImage.color = tempColor;
            yield return null;
        }
        time = 0;

        yield return new WaitForSeconds(1f);

        while(tempColor.a > 0)
        {
            time += Time.deltaTime / fadeSpeed;
            tempColor.a = Mathf.Lerp(1, 0, time);
            blackImage.color = tempColor;
            yield return null;
        }

        blackImage.gameObject.SetActive(false);
        yield return null;
    }
}
