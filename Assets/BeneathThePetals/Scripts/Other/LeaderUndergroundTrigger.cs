using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LeaderUndergroundTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private HidingInteractable hidingInteractable;

    private PlayerController playerController;
    private FirstPersonController playerControls;
    private Animator anim;

    private bool dialogueHasStarted = false;

    private bool isRotating = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerControls = playerController.transform.GetComponent<FirstPersonController>();
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
            director.Play();
            dialogueHasStarted = false;
        }

        /*
        if(!hidingInteractable.isHiding)
        {
            director.Stop();
            StartCoroutine(StartGameOver());
        }
        */
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
        RotateTowardsDestination(playerController.transform);
        anim.SetBool("isWalking", true);
        yield return new WaitForSeconds(1f);
        playerControls.DisableInput();
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();

        transform.DOMove(playerController.transform.position, 10f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(7f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hidingInteractable.isHiding)
        {
            director.Stop();
            StartCoroutine(StartGameOver());
        }
    }
}
