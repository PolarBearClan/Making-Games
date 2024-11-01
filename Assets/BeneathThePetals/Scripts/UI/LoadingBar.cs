using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class LoadingBar : MonoBehaviour
{
    public GameObject globalUiObject;
    string sceneToChangeTo;
    void Start()
    {
        var globalUiState = globalUiObject.GetComponent<GlobalUIState>();
        sceneToChangeTo = globalUiState.getSceneToChangeTo();


        StartCoroutine(MetaProgress(0.2f));
    }
    private IEnumerator MetaProgress(float waitTime)
    {

        if (transform.localScale.x >= 10) {

            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(sceneToChangeTo);
        }

        if (transform.localScale.x == 1) {
            yield return new WaitForSeconds(2);
        }

        StartCoroutine(Progress(Random.Range(0.05f, 0.3f)));
        yield return null;
    }

    private IEnumerator Progress(float waitTime)
    {
        if (transform.localScale.x <= 10) { 
            var randomProgress = Random.Range(1, 3);
            transform.localScale = new Vector3(transform.localScale.x+randomProgress, 0.5f, 1);
        }
        Debug.Log(waitTime);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MetaProgress(waitTime));
    }
}
