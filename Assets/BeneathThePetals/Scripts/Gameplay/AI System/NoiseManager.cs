using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public delegate void AlertNPCs();
    public AlertNPCs OnAlertNPCs;
    
    [SerializeField] private float noiseStep = 20f;
    [SerializeField] private float decayRate = 10f;
    [SerializeField, Range(0, 100)] private float currentNoiseLevel = 0f;

    private float maxValue = 100f;
    private float minValue = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease noise level over time, but don't go below the minimum value
        currentNoiseLevel -= decayRate * Time.deltaTime;
        currentNoiseLevel = Mathf.Max(currentNoiseLevel, minValue); // Clamp to minValue
    }
    
    // This will be called by the branch being broken
    public void IncreaseNoise()
    {
        currentNoiseLevel += noiseStep;
        if (currentNoiseLevel > maxValue)
        {
            // Threshold reached -> alert NPCs
            print("INTRUDER ALERT!");
            OnAlertNPCs();
            enabled = false;
            
            return;
        }
        currentNoiseLevel = Mathf.Min(currentNoiseLevel, maxValue); // Clamp to maxValue
    }
    
    public float CurrentNoiseLevel => currentNoiseLevel;
}
