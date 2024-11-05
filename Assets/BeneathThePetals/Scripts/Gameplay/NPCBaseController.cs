using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class NPCBaseController : MonoBehaviour, ITalkable
{
    [SerializeField] private string npcName;
    [SerializeField] private Transform pointToFace;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;
    
    [Space]
    [Header("Quest related variables")]
    [SerializeField] private Quest quest;
    [Space]
    [SerializeField] private List<DialogueNode> dialogueAfterQuestAssigned;
    

    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;

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

    public void Interact()
    {
        if (mainDialogue.Count > 0)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        firstPersonController.DisableInput();

        float tweenDuration = playerController.CameraLookAtTweenDuration;
        
        // Important to rotate them both
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);
        firstPersonController.transform.DOLookAt(pointToFace.position, tweenDuration);
        
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
        
        firstPersonController.EnableInput(true);
        playerController.EnableInput();
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
}
