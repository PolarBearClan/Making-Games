using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public TMP_Text interactionText;
    public GameObject dialogueBox;
    
    private PlayerInputActions playerInput;
    private InputAction interact;

    private GameObject currentTarget;
    private bool canInteract = true;
    
    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerInput.Player.Interact;
        interact.Enable();
        interact.started += InteractMethod;
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
        
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit))
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
        if (!currentTarget) return;
        
        var currentInteractable = currentTarget.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            currentInteractable.Activate();
            interactionText.text = "Press E to interact with\n" + currentTarget.name + ".";
        }
    }

    private void TryDeactivateCurrentTarget()
    {
        if (!currentTarget) return;
        
        var currentInteractable = currentTarget.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            currentInteractable.Deactivate();
            interactionText.text = "";
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
            interactable.Interact();
        }
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
}