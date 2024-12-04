using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;

public class QuestDeliveryLocation : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string actionName;
    [SerializeField] private Light light;

    [Space]
    [SerializeField] private List<Transform> goalLocations;
    [SerializeField] protected EventReference soundToPlayOnDelivery;

    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        light.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (!soundToPlayOnDelivery.Equals(null)) {
            PlayInteractSound();
        }
        
        var questItem = playerController.StopCarryingItem();
        
        if (questItem == null)
        {
            print("No quest item found!");
            return;
        }

        // Place item
        var targetTransform = goalLocations[playerController.GetCurrentQuest().currentAmount++];
        questItem.transform.position = targetTransform.position;
        questItem.transform.rotation = targetTransform.rotation;
    }

    public void PlayInteractSound()
    {
            EventInstance sound = RuntimeManager.CreateInstance(soundToPlayOnDelivery);
            RuntimeManager.AttachInstanceToGameObject(sound, transform);
            sound.start();
            sound.release();
    }

    public void Activate()
    {
        light.enabled = true;
    }

    public void Deactivate()
    {
        light.enabled = false;
    }
    
    public string GetActionType()
    {
        return "Press";
    }

    public string GetName() => itemName;
    public string GetActionName() => actionName;
}