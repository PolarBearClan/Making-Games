using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
public class SingleUseSoundTrigger : MonoBehaviour
{

    public EventReference soundToPlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider c) { 
    
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlay);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
        Destroy(this);
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
