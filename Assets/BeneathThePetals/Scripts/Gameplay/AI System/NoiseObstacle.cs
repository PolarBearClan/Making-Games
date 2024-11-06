using System;
using UnityEngine;

public class NoiseObstacle : MonoBehaviour
{
    private NoiseObstaclesManager noiseObstaclesManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noiseObstaclesManager = transform.parent.GetComponent<NoiseObstaclesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // TODO make sound
        // sound.Play() ?
        
        // send signal to NPC to follow you
        noiseObstaclesManager.OnNoiseMade();
        Destroy(gameObject);
    }
}
