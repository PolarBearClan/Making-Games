using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIFollower : MonoBehaviour
{
    [Tooltip("How close does the follower need to get to trigger Game Over.")]
    [SerializeField] private float catchDistance;
    [Tooltip("Running speed after the player is revealed.")]
    [SerializeField] private float runningSpeed;


    [Space]
    [Header("Sacrificial circle variables")]
    [Tooltip("Index of starting waypoint in the list of all waypoints.")]
    [SerializeField] private int startingIndex = 0;
    [SerializeField] private bool startingInner = true;

    private PathingManager pathingManager;
    private NavMeshAgent navMeshAgent;
    private GameObject playerGameObject;
    private NoiseManager noiseManager;

    private bool foundPlayer = false;
    private EActivity activity;
    private int currPoint = 0;
    private int currCircleCount = 0;
    private bool innerCircle = true;
    private int changeAfter = -1;
    private float findNextPointDistance = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathingManager = GameObject.FindGameObjectWithTag("CirclePathingManager").GetComponent<PathingManager>();
        changeAfter = pathingManager.ChangeCirclesAfterPoints;
        
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        currPoint = startingIndex;
        innerCircle = startingInner;
        if (!innerCircle) changeAfter--;
        
        noiseManager = GameObject.FindGameObjectWithTag("NoiseManager").GetComponent<NoiseManager>();
        noiseManager.OnAlertNPCs += RevealPlayer;
        
        // get next point
        GoToNextPoint();
        activity = EActivity.WALKING;
    }

    // Update is called once per frame
    void Update()
    {
        if (foundPlayer)
        {
            // follow him no matter what
            navMeshAgent.destination = playerGameObject.transform.position;
            navMeshAgent.isStopped = false;
        }
        else
        {
            if (navMeshAgent.remainingDistance <= findNextPointDistance)
            {
                GoToNextPoint();
            }
        }
        
        if (IsPlayerCloseEnough())
        {
            // Game Over
            
            // TODO game over screen
            print("Game Over!");
            
            enabled = false;
        }
    }

    private bool IsPlayerCloseEnough() => Vector3.Distance(transform.position, playerGameObject.transform.position) <= catchDistance;

    private void RevealPlayer()
    {
        foundPlayer = true;
        
        navMeshAgent.speed = runningSpeed;
        activity = EActivity.RUNNING;
        
        // TODO change animation to running
    }  

    private void GoToNextPoint()
    {
        // get next point
        navMeshAgent.destination = pathingManager.GetPoint(currPoint++, innerCircle).position;

        if (++currCircleCount % changeAfter == 0)
        {
            currCircleCount = 0;
            innerCircle = !innerCircle;
            
            if (innerCircle) changeAfter++;
            else changeAfter--;
        }
        
        navMeshAgent.isStopped = false;
    }
    
    public EActivity Activity => activity;
}