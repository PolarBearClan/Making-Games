using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
public class StoryClue : Collectible
{
    [Space]
    [SerializeField] private TMPro.TMP_Text noteText;
    [TextArea]
    [SerializeField] private string storyText;
    
    public delegate void StoryCluePickup();
    public StoryCluePickup OnStoryCluePickup;
    public EventReference soundToPlayOnPickUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        if (noteText != null) noteText.text = storyText;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void Interact()
    {
        PlayOnInteract();
        if (OnStoryCluePickup != null) OnStoryCluePickup();
        playerController.ScreenNoteManagerScript.ShowNote(storyText);
        
        playerController.DisableInput();
        firstPersonController.DisableInput();

        shouldEnableInput = false;
        base.Interact();
    }

    public void PlayOnInteract() { 
    
        
        EventInstance soundOnPickUp = RuntimeManager.CreateInstance(soundToPlayOnPickUp);
        RuntimeManager.AttachInstanceToGameObject(soundOnPickUp, transform);
        soundOnPickUp.start();
        soundOnPickUp.release();
    
    }

    public override void Activate()
    {
        //transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void Deactivate()
    {
        //transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
    }
    
    public override string GetActionName()
    {
        return "pick up";
    }
}
