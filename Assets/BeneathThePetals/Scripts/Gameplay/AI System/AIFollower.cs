using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    private GameObject playerGO;
    
    private NavMeshAgent navMeshAgent;
    private bool foundPlayer = false;
       
    private NoiseObstaclesManager noiseObstaclesManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        noiseObstaclesManager = GameObject.FindGameObjectWithTag("NoiseManager").GetComponent<NoiseObstaclesManager>();
        noiseObstaclesManager.OnNoiseMade += RevealPlayer;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private bool IsPlayerCloseEnough() => Vector3.Distance(transform.position, playerGO.transform.position) <= maxDistance;
    
    public void RevealPlayer() => foundPlayer = true;
}
