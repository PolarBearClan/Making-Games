using DG.Tweening;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;

public class CultistInsideRun : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] Transform[] walkPoints;
    [SerializeField] private float[] lerpDurations;
    [SerializeField] private Volume globalVolumeBase;

    private PlayerController playerController;
    private FirstPersonController playerControls;
    private Animator anim;
    private PauseMenu pauseMenu;

    [HideInInspector]
    public bool canWalk = false;

    private int currentPointIndex = 0;
    private float timeElapsed = 0f;
    private Vector3 startPosition;
    private bool isRotating = false;
    public EventReference killSounds;

    private bool coroutineStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerControls = playerController.transform.GetComponent<FirstPersonController>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        pauseMenu = FindAnyObjectByType<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineStarted || pauseMenu.isPaused)
            return;

        if (canWalk && currentPointIndex < walkPoints.Length)
        {
            WalkToNextPoint();
        }

        float sqrDistance = (transform.position - playerController.transform.position).sqrMagnitude;
        float sqrThreshold = distance * distance;
        float targetWeight = (sqrDistance <= sqrThreshold) ? 0.5f : 1f;

        globalVolumeBase.weight = Mathf.MoveTowards(globalVolumeBase.weight, targetWeight, 0.1f * Time.deltaTime);
    }
    private void WalkToNextPoint()
    {
        anim.SetBool("isWalking", true);

        if (!isRotating)
        {
            RotateTowardsDestination(walkPoints[currentPointIndex]);
            isRotating = true;
        }

        Vector3 targetPosition = walkPoints[currentPointIndex].position;
        float currentLerpDuration = lerpDurations[currentPointIndex];

        transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / currentLerpDuration);

        timeElapsed += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            timeElapsed = 0f;
            startPosition = transform.position;
            currentPointIndex++;

            if (currentPointIndex < walkPoints.Length)
            {
                isRotating = false;
            }
            else
            {
                canWalk = false;
                anim.SetBool("isWalking", false);
                RotateTowardsDestination(playerController.gameObject.transform);
            }
        }
    }

    private void RotateTowardsDestination(Transform point)
    {
        Vector3 direction = (point.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.DORotateQuaternion(targetRotation, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerControls.isWalking = false;
            GetComponent<AISoundForCultistRun>().enabled = false;
            StartCoroutine(StartGameOver());
        }
    }

    private IEnumerator StartGameOver()
    {
        coroutineStarted = true;
        playerControls.DisableInput();

        anim.SetBool("isWalking", false);
        RotateTowardsDestination(playerController.transform);
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
        yield return new WaitForSeconds(4f);
        playKillSound();

        yield return new WaitForSeconds(2f);
        anim.SetBool("isWalking", true);
        transform.DOMove(playerController.transform.position, 10f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(7f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void playKillSound()
    {
        EventInstance  soundOnKill = RuntimeManager.CreateInstance(killSounds);
        RuntimeManager.AttachInstanceToGameObject(soundOnKill, transform);
        soundOnKill.start();
        soundOnKill.release();
    }
}
