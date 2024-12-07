using DG.Tweening;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using FMOD;
using UnityEngine;
using UnityEngine.UI;

public class UndergroundJumpscare : Jumpscare
{
    [Header("Jumpscare Settings")]
    [SerializeField] private float durationUntilTalk;

    [Header("Smash Mechanic Settings")]
    [SerializeField] private Image smashBar;
    [SerializeField] private float barDecreaseRate = 0.5f;
    [SerializeField] private float barIncreaseAmount = 0.2f;
    
    private GameObject smashMenu;
    private GameObject player;
    private PlayerController playerController;
    private FirstPersonController firstPersonController;
    private Animator anim;
    private bool isSmashing = false;
    private bool dialogueHasStarted = false;
    private bool triggered = false;
    public EventReference killSounds;
    public EventReference DeathSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smashMenu = smashBar.transform.parent.gameObject;
        SetSmashMenu(false);
        gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        firstPersonController = player.GetComponent<FirstPersonController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.DialogueBox.activeSelf)
        {
            dialogueHasStarted = true;
        }
        if (dialogueHasStarted && !playerController.DialogueBox.activeSelf && !triggered)
        {
            if(InventoryManager.Instance.inventoryItems.Contains("Knife"))
                StartSmashMechanic();
            else
            {
                StartCoroutine(GameOverTransition());
                
            }
            dialogueHasStarted = false;
        }

        if (isSmashing)
        {
            smashBar.fillAmount -= barDecreaseRate * Time.deltaTime;

            if (smashBar.fillAmount <= 0)
            {
                StartCoroutine(GameOverTransition());
                isSmashing = false;
                SetSmashMenu(false);
            }

            
            if (Input.GetKeyDown(KeyCode.E))
            {
                smashBar.fillAmount += barIncreaseAmount;

                if (smashBar.fillAmount >= 1)
                {
                    isSmashing = false;
                    smashBar.fillAmount = 1;
                    StartCoroutine(StartKillTransition());
                }
            }
            

        }
    }

    public override void Scare()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        gameObject.SetActive(true);
        StartCoroutine(StartJumpscare());
    }
    private IEnumerator StartJumpscare()
    {
        RotateTowardsDestination(player.transform);
        transform.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(durationUntilTalk);
        transform.GetComponent<NPCBaseController>().Interact();
    }
    private void StartSmashMechanic()
    {
        smashBar.fillAmount = 0.5f;
        SetSmashMenu(true);
        firstPersonController.DisableInput();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        triggered = true;
        isSmashing = true;
    }
    private void RotateTowardsDestination(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.DORotateQuaternion(targetRotation, 1f);
    }

    private void SetSmashMenu(bool setBool)
    {
        foreach (Transform child in smashMenu.transform)
        {
            child.gameObject.SetActive(setBool);
        }
    }

    private IEnumerator StartKillTransition()
    {
        SetSmashMenu(false);

        playerController.GetComponentInChildren<PauseMenu>().SetKillTransition(true);
        yield return new WaitForSeconds(4f);
        playKillSound();
        yield return new WaitForSeconds(2f);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        playerController.GetComponentInChildren<PauseMenu>().SetKillTransition(false);
        anim.SetTrigger("Dead");
        playDeathSound();

        float moveDuration = 2f;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutCubic);

        firstPersonController.EnableInput();

    }
    
    private IEnumerator GameOverTransition()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
        firstPersonController.DisableInput();
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
    private void playDeathSound()
    {
        EventInstance  soundOnDeath = RuntimeManager.CreateInstance(DeathSound);
        RuntimeManager.AttachInstanceToGameObject(soundOnDeath, transform);
        soundOnDeath.start();
        soundOnDeath.release();
    }
    
}
