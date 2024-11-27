using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UndergroundJumpscare : Jumpscare
{
    [Header("Jumpscare Settings")]
    [SerializeField] private float durationUntilTalk;

    [Header("Smash Mechanic Settings")]
    [SerializeField] private GameObject smashMenu;
    [SerializeField] private Image smashBar;
    [SerializeField] private float barDecreaseRate = 0.5f;
    [SerializeField] private float barIncreaseAmount = 0.2f;

    private GameObject player;
    private PlayerController playerController;
    private FirstPersonController firstPersonController;
    private Animator anim;

    private bool isSmashing = false;
    private bool dialogueHasStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (dialogueHasStarted && !playerController.DialogueBox.activeSelf)
        {
            if(InventoryManager.Instance.inventoryItems.Contains("Knife"))
                StartSmashMechanic();
            else
            {
                playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
                firstPersonController.DisableInput();
            }
            dialogueHasStarted = false;
        }

        if (isSmashing)
        {
            smashBar.fillAmount -= barDecreaseRate * Time.deltaTime;

            if (smashBar.fillAmount <= 0)
            {
                playerController.GetComponentInChildren<PauseMenu>().StartGameOver();

                isSmashing = false;
                SetSmashMenu(false);
            }

            
            if (Input.GetKeyDown(KeyCode.E))
            {
                smashBar.fillAmount += barIncreaseAmount;

                if (smashBar.fillAmount >= 1)
                {
                    StartCoroutine(StartKillTransition());
                }
            }
            

        }
    }

    public override void Scare()
    {
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
        isSmashing = false;
        SetSmashMenu(false);
        smashBar.fillAmount = 1;

        playerController.GetComponentInChildren<PauseMenu>().StartKillTransition();
        yield return new WaitForSeconds(7f);
        anim.SetTrigger("Dead");

        float moveDuration = 2f;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z);
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(moveDuration);
        firstPersonController.EnableInput();

    }
}
