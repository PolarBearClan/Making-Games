using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CultistOutsideRun : MonoBehaviour
{
    [Header("Chasing Settings")]
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private bool isRunning = true;

    [Header("Leader Cultist")]
    [SerializeField] private bool isLeader = false;
    [SerializeField] private float distanceBetween = 10f;
    [SerializeField] private Volume globalVolumeBase;

    private GameObject player;
    private Rigidbody rb;
    private Animator anim;
    private PauseMenu pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseMenu = FindAnyObjectByType<PauseMenu>();
        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            RandomizeAnimation();
        }
        if(!isRunning)
        {
            anim.SetBool("isWalking", false);
        }
        else
            anim.SetBool("isWalking", true);
    }

    void Update()
    {
        if (pauseMenu.isPaused)
            return;

        if (isLeader)
        {
            float sqrDistance = (transform.position - player.transform.position).sqrMagnitude;
            float sqrThreshold = distanceBetween * distanceBetween;
            float targetWeight = (sqrDistance <= sqrThreshold) ? 0.5f : 1f;

            globalVolumeBase.weight = Mathf.MoveTowards(globalVolumeBase.weight, targetWeight, 0.1f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (pauseMenu.isPaused)
            return;

        if (isRunning)
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            rb.MovePosition(transform.position + direction * runSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void RandomizeAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(stateInfo.fullPathHash, -1, Random.Range(0f, 1f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<FirstPersonController>().DisableInput();
            other.GetComponentInChildren<PauseMenu>().StartGameOver();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
