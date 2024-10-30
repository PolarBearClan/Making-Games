using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneChange : MonoBehaviour, IInteractable
{
    public string sceneToChangeTo;
    public string actionName;
    public GameObject fadeToBlack;
    public GameObject globalUiObject;

    public void ChangeScene() 
    {
        var globalUiState = globalUiObject.GetComponent<GlobalUIState>();
        globalUiState.setSceneToChangeTo(sceneToChangeTo);

        StartCoroutine(MetaFade(0.01f));
    }

    public void Interact() 
    {
        ChangeScene();
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return " ";
    }

    public string GetActionName()
    {
        return actionName;
    }

    private IEnumerator MetaFade(float waitTime)
    {

        if (fadeToBlack.GetComponent<Image>().color.a >= 1) {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("LoadingScreen");
        }

        StartCoroutine(Fade(waitTime));
        yield return null;
    }

    private IEnumerator Fade(float waitTime)
    {
        Image image = fadeToBlack.GetComponent<Image>();
        var alpha = image.color.a;
        image.color = new Color(0, 0, 0, alpha + 0.01f);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MetaFade(waitTime));
    }
}
