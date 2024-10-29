using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MetaFade(0.005f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator MetaFade(float waitTime)
    {
        Image image = GetComponent<Image>();
        if (image.color.a == 1) { 
            yield return new WaitForSeconds(waitTime);
        }  
        
        if (image.color.a < 0)
        {
            yield return null;
        }
        else { 
            StartCoroutine(Fade(waitTime));
        }

        yield return null;
    }

    private IEnumerator Fade(float waitTime)
    {
        Image image = GetComponent<Image>();
        var alpha = image.color.a;
        image.color = new Color(0, 0, 0, alpha - 0.01f);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MetaFade(waitTime));
    }
}
