using UnityEngine;

public class StoryClue : Collectible
{
    [Space]
    [SerializeField] private TMPro.TMP_Text noteText;
    [TextArea]
    [SerializeField] private string storyText;
    
    
    public delegate void StoryCluePickup();
    public StoryCluePickup OnStoryCluePickup;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        noteText.text = storyText;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void Interact()
    {
        if (OnStoryCluePickup != null) OnStoryCluePickup();
        playerController.screenNoteManager.ShowNote(storyText);
        
        playerController.DisableInput();
        firstPersonController.DisableInput();

        shouldEnableInput = false;
        base.Interact();
    }

    public override void Activate()
    {
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void Deactivate()
    {
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
    }
    
    public override string GetActionName()
    {
        return "pick up";
    }
}
