using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using FMOD;
using FMOD.Studio;
using FMODUnity; 
public class NPCBaseController : MonoBehaviour, ITalkable
{
    [SerializeField] private string npcName;
    [SerializeField] private Transform pointToFace;
    [SerializeField] private Transform targetPosition;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;
    
    [Space]
    [Header("Quest related variables")]
    [SerializeField] private Quest quest;
    [Space]
    [SerializeField] private List<DialogueNode> dialogueAfterQuestAssigned;

    private EActivity activity;
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    private Vector3 initialCameraPosition;
    public EventReference soundToPlayOnInteract; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
        activity = EActivity.IDLE;
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
        activity = EActivity.TALKING;
        firstPersonController.DisableInput();

        float tweenDuration = playerController.CameraLookAtTweenDuration;

        initialCameraPosition = firstPersonController.playerCamera.transform.position;


        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        Quaternion npcTargetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.DORotateQuaternion(npcTargetRotation, tweenDuration);

        //firstPersonController.playerCamera.transform.DOMove(targetPosition.position, tweenDuration);

        // Important to rotate them both
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);

        Vector3 directionToNPC = (pointToFace.position - firstPersonController.transform.position).normalized;
        directionToNPC.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNPC);
        firstPersonController.transform.DORotateQuaternion(targetRotation, tweenDuration);

        playerController.DisableInput();

        var dialogueBox = playerController.DialogueBox;
        var dialogueSystem = dialogueBox.GetComponent<DialogueSystem>();
        dialogueSystem.DialogueEndCallback = EndDialogue;
        dialogueSystem.QuestFromDialogueCallback = AssignQuest;
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
        return npcName;
    }

    private void EndDialogue()
    {
        // Finish tweens in case of a quick dialogue ending
        firstPersonController.playerCamera.transform.DOComplete();
        firstPersonController.transform.DOComplete();

        float tweenDuration = playerController.CameraLookAtTweenDuration;
        //firstPersonController.playerCamera.transform.DOMove(initialCameraPosition, tweenDuration);

        firstPersonController.EnableInput(true);
        playerController.EnableInput();
        activity = EActivity.IDLE;
    }

    private void AssignQuest()
    {
        quest.OnQuestFinished = ChangeDialogueAfterQuest;
        playerController.AssignQuest(quest);
        mainDialogue = dialogueAfterQuestAssigned;
    }

    private void ChangeDialogueAfterQuest(List<DialogueNode> newDialogue)
    {
        mainDialogue = newDialogue;
    }
    
    public EActivity Activity => activity;
}
