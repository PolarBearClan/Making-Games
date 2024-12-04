
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class LookScriptInsideDay1 : MonoBehaviour, ITalkable
{
    [SerializeField] private Transform pointToFace;

    [Space]
    [SerializeField] private List<DialogueNode> mainDialogue;
    
    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;
    public EventReference soundToPlayOnInteract;
    public GameObject clothes;
    public GameObject triggerToEnable;
    public GameObject fadeToBlack;

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

    private void OnTriggerEnter(Collider other)
    {
        Interact();
        firstPersonController.isWalking = false;
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
        StartCoroutine(MetaFade());
    }
    
    public string GetActionType()
    {
        return "Press";
    }
    
    private IEnumerator MetaFade()
    {
        Cursor.visible = false;
        firstPersonController.DisableInput();

        yield return StartCoroutine(Fade(0f, 1f, 1f));

        yield return new WaitForSeconds(1f);
        clothes.SetActive(false);

        yield return StartCoroutine(Fade(1f, 0f, 1f));

        firstPersonController.EnableInput(true);
        triggerToEnable.SetActive(true);
        Destroy(gameObject);

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
