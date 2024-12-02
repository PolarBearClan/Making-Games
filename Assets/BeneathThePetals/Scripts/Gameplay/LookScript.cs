
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using FMOD;
using FMOD.Studio;
using FMODUnity; 
public class LookScript : MonoBehaviour, ITalkable
{
    [SerializeField] private Transform pointToFace;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;
    
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    public EventReference soundToPlayOnInteract; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayInteractSound() {
    
    
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayOnInteract);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    
    }
    public void Interact()
    {
        if (mainDialogue.Count > 0)
        {
            PlayInteractSound();
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        firstPersonController.DisableInput();

        float tweenDuration = playerController.CameraLookAtTweenDuration;
        firstPersonController.transform.localScale = new Vector3(firstPersonController.originalScale.x, firstPersonController.originalScale.y, firstPersonController.originalScale.z);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        
        // Important to rotate them both
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);
        firstPersonController.transform.DOLookAt(pointToFace.position, tweenDuration);
        
        playerController.DisableInput();

        var dialogueBox = playerController.DialogueBox;
        var dialogueSystem = dialogueBox.GetComponent<DialogueSystem>();
        dialogueSystem.DialogueEndCallback = EndDialogue;
        dialogueSystem.PlayDialogue(mainDialogue);

        var animator = dialogueBox.GetComponent<Animator>();
        if (animator.gameObject.activeSelf)
            animator.SetBool("DialogueBars", true);
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
        return "";
    }

    private void EndDialogue()
    {
        // Finish tweens in case of a quick dialogue ending
        firstPersonController.playerCamera.transform.DOComplete();
        firstPersonController.transform.DOComplete();
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        firstPersonController.EnableInput(true);
        playerController.EnableInput();
    }
    
    public string GetActionType()
    {
        return "Press";
    }
}
