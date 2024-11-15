using System;
using UnityEngine;

public class NoiseObstacle : MonoBehaviour
{
    private NoiseManager noiseManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noiseManager = transform.parent.GetComponent<NoiseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // TODO make sound of branch braking
        
        noiseManager.IncreaseNoise();
        Destroy(gameObject);
    }
}
