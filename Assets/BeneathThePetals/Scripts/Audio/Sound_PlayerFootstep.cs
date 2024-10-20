using FMODUnity;
using UnityEngine;

public class Sound_PlayerFootstep : MonoBehaviour
{
    public GameObject footStepEmitter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {

            var emitterComponent = footStepEmitter.GetComponent<StudioEventEmitter>();
            emitterComponent.Play();
        
        }  
    }
}
