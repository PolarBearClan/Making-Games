using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
public class AISound : MonoBehaviour
{
    public NPCBaseController AI;
    public EventReference soundToPlayVoice;
    private EventReference soundToPlayCarpet;
    private EventReference soundToPlayWoodInside;
    private EventReference soundToPlayDirt;
    private EventReference soundToPlayWoodOutside;
    private EventReference soundToPlayStairs;
    private EventReference soundToPlayGrass;


    public AISoundEnum terrain;

    private float noiseTime;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundToPlayCarpet = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/CarpetFootstep3D");
        soundToPlayDirt = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/DirtFootstep3D");
        soundToPlayWoodInside = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/WoodFootstep3D");
        soundToPlayWoodOutside = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/WoodOutside3D");
        soundToPlayStairs = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/StairFootstep3D");
        soundToPlayGrass = FMODUnity.RuntimeManager.PathToEventReference("event:/World/NPCWalk/GrassFootstep3D");
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log(noiseTime);
        UnityEngine.Debug.Log(AI.Activity);
        if (noiseTime >= 0.61 && AI.Activity == EActivity.WALKING)
        {
            PlayFootSound();
            noiseTime = 0;
        }
        else if (noiseTime >= Random.Range(3,6) && AI.Activity == EActivity.IDLE)
        {
           PlayVoiceSound();
           noiseTime = 0;
        }

        noiseTime += Time.deltaTime;
        
    }

    public void PlayFootSound()
    {
        switch (terrain)
        {
            case AISoundEnum.WoodInside:
                PlayWoodInsideSound();
                break;
            case AISoundEnum.Carpet:
                PlayCarpetSound();
                break;
            case AISoundEnum.Dirt:
                PlayDirtSound();
                break;
            case AISoundEnum.Grass:
                PlayGrassSound();
                break;
            case AISoundEnum.WoodOutside:
                PlayWoodOutsideSound();
                break;
            case AISoundEnum.Stairs:
                PlayStairSound();
                break;
            
        } 
        
    }

    public void PlayVoiceSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayVoice);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
        
    public void PlayCarpetSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayCarpet);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
        
    public void PlayWoodInsideSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayWoodInside);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayStairSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayStairs);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayDirtSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayDirt);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayWoodOutsideSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayWoodOutside);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayGrassSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(soundToPlayGrass);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }

}