using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class LoadingBar : MonoBehaviour
{
    public GameObject globalUiObject;
    private float timeToLoad; 
    
    string sceneToChangeTo;
    void Start()
    {
        var globalUiState = globalUiObject.GetComponent<StaticStateManager>();
        sceneToChangeTo = globalUiState.getSceneToChangeTo();
        timeToLoad = globalUiState.getTimeToLoad();

        StartCoroutine(LoadScene(timeToLoad));
    }

    private IEnumerator LoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
