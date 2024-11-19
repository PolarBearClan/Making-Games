using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using FMOD.Studio;

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
    public EventReference eventToPlayWhenBob;
    public EventReference eventToPlayWhenJump;
    private GameObject currentTarget;
    private bool canInteract = true;
    private Transform cameraTransform;
    private bool hiding = false;

    private List<string> inventory = new List<string>();

    // Quest related properties
    public delegate void ActivateQuestItems();
    public ActivateQuestItems ActivateQuestItemsCallback;

    private Quest currentQuest;
    private GameObject currentlyCarriedItem1 = null;
    private GameObject currentlyCarriedItem2 = null;
    private bool carryingItem = false;

    [Header("Quest related")]
    [SerializeField] private Transform carryParent1;
    [SerializeField] private Transform carryParent2;

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

        // DontDestroyOnLoad(gameObject); TODO in the next iteration - needs more thought
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
        UnityEngine.Debug.Log("Inventory: " + string.Join(", ", inventory));
        Debug.Log("Added " + item + " to inventory");
        Debug.Log("Inventory: " + string.Join(", ", inventory));

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

    public QuestItemType GetCarriedItemType()
    {
        return GetCarriedItemGameObject() != null
            ? GetCarriedItemGameObject().GetComponent<QuestItemCarry>().QuestItemType
            : QuestItemType.None;
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

        interactionText.text = text;
    }

    public void SetHidingStatus(bool desiredState)
    {
        hiding = desiredState;
    }

    public Quest GetCurrentQuest() => currentQuest;
    public bool GetHidingStatus() => hiding;

    public GameObject DialogueBox => dialogueBox;
    public Image ProgressImage => progressImage;
    public ScreenNoteManager ScreenNoteManagerScript => screenNoteManager;
    public float CameraLookAtTweenDuration => cameraLookAtTweenDuration;
}