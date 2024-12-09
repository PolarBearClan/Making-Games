
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
public class VisionDetection : MonoBehaviour
{
    [SerializeField] private AIFollower AI;
    private GameObject player;
    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!(AI.personalNoiseLevel >= 110) && !player.GetComponent<FirstPersonController>().isHiding && AI.lookingForPlayer) { 
            AI.IncreaseLocalNoise();      
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!(AI.personalNoiseLevel >= 110) && !player.GetComponent<FirstPersonController>().isHiding && AI.lookingForPlayer) {
            Debug.Log(AI.personalNoiseLevel);
            AI.IncreaseLocalNoise();      
        }
    }
}
