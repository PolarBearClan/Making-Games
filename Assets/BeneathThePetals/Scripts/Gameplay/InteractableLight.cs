using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Light))]
public class InteractableLight : MonoBehaviour
{
    private Light light;
    [SerializeField] private Image billboardImage;
    [SerializeField] private InteractableLightType lightType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();
        LightsOut();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        // Light ON
        switch (lightType)
        {
            case InteractableLightType.Billboard:
                billboardImage.enabled = true;
                break;
            
            case InteractableLightType.Light:
                light.enabled = true;
                break;
            
            case InteractableLightType.Both:
                billboardImage.enabled = true;
                light.enabled = true;
                break;
                
        }
        print("Light ON");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        // Light OFF
        LightsOut();
        print("Light OFF");
    }

    private void LightsOut()
    {
        billboardImage.enabled = false;
        light.enabled = false;
    }
}

enum InteractableLightType
{
    Light,
    Billboard,
    Both
}
