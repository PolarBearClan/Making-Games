using PSX;
using UnityEngine;
using UnityEngine.Rendering;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class NoiseManager : MonoBehaviour
{
    public delegate void AlertNPCs();
    public AlertNPCs OnAlertNPCs;

    [SerializeField] private float noiseStep = 20f;
    [SerializeField] private float decayRate = 10f;
    [SerializeField, Range(0, 100)] private float currentGlobalNoiseLevel = 0f;

    public EventReference noiseEvent;
    private EventInstance noiseSound;
    private float maxValue = 100f;
    private float minValue = 0f;

    // In the Inspector, assign a Render Pipeline Asset to each of these fields, taken from documentation
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        noiseSound = RuntimeManager.CreateInstance(noiseEvent);
        RuntimeManager.AttachInstanceToGameObject(noiseSound, transform);
        noiseSound.setVolume(0);
        noiseSound.start();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease noise level over time, but don't go below the minimum value
        if (!(currentGlobalNoiseLevel >= 99)) { 
        currentGlobalNoiseLevel -= decayRate * Time.deltaTime;
        currentGlobalNoiseLevel = Mathf.Max(currentGlobalNoiseLevel, minValue); // Clamp to minValue
        }

        noiseSound.setVolume(currentGlobalNoiseLevel / 100);
    }

    // This will be called by the branch being broken
    public void IncreaseGlobalNoise()
    {
        currentGlobalNoiseLevel += noiseStep;
        if (currentGlobalNoiseLevel > maxValue)
        {
            // Threshold reached -> alert NPCs
            OnAlertNPCs();
            enabled = false;

            return;
        }
        currentGlobalNoiseLevel = Mathf.Min(currentGlobalNoiseLevel, maxValue); // Clamp to maxValue

        // TODO update UI overlay based on the noise level
    }

    public void IncreaseGlobalNoiseObstacle()
    {
        currentGlobalNoiseLevel += noiseStep * 10;
        if (currentGlobalNoiseLevel > maxValue)
        {
            // Threshold reached -> alert NPCs
            OnAlertNPCs();
            enabled = false;

            return;
        }
        currentGlobalNoiseLevel = Mathf.Min(currentGlobalNoiseLevel, maxValue); // Clamp to maxValue

        // TODO update UI overlay based on the noise level
    }
    public float CurrentGlobalNoiseLevel => currentGlobalNoiseLevel;

    public void TriggerNPCs()
    {
        currentGlobalNoiseLevel = (maxValue + 1);
        IncreaseGlobalNoise();
    }
}
