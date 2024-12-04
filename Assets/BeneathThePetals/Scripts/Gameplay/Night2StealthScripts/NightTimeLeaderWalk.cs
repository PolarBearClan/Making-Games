using DG.Tweening;
using System.Drawing;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class NightTimeLeaderWalk : MonoBehaviour
{
    [SerializeField] Transform[] walkPoints;
    [SerializeField] private float[] lerpDurations;

    private PlayerController playerController;
    public Animator anim;

    private bool dialogueHasStarted = false;
    public bool canWalk = false;

    private int currentPointIndex = 0;
    private float timeElapsed = 0f;
    private Vector3 startPosition;
    private bool isRotating = false;

    public GameObject killbox0;
    public GameObject killbox1;
    public GameObject killbox2;

    public NPCBaseController AI; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        anim.SetBool("isWalking", true);
        dialogueHasStarted = true;
        AI.Activity = EActivity.WALKING;

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
            RotateTowardsDestination(walkPoints[currentPointIndex]);
        }
        if (canWalk && currentPointIndex < walkPoints.Length)
        {
            WalkToNextPoint();
        }

        if (currentPointIndex >= 1 && currentPointIndex < 5)
        {
            killbox0.gameObject.SetActive(false);
            killbox1.gameObject.SetActive(true);
            killbox2.gameObject.SetActive(false);
        }
        else 
        if (currentPointIndex >= 5 && currentPointIndex < 20) {
            killbox0.gameObject.SetActive(false);
            killbox1.gameObject.SetActive(false);
            killbox2.gameObject.SetActive(true);

        }
        else 
        if (currentPointIndex >= 20 && currentPointIndex < 25) {
            killbox0.gameObject.SetActive(false);
            killbox1.gameObject.SetActive(true);
            killbox2.gameObject.SetActive(false);

        }
        else 
        if (currentPointIndex >= 25 && currentPointIndex < 40) {
            killbox0.gameObject.SetActive(true);
            killbox1.gameObject.SetActive(false);
            killbox2.gameObject.SetActive(false);

        }
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
                currentPointIndex = 0;
            }
        }
    }
    
    public void RotateTowardsDestination(Transform point)
    {
        Vector3 direction = (point.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.DORotateQuaternion(targetRotation, 1f);
    }
}
