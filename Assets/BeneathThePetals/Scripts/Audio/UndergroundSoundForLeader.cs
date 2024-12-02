using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
public class UndergroundSoundForLeader : MonoBehaviour
{

    public EventReference soundToPlayLeader;
    public EventReference soundToPlayDoorOpen;
    public EventReference soundToPlayDoorClose;
    public EventReference soundToPlayFootstep;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PlayLeaderSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayLeader);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayDoorOpenSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayDoorOpen);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayDoorCloseSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayDoorClose);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    public void PlayFootstepSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayFootstep);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
