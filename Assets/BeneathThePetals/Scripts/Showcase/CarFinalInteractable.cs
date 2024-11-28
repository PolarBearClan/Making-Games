using DG.Tweening;
using UnityEngine;

public class CarFinalInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private NoiseManager noiseManager;

    [SerializeField] private BoxCollider carCollider;
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public string GetActionName()
    {
        return "Start";
    }

    public string GetName()
    {
        return "Car";
    }

    public void Interact()
    {
        transform.DOComplete();

        noiseManager.TriggerNPCs();
        Debug.Log("NPCs alerted by car");
    }

    public void PlayInteractSound()
    {
    }
}
