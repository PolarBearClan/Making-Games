using UnityEngine;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationAngle = 90;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private Ease rotationEase = Ease.OutBounce;
    
    [Space]
    [SerializeField] private bool doorLocked = false;
    [SerializeField] private StoryClue requiredStoryClue;
    
    private bool doorOpen = false;
    private bool interactable = true;
    private BoxCollider doorCollider;
    
    private PlayerController playerController;
    
    public EventReference eventToPlayWhenOpen;
    public EventReference eventToPlayWhenClose;
    public EventReference eventToPlayWhenLocked;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        doorCollider = GetComponent<BoxCollider>();
        
        if (requiredStoryClue) requiredStoryClue.OnStoryCluePickup += () => { doorLocked = false; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (!interactable) return;
        
        if (doorLocked)
        {
            PlayLockedSound();
            playerController.LockedDoorText();
            print("Door is locked");
        }
        else
        {
            transform.DOComplete();
            
            PlayInteractSound();
            var rotationChange = new Vector3(0, rotationAngle, 0);
            if (doorOpen) rotationChange *= -1;
            doorOpen = !doorOpen;
            interactable = false;
            doorCollider.enabled = false;
            
            transform.DORotate(transform.eulerAngles + rotationChange, rotationDuration).OnComplete(() =>
            {
                interactable = true;
                doorCollider.enabled = true;
            }).SetEase(rotationEase);
        }
    }

    public void PlayInteractSound() {

        if (!doorOpen) { 
        
        EventInstance soundWhenOpen = RuntimeManager.CreateInstance(eventToPlayWhenOpen);
        RuntimeManager.AttachInstanceToGameObject(soundWhenOpen, transform);
        soundWhenOpen.start();
        soundWhenOpen.release();
        
        }     
        if (doorOpen) { 
        
        EventInstance soundWhenClose = RuntimeManager.CreateInstance(eventToPlayWhenClose);
        RuntimeManager.AttachInstanceToGameObject(soundWhenClose, transform);
        soundWhenClose.start();
        soundWhenClose.release();
        
        }



    }

    public void PlayLockedSound () { 
    
        EventInstance soundWhenLocked = RuntimeManager.CreateInstance(eventToPlayWhenLocked);
        RuntimeManager.AttachInstanceToGameObject(soundWhenLocked, transform);
        soundWhenLocked.start();
        soundWhenLocked.release();
        
    
    }
    public void Activate()
    {
        //GetComponentInChildren<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        //GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return "door";
    }

    public string GetActionName()
    {
        return doorOpen ? "close" : "open";
    }
}
