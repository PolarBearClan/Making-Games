using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine.InputSystem.Editor;

public class NPCBaseController : MonoBehaviour, ITalkable
{
    public string npcName;
    public Transform pointToFace;

    [Space]
    [Header("Quest")]
    public bool givesQuest = false;
    public Quest quest;
    public List<DialogueNode> dialogueAfterQuestAssigned;
    
    [Space] 
    public List<DialogueNode> dialogue;

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
        if (dialogue.Count > 0)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        firstPersonController.DisableInput();

        float tweenDuration = playerController.cameraTweenDuration;
        
        // Important to rotate them both
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);
        firstPersonController.transform.DOLookAt(pointToFace.position, tweenDuration);
        
        playerController.DisableInput();

        var dialogueSystem = playerController.dialogueBox.GetComponent<DialogueSystem>();
        dialogueSystem.DialogueEndCallback = EndDialogue;
        dialogueSystem.QuestFromDialogueCallback = AssignQuest;
        dialogueSystem.PlayDialogue(dialogue);
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return npcName;
    }

    private void EndDialogue()
    {
        firstPersonController.EnableInput();
        playerController.EnableInput();
    }

    private void AssignQuest()
    {
        quest.OnQuestFinished = ChangeDialogueAfterQuest;
        playerController.AssignQuest(quest);
        dialogue = dialogueAfterQuestAssigned;
    }

    private void ChangeDialogueAfterQuest(List<DialogueNode> newDialogue)
    {
        dialogue = newDialogue;
    }
}
