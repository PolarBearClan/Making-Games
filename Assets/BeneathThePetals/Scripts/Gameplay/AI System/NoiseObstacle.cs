using System;
using Unity.Collections;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;


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
        var results = new NativeArray<ColliderHit>(100, Allocator.TempJob);

        commands[0] = new OverlapSphereCommand(transform.position, 10f, QueryParameters.Default);

        OverlapSphereCommand.ScheduleBatch(commands, results, 1, 100).Complete();

        foreach (var hit in results)
        {

            AIFollower ai;
               
            if (hit.collider.gameObject.TryGetComponent<AIFollower>(out ai))
            {
                ai.IncreaseLocalNoise(loudness);
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
