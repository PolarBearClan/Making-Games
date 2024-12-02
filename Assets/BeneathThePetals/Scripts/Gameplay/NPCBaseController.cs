using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;

    [Space]
    [Header("Quest related variables")]
    [SerializeField] private Quest quest;
    [Space]
    [SerializeField] private List<DialogueNode> dialogueAfterQuestAssigned;

    private List<SceneChange> sceneChangers;    // Scene changers requiring quest completion

    private EActivity activity;
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    private Animator anim;
    private NPCWalking npcWalking;
    public bool undergroundScareConvo = false;

    private Quaternion defaultRotation;
    public EventReference soundToPlayOnInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
        activity = EActivity.IDLE;
        anim = GetComponent<Animator>();
        npcWalking = GetComponent<NPCWalking>();

        if (quest != null)
        {
            sceneChangers = FindObjectsByType<SceneChange>(FindObjectsSortMode.None)
                .Where(sceneChanger => sceneChanger.UnlockRequirement == UnlockRequirementType.QuestCompletionRequired)
                .ToList();
        }
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            firstPersonController.isWalking = false;
        }
    }

    private void StartDialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        firstPersonController.transform.localScale = new Vector3(firstPersonController.originalScale.x, firstPersonController.originalScale.y, firstPersonController.originalScale.z);

        if(npcWalking != null)
            npcWalking.canWalk = false;
        defaultRotation = transform.rotation;

        activity = EActivity.TALKING;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        firstPersonController.DisableInput();

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        Quaternion npcTargetRotation = Quaternion.LookRotation(directionToPlayer);

        float tweenDuration = playerController.CameraLookAtTweenDuration;
        transform.DORotateQuaternion(npcTargetRotation, tweenDuration);

        playerController.DisableInput();
        Invoke("LookAtNPC", .5f);
        
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
        if (!undergroundScareConvo)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;   
        }

        if (anim != null)
            anim.SetBool("isTalking", false);

        if (npcWalking != null)
            npcWalking.canWalk = true;
        float tweenDuration = playerController.CameraLookAtTweenDuration;
        transform.DORotateQuaternion(defaultRotation, tweenDuration);
        
        firstPersonController.EnableInput(true);
        playerController.EnableInput();
        activity = EActivity.IDLE;
    }

    private void AssignQuest()
    {
        quest.OnQuestFinished = QuestComplete;
        playerController.AssignQuest(quest);
        mainDialogue = dialogueAfterQuestAssigned;
    }

    private void QuestComplete(List<DialogueNode> newDialogue)
    {
        mainDialogue = newDialogue;
        
        if (quest.ShouldNotify)
            playerController.ScreenNoteManagerScript.ShowNoteNotification(quest.NotificationText, quest.NotificationDuration);
        
        playerController.ResetInteractionTarget();
        
        // unlock all scene changers that require quest completion
        foreach (var s in sceneChangers)
        {
            s.OnQuestCompleted();
        }
    }

    private void LookAtNPC()
    {
        float tweenDuration = playerController.CameraLookAtTweenDuration;
        firstPersonController.playerCamera.transform.DOLookAt(pointToFace.position, tweenDuration);
        firstPersonController.transform.DOLookAt(pointToFace.position, tweenDuration);
    }
    
    public string GetActionType()
    {
        return "Press";
    }

    public EActivity Activity => activity;
}
