using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class StealthKillBox : MonoBehaviour
{
    [SerializeField] private Transform pointToFace;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;
    
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    public EventReference soundToPlayOnInteract;
    public NightTimeLeaderWalk leaderLookingToKill;
    public NPCBaseController leader;
    public EventReference killSounds;
    public StoryClueImage objectToDestroy;
    public StoryClueImage objectToDestroy2;
    public StoryClueImage objectToDestroy3;
    private GameObject storyClueUI;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storyClueUI = GameObject.FindGameObjectWithTag("StoryClueUI");
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

    private void OnTriggerEnter(Collider other)
    {

        if (!firstPersonController.isHiding && other.name == "Player") {
            foreach (Transform child in storyClueUI.transform)
            {
               Destroy(child.gameObject);
            }
            
            StartDialogue();
            

        }
    }

    private void StartDialogue()
    {
        Destroy(objectToDestroy);
        Destroy(objectToDestroy2);
        Destroy(objectToDestroy3);
        
        leaderLookingToKill.GetComponentInChildren<BoxCollider>().enabled = false;
        leaderLookingToKill.AI.Activity = EActivity.TALKING;
        leaderLookingToKill.enabled = false;
        firstPersonController.DisableInput();
        PlayInteractSound();
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        leaderLookingToKill.canWalk = false;
        leaderLookingToKill.anim.SetBool("isWalking", false);
        leaderLookingToKill.RotateTowardsDestination(transform);
        float tweenDuration = playerController.CameraLookAtTweenDuration;
        
        // Important to rotate them both
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);
        firstPersonController.isWalking = false;
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
        
        //firstPersonController.EnableInput(true);
        StartCoroutine(StartKillTransition());
        //SceneManager.LoadScene("Day2_Inside_Nighttime");
    }

    private IEnumerator StartKillTransition()
    {
        
        
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
        yield return new WaitForSeconds(4f);
        playKillSound();
        yield return new WaitForSeconds(2f);
        
    }
    
    private void playKillSound()
    {
        EventInstance  soundOnKill = RuntimeManager.CreateInstance(killSounds);
        RuntimeManager.AttachInstanceToGameObject(soundOnKill, transform);
        soundOnKill.start();
        soundOnKill.release();
    }

}
