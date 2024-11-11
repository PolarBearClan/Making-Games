using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float runningSpeed;

    [Space]
    [Header("Sacrificial place variables")]
    public PathingManager pathingManager;
    public int startingIdx = 0;
    public bool startingInner = true;

    private NavMeshAgent navMeshAgent;
    private GameObject playerGO;
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
        changeAfter = pathingManager.changeCirclesAfterPoints;
        
        playerGO = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        currPoint = startingIdx;
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
            navMeshAgent.destination = playerGO.transform.position;
            navMeshAgent.isStopped = false;

            if (IsPlayerCloseEnough())
            {
                // Game Over
                
                // TODO game over screen
                
                enabled = false;
            }
        }
        else
        {
            if (navMeshAgent.remainingDistance <= findNextPointDistance)
            {
                GoToNextPoint();
            }
        }
    }

    private bool IsPlayerCloseEnough() => Vector3.Distance(transform.position, playerGO.transform.position) <= maxDistance;

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
