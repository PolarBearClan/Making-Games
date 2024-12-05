using DG.Tweening;
using UnityEngine;

public class NPCWalking : MonoBehaviour
{
    [SerializeField] Transform[] walkPoints;
    [SerializeField] private float lerpDuration;

    [HideInInspector]
    public bool canWalk = true;

    private Animator anim;

    private int currentPointIndex = 0;
    private float timeElapsed = 0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRotating = false;
    public NPCBaseController AI;

    void Start()
    {
        anim = GetComponent<Animator>();
        AI.Activity = EActivity.WALKING;
        startPosition = transform.position;
        targetPosition = walkPoints[currentPointIndex].position;
        canWalk = true;
    }

    void Update()
    {
        if (canWalk)
        {
            WalkToNextPoint();
        }
    }
    private void WalkToNextPoint()
    {
        //anim.SetBool("isWalking", true);

        if (!isRotating)
        {
            RotateTowardsDestination(walkPoints[currentPointIndex]);
            isRotating = true;
        }

        transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            timeElapsed = 0f;
            startPosition = transform.position;

            currentPointIndex = (currentPointIndex + 1) % walkPoints.Length;
            targetPosition = walkPoints[currentPointIndex].position;

            if (currentPointIndex < walkPoints.Length)
            {
                isRotating = false;
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
