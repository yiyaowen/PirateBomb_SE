using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchManager : MonoBehaviour
{
    public Image logo;
    public float intervalSecs;

    [Header("Fade In")]
    public float fadeInSecs;
    public float fadeInCount;

    [Header("Fade Out")]
    public float fadeOutSecs;
    public int fadeOutCount;

    private void Start()
    {
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        float singleFadeValue = 1.0f / fadeInCount;
        float fadeIntervalSecs = fadeInSecs / fadeInCount;
        for (int i = 0; i < fadeInCount; ++i)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + singleFadeValue);
            yield return new WaitForSeconds(fadeIntervalSecs);
        }
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 1.0f);
        yield return new WaitForSeconds(intervalSecs);
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        float singleFadeValue = 1.0f / fadeOutCount;
        float fadeIntervalSecs = fadeOutSecs / fadeOutCount;
        for (int i = 0; i < fadeOutCount; ++i)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a - singleFadeValue);
            yield return new WaitForSeconds(fadeIntervalSecs);
        }
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
