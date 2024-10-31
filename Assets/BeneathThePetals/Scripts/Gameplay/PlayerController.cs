using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("General")] 
    [SerializeField] private float interactionDistance;
    
    [Tooltip("Time it takes for the camera to look at NPC when interaction is started.")]
    [SerializeField] internal float cameraLookAtTweenDuration;

    [Header("UI")]
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] internal GameObject dialogueBox;
    [SerializeField] private QuestManager questManager;
    [SerializeField] internal Image progressImage;

    private PlayerInputActions playerInput;
    private InputAction interact;

    private GameObject currentTarget;
    private bool canInteract = true;

    private string[] inventory = Array.Empty<string>();

    private Quest currentQuest;

    public delegate void ActivateQuestItems();
    public ActivateQuestItems ActivateQuestItemsCallback;
    
    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerInput.Player.Interact;
        interact.Enable();
        interact.started += InteractMethod;
        interact.canceled += StopInteractionMethod;
    }


    private void OnDisable()
    {
        interact.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        var cameraTransform = GetCamera().transform;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, interactionDistance))
        {
            var newTarget = hit.collider.gameObject;
            
            if (currentTarget)
            {
                if (newTarget != currentTarget)
                {
                    TryDeactivateCurrentTarget();

                    currentTarget = newTarget;

                    TryActivateCurrentTarget();
                }
            }
            else
            {
                currentTarget = newTarget;
                TryActivateCurrentTarget();
            }
        }
        else
        {
            TryDeactivateCurrentTarget();
            currentTarget = null;
        }
    }


    private void TryActivateCurrentTarget()
    {
        if (currentTarget == null || !canInteract) return;

        var currentInteractable = currentTarget.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            if (!currentInteractable.IsInteractable()) return;
            
            currentInteractable.Activate();
            interactionText.text = currentInteractable.GetActionType() + " E to " + currentInteractable.GetActionName() + " \n" +
                                   currentInteractable.GetName();
        }
    }

    private void TryDeactivateCurrentTarget()
    {
        interactionText.text = "";  // Do this regardless of having any target
        
        if (!currentTarget) return;

        var currentInteractable = currentTarget.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            currentInteractable.Deactivate();
        }
    }

    private Transform GetCamera()
    {
        return transform.GetChild(0).GetChild(0);
    }


    void InteractMethod(InputAction.CallbackContext context)
    {
        if (currentTarget == null || !canInteract) return;

        var interactable = currentTarget.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (!interactable.IsInteractable()) return;
            
            interactable.Interact();
        }
    }

    private void StopInteractionMethod(InputAction.CallbackContext context)
    {
        TryDeactivateCurrentTarget();
        TryActivateCurrentTarget();
    }

    public void DisableInput()
    {
        canInteract = false;
        TryDeactivateCurrentTarget();
    }

    public void EnableInput()
    {
        canInteract = true;
    }

    public void AddToInventory(string item)
    {
        Array.Resize(ref inventory, inventory.Length + 1);
        inventory[^1] = item;

        Debug.Log("Added " + item + " to inventory");
        Debug.Log("Inventory: " + string.Join(", ", inventory));
    }

    public void AssignQuest(Quest q)
    {
        currentQuest = q;
        q.OnQuestAdvanced = questManager.UpdateLog;

        ActivateQuestItemsCallback();

        questManager.UpdateLog(currentQuest);
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }
}