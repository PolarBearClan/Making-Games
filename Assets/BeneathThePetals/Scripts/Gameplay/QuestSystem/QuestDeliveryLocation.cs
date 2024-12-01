using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class QuestDeliveryLocation : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string actionName;
    [SerializeField] private Light light;
    [Tooltip("This object will accept only items of the following type.")]
    [SerializeField] private QuestItemType questItemType;
    [SerializeField] private GameObject flowerPotPrefab;

    [Space]
    [SerializeField] private List<Transform> goalLoactions;
    [SerializeField] private List<GameObject> finalLogs;

    [SerializeField] protected EventReference soundToPlayOnDelivery;

    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        light.enabled = false;
        foreach(GameObject obj in finalLogs)
        {
            obj.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        GameObject questItem = null;

        if (!soundToPlayOnDelivery.Equals(null)) {
            PlayInteractSound();
        
        }
        switch (questItemType)
        {
            case QuestItemType.WoodLog:
            {
                if (playerController.GetCarriedItemType() != QuestItemType.WoodLog)
                {
                    print("This item accepts different quest item type!");
                    return;
                }
                questItem = playerController.StopCarryingItem();
                break;
            }

            case QuestItemType.FlowerPot:
            {
                if (playerController.RemoveFromInventory("Flower Pot"))
                {
                    questItem = Instantiate(flowerPotPrefab);
                    questItem.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    print("Player does not have any flower pot in his inventory.");
                    return;
                }
                break;
            }
        }

        if (questItem == null)
        {
            print("No quest item found!");
            return;
        }

        // Place item
        int logNumber = playerController.GetCurrentQuest().currentAmount++;
        var targetTransform = goalLoactions[logNumber];
        questItem.transform.position = targetTransform.position;
        questItem.transform.rotation = targetTransform.rotation;
        finalLogs[logNumber].SetActive(true);
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

    public string GetName() => itemName;
    public string GetActionName() => actionName;
    public QuestItemType QuestItemType => questItemType;
}