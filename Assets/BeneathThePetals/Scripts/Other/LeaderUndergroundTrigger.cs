using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Playables;

public class LeaderUndergroundTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private UndergroundCloset hidingInteractable;

    private PlayerController playerController;
    private FirstPersonController playerControls;
    private Animator anim;
    private JumpscareTrigger undergroundJumpscare;

    private bool dialogueHasStarted = false;
    private bool coroutineStarted = false;
    private bool isRotating = false;
    private bool isLooking = false;
    public EventReference killSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerControls = playerController.transform.GetComponent<FirstPersonController>();
        anim = GetComponent<Animator>();
        undergroundJumpscare = FindAnyObjectByType<JumpscareTrigger>();

        if(undergroundJumpscare != null)
            undergroundJumpscare.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.DialogueBox.activeSelf)
        {
            dialogueHasStarted = true;
        }
        if (dialogueHasStarted && !playerController.DialogueBox.activeSelf)
        {
            director.Play();
            dialogueHasStarted = false;
        }

        
        if(!hidingInteractable.isHiding && !coroutineStarted && isLooking)
        {
            director.Stop();
            StartCoroutine(StartGameOver());
        }
    }
    private void RotateTowardsDestination(Transform point)
    {
        if (isRotating) return;
        isRotating = true;

        Vector3 direction = (point.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.DORotateQuaternion(targetRotation, 1f).OnComplete(() => isRotating = false);
    }

    private IEnumerator StartGameOver()
    {
        coroutineStarted = true;
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezeAll;

        RotateTowardsDestination(playerController.transform);
        anim.SetBool("isWalking", true);
        yield return new WaitForSeconds(1f);
        playerControls.DisableInput();
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
        yield return new WaitForSeconds(4f);
        playKillSound();

        transform.DOMove(playerController.transform.position, 10f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(7f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartLooking(bool setLooking)
    {
        isLooking = setLooking;
    }
    
    private void playKillSound()
    {
        EventInstance  soundOnKill = RuntimeManager.CreateInstance(killSounds);
        RuntimeManager.AttachInstanceToGameObject(soundOnKill, transform);
        soundOnKill.start();
        soundOnKill.release();
    }

    public void EnableUndergroundJumpscare()
    {
        if (transform.GetComponent<LeaderUndergroundTrigger>() != null)
            transform.GetComponent<LeaderUndergroundTrigger>().enabled = false;

        if(undergroundJumpscare != null)
            undergroundJumpscare.gameObject.SetActive(true);
    }
}
