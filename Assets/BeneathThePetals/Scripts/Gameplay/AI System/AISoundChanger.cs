using System;
using UnityEngine;

public class AISoundChanger : MonoBehaviour
{
    public AISoundEnum TerrainOnEnter;
    public AISoundEnum TerrainOnLeave;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<AISound>(out AISound component))
        {
            component.terrain = TerrainOnEnter;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.TryGetComponent<AISound>(out AISound component))
        {
            component.terrain = TerrainOnLeave;
        }
    }
}
