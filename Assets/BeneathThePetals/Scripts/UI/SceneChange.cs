using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using FMOD;
using FMODUnity;
using FMOD.Studio;
public class SceneChange : MonoBehaviour, IInteractable
{
    [Header("Scene variables")]
    [SerializeField] private string sceneToChangeTo;
    [SerializeField] private string objectToSpawnAt;
    [SerializeField] private float timeToLoad = 1f;
    [SerializeField] private float fadeInSpeed = 1f;
    [SerializeField] private string actionName;
    [SerializeField] private GameObject fadeToBlack;
    [SerializeField] private GameObject globalUiObject;

    [Header("Lock/Unlock")]
    [SerializeField] private UnlockRequirementType unlockRequirement;
    [SerializeField] private StoryClue requiredStoryClue;
    [SerializeField] private int notificationDuration;
    [SerializeField] [TextArea] private string notificationText;
    private bool canUse;

    public EventReference eventToPlayAtSceneChange;
    private Animator anim;
    private PauseMenu pauseMenu;
    private PlayerController playerController;

    private void Start()
    {
        canUse = unlockRequirement == UnlockRequirementType.NoRequirement;
        pauseMenu = FindAnyObjectByType<PauseMenu>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (unlockRequirement == UnlockRequirementType.ItemRequired && requiredStoryClue)
            requiredStoryClue.OnStoryCluePickup += () => { canUse = true; };
    }

    public void ChangeScene()
    {
        var globalUiState = globalUiObject.GetComponent<StaticStateManager>();
        globalUiState.setSceneToChangeTo(sceneToChangeTo);
        globalUiState.setObjectToSpawnAt(objectToSpawnAt);
        globalUiState.setTimeToLoad(timeToLoad);

        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(MetaFade(0.01f));
    }

    public void Interact()
    {
        if (canUse)
        {
            //use
            this.gameObject.layer = 13; // disabling collider (I guess ?)
            
            PlayInteractSound();
            GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isCurrentlyChangingScenes =
                true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeAll;
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().SetTrigger("OpenDoors");
            ChangeScene();
            pauseMenu.SetPause(true);
        }
        else
        {
            // show notification
            playerController.ScreenNoteManagerScript.ShowNoteNotification(notificationText, notificationDuration);
        }
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
    
    public string GetActionType()
    {
        return "Press";
    }

    private IEnumerator MetaFade(float waitTime)
    {

        if (fadeToBlack.GetComponent<Image>().color.a >= 1) {
            yield return new WaitForSeconds(fadeInSpeed);
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
            var globalUiState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StaticStateManager>();

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

    public void OnQuestCompleted()
    {
        canUse = true;
    }
    
    public UnlockRequirementType UnlockRequirement => unlockRequirement;
}

public enum UnlockRequirementType
{
    NoRequirement,
    ItemRequired,
    QuestCompletionRequired
}
