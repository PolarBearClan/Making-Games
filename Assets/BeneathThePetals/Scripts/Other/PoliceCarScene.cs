using UnityEngine;
using UnityEngine.Playables;

public class PoliceCarScene : MonoBehaviour, IInteractable
{
    [SerializeField] private string actionName;
    [SerializeField] private PlayableDirector director;
    
    private LeaderIntroWalk policeOfficer;
    private FirstPersonController playerControls;
    private Collider carCollider;
    private bool canInteract = false;

    void Start()
    {
        playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        policeOfficer = FindAnyObjectByType<LeaderIntroWalk>();
        carCollider = GetComponent<Collider>();
        carCollider.enabled = false;
    }

    void Update()
    {
        if(!policeOfficer.transform.gameObject.activeSelf || !policeOfficer.isActiveAndEnabled)
        {
            canInteract = true;
            carCollider.enabled = true;
        }
    }

    public void Interact()
    {
        if(canInteract)
            StartCutScene();
    }
    private void StartCutScene()
    {
        director.Play();
        playerControls.DisableInput();
        Cursor.visible = false;

        CultistOutsideRun[] cultists = Object.FindObjectsByType<CultistOutsideRun>(FindObjectsSortMode.None);
        foreach (var cultist in cultists)
        {
            cultist.gameObject.SetActive(false);
        }
    }

    public void Activate()
    {
        //
    }

    public void Deactivate()
    {
        //
    }

    public string GetActionName()
    {
        return actionName;
    }

    public string GetName()
    {
        return " ";
    }

    public void PlayInteractSound()
    {
        //
    }

    public void ShowRestart()
    {
        playerControls.transform.GetComponentInChildren<PauseMenu>().ShowRestartMenu();
    }
    public string GetActionType()
    {
        return "Press";
    }

}
