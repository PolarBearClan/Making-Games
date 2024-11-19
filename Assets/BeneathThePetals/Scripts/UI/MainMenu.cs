using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator transition;

    public void LoadScene(string name)
    {
        StartCoroutine(StartScene(name));
    }

    IEnumerator StartScene(string name)
    {
        transition.transform.gameObject.SetActive(true);
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
