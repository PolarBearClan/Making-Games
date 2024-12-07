using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Debug = FMOD.Debug;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float interactionDistance;

    [Tooltip("Time it takes for the camera to look at NPC when interaction is started.")]
    [SerializeField] private float cameraLookAtTweenDuration;

    [Header("UI")]
    [SerializeField] public TMP_Text interactionText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private Image progressImage;
    [SerializeField] private ScreenNoteManager screenNoteManager;

    private PlayerInputActions playerInput;
    private InputAction interact;
    public EventReference eventToPlayWhenBob;
    public EventReference eventToPlayWhenJump;
    private GameObject currentTarget;
    private bool canInteract = true;
    private Transform cameraTransform;

    private List<string> inventory = new List<string>();

    // Quest related properties
    public delegate void ActivateQuestItems();
    public ActivateQuestItems ActivateQuestItemsCallback;

    private Quest currentQuest;
    private GameObject currentlyCarriedItem1 = null;
    private GameObject currentlyCarriedItem2 = null;
    private bool carryingItem = false;
    private PauseMenu pauseMenu;
    public bool isCurrentlyChangingScenes = false;
    private int pickedUpItems = 0;

    [Header("Quest related")]
    [SerializeField] private Transform carryParent1;
    [SerializeField] private Transform carryParent2;

    /*
    [Space] [Header("Inventory related")]
    [SerializeField] private GameObject uiGameObject;
    [SerializeField] private GameObject inventoryUIGameObject;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemInfo;
    [SerializeField] private GameObject textPanel;
    */
    
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
        pauseMenu = GameObject.FindAnyObjectByType<PauseMenu>();

        screenNoteManager.NoteEndCallback = () =>
        {
            EnableInput();
            GetComponent<FirstPersonController>().EnableInput();
        };
        cameraTransform = GetCamera().transform;
        
        InitInventoryObject();
    }

    private void InitInventoryObject()
    {
        // Load inventory
        var inventoryManager = FindAnyObjectByType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventory = inventoryManager.inventoryItems;
            print("Inventory loaded successfully");
        }
        else
        {
            UnityEngine.Debug.LogError("Inventory Manager not found! Could not load inventory!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCurrentlyChangingScenes)
        {
            if (pauseMenu != null)
                if(pauseMenu.isPaused)
                    return;

            CheckForInteractables();
        } else {
            DisableInput();
            interactionText.SetText("");
        }


    }

    private void CheckForInteractables()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, interactionDistance, ~LayerMask.GetMask("Player", "SoundTrigger", "UselessColliders")) && !isCurrentlyChangingScenes) {

            var newTarget = hit.collider.gameObject;

            // Check if this is a quest delivery area -> only able to interact with this when carryingItem
            var deliveryArea = newTarget.GetComponent<QuestDeliveryLocation>();
            if (deliveryArea != null)
            {
                // This is a quest delivery area
                if (currentQuest == null || currentQuest.Completed) return;
                if (!carryingItem) return;

                TryDeactivateCurrentTarget();
                currentTarget = newTarget;
                TryActivateCurrentTarget(true);
            }

            // Check if this is a scene changer -> if carrying at least 1 log -> disable this interaction
            var sceneChanger = newTarget.GetComponent<SceneChange>();
            if (sceneChanger != null)
            {
                // this is a scene changer
                if (GetCarriedItemsCount() > 0) return;
            }
            
            // check if it is a QuestItemCarry
            var questItemCarry = newTarget.GetComponent<QuestItemCarry>();
            if (questItemCarry != null)
            {
                // it is a quest item carry -> only show text if player can carry more items
                if (!CanPickUpItem()) return;
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
        }  else {
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

    public Transform GetCamera()
    {
        return transform.GetChild(0).GetChild(0);
    }


    private void InteractMethod(InputAction.CallbackContext context)
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
        if (AimingAtDoor()) return;

        TryDeactivateCurrentTarget();
        TryActivateCurrentTarget();
    }

    private bool AimingAtDoor()
    {
        if (currentTarget == null) return true; // optimization

        var doorController = currentTarget.GetComponent<DoorController>();

        return doorController != null;
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

    public void ResetInteractionTarget()
    {
        TryDeactivateCurrentTarget();
        currentTarget = null;
    }

    public void AddToInventory(string item)
    {
        inventory.Add(item);
        UnityEngine.Debug.Log("Inventory: " + string.Join(", ", inventory));
        UnityEngine.Debug.Log("Added " + item + " to inventory");
        UnityEngine.Debug.Log("Inventory: " + string.Join(", ", inventory));

        InventoryManager.Instance.AddItem(item);
        FindAnyObjectByType<InventoryUI>().UpdateInventoryUI();
    }

    public bool RemoveFromInventory(string item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            UnityEngine.Debug.Log("Inventory: " + string.Join(", ", inventory));
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
        ref GameObject carriedItem = ref GetFreeItemGameObject();
        Transform carryParent = GetFreeSpotParent();

        carriedItem = itemToCarry;
        carryingItem = true;
        interactionText.text = "";

        carriedItem.transform.SetParent(carryParent);
        carriedItem.transform.localPosition = Vector3.zero;

        carriedItem.GetComponent<QuestItemBase>().DeactivateItem(); // not available for further interaction
        carriedItem.GetComponent<Collider>().enabled = false;

        pickedUpItems++;
    }

    public GameObject StopCarryingItem()
    {
        ref GameObject carriedItem = ref GetCarriedItemGameObject();

        carriedItem.transform.SetParent(null);
        GameObject carriedItemRet = carriedItem;
        carryingItem = GetCarriedItemsCount() != 1; // Only set this to false if I am carrying just 1 item

        carriedItem = null;
        interactionText.text = "";
        return carriedItemRet;
    }

    public bool HasFreeSpot() => currentlyCarriedItem1 == null || currentlyCarriedItem2 == null;
    private Transform GetFreeSpotParent() => currentlyCarriedItem1 == null ? carryParent1 : carryParent2;

    private ref GameObject GetFreeItemGameObject()
    {
        if (currentlyCarriedItem1 == null) return ref currentlyCarriedItem1;
        return ref currentlyCarriedItem2;
    }

    private ref GameObject GetCarriedItemGameObject()
    {
        if (currentlyCarriedItem1 != null) return ref currentlyCarriedItem1;
        return ref currentlyCarriedItem2;
    }

    private int GetCarriedItemsCount()
    {
        var c = 0;
        if (currentlyCarriedItem1 != null) c++;
        if (currentlyCarriedItem2 != null) c++;
        return c;
    }

    private void ChangeText(string text, bool overridingPermission = false)
    {
        // TODO remove this if it is okay like this
        // should players interaction be disabled if he is carrying 2 items?
        // or even 1 item? - this probably no
        //
        //if (overridingPermission || GetCarriedItemsCount() < 2)
        if (!isCurrentlyChangingScenes)
        {
            interactionText.text = text;
        }


    }

    public void LockedDoorText()
    {
        ChangeText("Locked.");
    }

    public Quest GetCurrentQuest() => currentQuest;
    public GameObject DialogueBox => dialogueBox;
    public Image ProgressImage => progressImage;
    public ScreenNoteManager ScreenNoteManagerScript => screenNoteManager;
    public float CameraLookAtTweenDuration => cameraLookAtTweenDuration;

    void OnDrawGizmos()
    {
        cameraTransform = GetCamera().transform;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance);
    }

    public bool CanPickUpItem()
    {
        return pickedUpItems < currentQuest.GoalAmount;
    }
}