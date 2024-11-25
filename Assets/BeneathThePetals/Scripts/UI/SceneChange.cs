using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using FMOD;
using FMODUnity;
using FMOD.Studio;
public class SceneChange : MonoBehaviour, IInteractable
{
    public string sceneToChangeTo;
    public string objectToSpawnAt;
    public string actionName;
    public GameObject fadeToBlack;
    public GameObject globalUiObject;

    public EventReference eventToPlayAtSceneChange;
    private Animator anim;
    private PauseMenu pauseMenu;

    private void Start()
    {
        pauseMenu = FindAnyObjectByType<PauseMenu>();
    }

    public void ChangeScene()
    {
        var globalUiState = globalUiObject.GetComponent<GlobalUIState>();
        globalUiState.setSceneToChangeTo(sceneToChangeTo);
        globalUiState.setObjectToSpawnAt(objectToSpawnAt);

        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(MetaFade(0.01f));
    }

    public void Interact()
    {
        PlayInteractSound();
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().SetTrigger("OpenDoors");
        ChangeScene();
        pauseMenu.SetPause(true);
    }

    public void Activate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
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
            yield return new WaitForSeconds(5f);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "LoadingScreen")
        {
            GameObject _player = GameObject.FindGameObjectWithTag("Player");
            var globalUiState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GlobalUIState>();

            if (GameObject.Find(globalUiState.getObjectToSpawnAt()) != null)
            {
                _player.transform.position = GameObject.Find(globalUiState.getObjectToSpawnAt()).transform.position;
                _player.transform.rotation = GameObject.Find(globalUiState.getObjectToSpawnAt()).transform.rotation;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Object to spawn at not found: " + globalUiState.getObjectToSpawnAt());
            }
        }

        UnityEngine.Debug.Log("OnSceneLoaded: " + scene.name);
        UnityEngine.Debug.Log(mode);
    }

    public void PlayInteractSound() {

        EventInstance soundWhenSceneChange = RuntimeManager.CreateInstance(eventToPlayAtSceneChange);
        RuntimeManager.AttachInstanceToGameObject(soundWhenSceneChange, transform);
        soundWhenSceneChange.start();
        soundWhenSceneChange.release();
    }
}
