using DG.Tweening;
using System.Drawing;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class LeaderIntroWalk : MonoBehaviour
{
    [SerializeField] Transform[] walkPoints;
    [SerializeField] private float[] lerpDurations;

    [SerializeField] private Animator gatesAnim;
    [SerializeField] private EventReference gateEvent;
    [SerializeField] private EventInstance gateSound;

    private PlayerController playerController;
    private Animator anim;

    private bool dialogueHasStarted = false;
    private bool canWalk = false;

    private int currentPointIndex = 0;
    private float timeElapsed = 0f;
    private Vector3 startPosition;
    private bool isRotating = false;
    bool gateSoundPlayed = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        gateSound = RuntimeManager.CreateInstance(gateEvent); 
        RuntimeManager.AttachInstanceToGameObject(gateSound, transform);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.DialogueBox.activeSelf)
        {
            dialogueHasStarted = true;
        }
        if(dialogueHasStarted && !playerController.DialogueBox.activeSelf)
        {
            canWalk = true;
            if (gatesAnim != null)
            {
                gatesAnim.SetTrigger("Open");

                if (!gateSoundPlayed) {
                    PlayGateSound();
                    gateSoundPlayed = true;
                }

            }
            RotateTowardsDestination(walkPoints[currentPointIndex]);
        }
        if (canWalk && currentPointIndex < walkPoints.Length)
        {
            WalkToNextPoint();
            transform.GetComponent<NPCBaseController>().enabled = false;
        }
    }

    void PlayGateSound() { 
    
                gateSound.start();
                gateSound.release();
    
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
}
