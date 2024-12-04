using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstDayQuest : MonoBehaviour, ITalkable
{
    [SerializeField] private string npcName;
    [SerializeField] private Transform pointToFace;
    [SerializeField] private bool facePlayerOnInteraction = true;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;

    [Space]
    [Header("Quest related variables")]
    [SerializeField] private Quest quest;
    [Space]
    [SerializeField] private List<DialogueNode> dialogueAfterQuestAssigned;

    [Header("Other")]
    [SerializeField] private GameObject fadeToBlack;
    [SerializeField] private GameObject clothes;

    private List<SceneChange> sceneChangers;    // Scene changers requiring quest completion

    private EActivity activity;
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    private Animator anim;
    private NPCWalking npcWalking;
    private Collider colliderObj;
    private NPCBaseController npcBaseController;

    public bool undergroundScareConvo = false;

    private Quaternion defaultRotation;
    public EventReference soundToPlayOnInteract;

    private bool dialogueStarted = false;
    private bool hasTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
        activity = EActivity.IDLE;
        anim = GetComponent<Animator>();
        npcWalking = GetComponent<NPCWalking>();
        colliderObj = GetComponent<Collider>();
        colliderObj.enabled = false;
        npcBaseController = GetComponent<NPCBaseController>();

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
        if(playerController.DialogueBox.activeSelf && !hasTriggered)
        {
            dialogueStarted = true;
        }
        if(dialogueStarted && !playerController.DialogueBox.activeSelf && !hasTriggered)
        {
            hasTriggered = true;
            EndDialogue();
            StartCoroutine(MetaFade());
        }
    }

    public void PlayInteractSound()
    {


        EventInstance soundOnInteract = RuntimeManager.CreateInstance(soundToPlayOnInteract);
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

        if (npcWalking != null)
            npcWalking.canWalk = false;
        defaultRotation = transform.rotation;

        activity = EActivity.TALKING;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        firstPersonController.DisableInput();

        if (facePlayerOnInteraction)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion npcTargetRotation = Quaternion.LookRotation(directionToPlayer);

            float tweenDuration = playerController.CameraLookAtTweenDuration;
            transform.DORotateQuaternion(npcTargetRotation, tweenDuration);
        }

        if (anim != null)
            anim.SetBool("isTalking", true);

        playerController.DisableInput();
        Invoke("LookAtNPC", .5f);

        var dialogueBox = playerController.DialogueBox;
        var dialogueSystem = dialogueBox.GetComponent<DialogueSystem>();
        dialogueSystem.DialogueEndCallback = EndDialogue;
        dialogueSystem.QuestFromDialogueCallback = AssignQuest;
        dialogueSystem.PlayDialogue(mainDialogue);

        var animator = dialogueBox.GetComponent<Animator>();
        if (animator.transform.gameObject.activeSelf)
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

    private IEnumerator MetaFade()
    {
        Cursor.visible = false;
        firstPersonController.DisableInput();

        yield return StartCoroutine(Fade(0f, 1f, 1f));

        yield return new WaitForSeconds(1f);
        clothes.SetActive(false);
        anim.SetTrigger("Clothes");

        yield return StartCoroutine(Fade(1f, 0f, 1f));

        firstPersonController.EnableInput(true);

        npcBaseController.enabled = true;
        npcBaseController.Interact();
        transform.GetComponent<FirstDayQuest>().enabled = false;
        colliderObj.enabled = true;
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        Image image = fadeToBlack.GetComponent<Image>();
        Color color = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }
}
