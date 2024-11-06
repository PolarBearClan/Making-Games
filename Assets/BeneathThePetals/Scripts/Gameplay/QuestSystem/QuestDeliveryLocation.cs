using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestDeliveryLocation : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string actionName;
    [SerializeField] private Light light;
    
    [SerializeField] private List<Transform> goalLoactions;
    
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
        // Deliver quest item
        var questItem = playerController.DropCarriedItem();

        var targetPos = goalLoactions[playerController.GetCurrentQuest().currentAmount++];
        
        questItem.transform.position = targetPos.position;
        questItem.transform.rotation = targetPos.rotation;
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
}
