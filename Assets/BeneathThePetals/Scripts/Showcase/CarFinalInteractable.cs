using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CarFinalInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private NoiseManager noiseManager;

    [SerializeField] protected string itemName;
    [SerializeField] protected string actionName;
    [SerializeField] protected EventReference soundToPlayOnInteract;

    void Start()
    {
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public virtual string GetName() => itemName;
    public virtual string GetActionName() => actionName;
    public string GetActionType()
    {
        return " ";
    }

    public void Interact()
    {
        noiseManager.TriggerNPCs();
        Debug.Log("NPCs alerted by car");
    }

    public void PlayInteractSound()
    {
        if (!soundToPlayOnInteract.Equals(null))
        {
            EventInstance sound = RuntimeManager.CreateInstance(soundToPlayOnInteract);
            RuntimeManager.AttachInstanceToGameObject(sound, transform);
            sound.start();
            sound.release();
        }
    }
}
