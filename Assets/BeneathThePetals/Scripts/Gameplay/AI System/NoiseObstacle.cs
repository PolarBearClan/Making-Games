using System;
using Unity.Collections;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;


public class NoiseObstacle : MonoBehaviour
{
    private NoiseManager noiseManager;
    public EventReference branchSound;
    public int loudness = 10; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noiseManager = transform.parent.GetComponent<NoiseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BatchOverlapSphere()
    {
        var commands = new NativeArray<OverlapSphereCommand>(1, Allocator.TempJob);
        var results = new NativeArray<ColliderHit>(1000, Allocator.TempJob);

        commands[0] = new OverlapSphereCommand(transform.position, 15f, QueryParameters.Default);

        OverlapSphereCommand.ScheduleBatch(commands, results, 1, 1000).Complete();

        foreach (var hit in results)
        {
            
            print(hit.collider.gameObject.name);
               
            if (hit.collider.gameObject.GetComponentInChildren<AIFollower>() == true)
            {
                hit.collider.gameObject.GetComponentInChildren<AIFollower>() .IncreaseLocalNoise(loudness*15);
            }
        }

        commands.Dispose();
        results.Dispose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // TODO make sound of branch braking
        PlayInteractSound();
        GetComponent<BoxCollider>().enabled = false; 
        noiseManager.IncreaseGlobalNoiseObstacle();
        
        Destroy(gameObject);
        BatchOverlapSphere();

    }
    
    public void PlayInteractSound()
    {
            EventInstance sound = RuntimeManager.CreateInstance(branchSound);
            RuntimeManager.AttachInstanceToGameObject(sound, transform);
            sound.start();
            sound.release();
        
    }
}
