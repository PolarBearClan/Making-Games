using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCBaseController : MonoBehaviour, ITalkable
{
    public string npcName;
    public Transform pointToFace;

    [Space] 
    public List<DialogueNode> dialogue;

    private GameObject player;
    private FirstPersonController FPSScript;
    private PlayerController PlayerController;

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

        PlayerController.DisableInput();

        GameObject dialogueGo = PlayerController.dialogueBox;
        dialogueGo.GetComponent<DialogueSystem>().DialogueEndCallback = EndDialogue;
        dialogueGo.GetComponent<DialogueSystem>().PlayDialogue(dialogue);


        // Few things:
        /*
           1) Disable controls: movement + camera rotation
               movement -> ez in FPS controller
              camera rotation -> ez in FPS controller
              TODO IMPORTANT -> we need to rotate the player himself as well
                       -> issue with 'pitch'
                           -> when dialogue ends the camera pops back to the original pitch
                                -> resulting in camera snap
                                    -> we need to calculate the pitch for this and then set it in the FPS
                                        -> or 'zatmíva?ka'

           2) Rotate the player to face me (NPC)
               -> EZ: playerCamera.transform.lookAt

           3) Start processing the dialogue
               -> put the strings into UI elements
                   -> UI elements of the player probably?
                   -> we need reference for the player then
                       -> just find by tag

         */
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
}