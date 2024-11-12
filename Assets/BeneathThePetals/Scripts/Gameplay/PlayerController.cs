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
    [SerializeField] private float cameraLookAtTweenDuration;

    [Header("UI")]
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private Image progressImage;
    [SerializeField] private ScreenNoteManager screenNoteManager;

    private PlayerInputActions playerInput;
    private InputAction interact;

    private GameObject currentTarget;
    private bool canInteract = true;
    private Transform cameraTransform;
    private bool hiding = false;
    
    private List<string> inventory = new List<string>();

    
    // Quest related properties
    public delegate void ActivateQuestItems();
    public ActivateQuestItems ActivateQuestItemsCallback;

    private Quest currentQuest;
    private GameObject currentlyCarriedItem = null;
    private bool carryingItem = false;
    
    [Header("Quest related")]
    [SerializeField] private Transform carryParrent;
    
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
        screenNoteManager.NoteEndCallback = () =>
        {
            EnableInput();
            GetComponent<FirstPersonController>().EnableInput();
        };
        cameraTransform = GetCamera().transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, interactionDistance))
        {
            var newTarget = hit.collider.gameObject;

            // Check if this is a quest delivery area -> only able to interact with this when carryingItem
            var deliveryArea = newTarget.GetComponent<QuestDeliveryLocation>();
            if (deliveryArea != null)
            {
                // This is a quest delivery area
                if (GetCurrentQuest() == null || GetCurrentQuest().Completed) return;
                if (deliveryArea.QuestItemType == QuestItemType.WoodLog && !carryingItem) return;
                
                TryDeactivateCurrentTarget();
                currentTarget = newTarget;
                TryActivateCurrentTarget(true);
            }
            
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


    private void TryActivateCurrentTarget(bool aimingAtQuestItem = false)
    {
        if (currentTarget == null || !canInteract) return;

        var currentInteractable = currentTarget.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            if (!currentInteractable.IsInteractable()) return;
            
            currentInteractable.Activate();
            ChangeText(currentInteractable.GetActionType() + " E to " + currentInteractable.GetActionName() + " \n" +
                       currentInteractable.GetName(), aimingAtQuestItem);
        }
    }

    private void TryDeactivateCurrentTarget()
    {
        ChangeText("", true); // Do this regardless of having any target
        
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
        inventory.Add(item);
        Debug.Log("Inventory: " + string.Join(", ", inventory));
    }

    public bool RemoveFromInventory(string item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            Debug.Log("Inventory: " + string.Join(", ", inventory));
            return true;
        }
        return false;
    }
    
    public void AssignQuest(Quest q)
    {
        currentQuest = q;
        q.OnQuestAdvanced = questManager.UpdateLog;

        ActivateQuestItemsCallback();

        questManager.UpdateLog(currentQuest);
    }

    public void StartCarryingItem(GameObject itemToCarry)
    {
        currentlyCarriedItem = itemToCarry;
        carryingItem = true;
        interactionText.text = "";
        
        currentlyCarriedItem.transform.SetParent(carryParrent);
        currentlyCarriedItem.transform.localPosition = Vector3.zero;
        
        currentlyCarriedItem.GetComponent<QuestItemBase>().DeactivateItem(); // not available for further interaction
        currentlyCarriedItem.GetComponent<Collider>().enabled = false;
    }

    public GameObject DropCarriedItem()
    {
        currentlyCarriedItem.transform.SetParent(null);

        GameObject carriedItem = currentlyCarriedItem;
        
        carryingItem = false;
        currentlyCarriedItem = null;
        interactionText.text = "";

        return carriedItem;
    }

    public QuestItemType GetCarriedItemType()
    {
        return currentlyCarriedItem.GetComponent<QuestItemCarry>().QuestItemType;
    }

    private void ChangeText(string text, bool overridingPermission = false)
    {
        if (overridingPermission || !carryingItem)
            interactionText.text = text;
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }

    public bool GetHidingStatus()
    {
        return hiding;
    }

    public void SetHidingStatus(bool desiredState)
    {
        hiding = desiredState;
    }

    public GameObject DialogueBox => dialogueBox;
    public Image ProgressImage => progressImage;
    public ScreenNoteManager ScreenNoteManagerScript => screenNoteManager;
    public float CameraLookAtTweenDuration => cameraLookAtTweenDuration;
}