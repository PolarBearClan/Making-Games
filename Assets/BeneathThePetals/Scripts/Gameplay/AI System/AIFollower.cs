using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    private GameObject playerGO;
    
    private NavMeshAgent navMeshAgent;
    private bool foundPlayer = false;
       
    private NoiseObstaclesManager noiseObstaclesManager;
    public PathingManager pathingManager;

    public int startingIdx = 0;
    private int currPoint = 0;
    private int currCircleCount = 0;
    private bool innerCircle = true;
    public bool startingInner = true;
    private int changeAfter = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changeAfter = pathingManager.changeCirclesAfterPoints;
        
        playerGO = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        currPoint = startingIdx;
        innerCircle = startingInner;
        if (!innerCircle) changeAfter--;
        
        // get next point
        GoToNextPoint();

        //noiseObstaclesManager = GameObject.FindGameObjectWithTag("NoiseManager").GetComponent<NoiseObstaclesManager>();
        //noiseObstaclesManager.OnNoiseMade += RevealPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            GoToNextPoint();
        }
        
        
        /*
        if (foundPlayer)
        {
            // follow him no matter what
            navMeshAgent.destination = playerGO.transform.position;
            navMeshAgent.isStopped = false;
        }
        else
        {
            if (IsPlayerCloseEnough())
            {
                foundPlayer = true;
                noiseObstaclesManager.OnNoiseMade();
                print("Target acquired");
            }
        }
        */
    }

    private bool IsPlayerCloseEnough() => Vector3.Distance(transform.position, playerGO.transform.position) <= maxDistance;
    
    public void RevealPlayer() => foundPlayer = true;

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
}
