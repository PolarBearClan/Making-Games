using JetBrains.Annotations;
using UnityEngine;

public class HidingInteractable : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float hidingHeight;
    public GameObject lookAt;
    private FirstPersonController playerControls;
    private Vector3 previousSpot;

    public void PlayInteractSound() { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    { 

        playerControls = player.GetComponent<FirstPersonController>();
        previousSpot = new Vector3(0, 0, 0);
    }
    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetActionName()
    {
        if (!playerControls.isHiding)
        {
            return "Hide";
        }
        else
        {

            return "Stop hiding";

        }
    }

    public string GetName()
    {
        return "";
    }

    public FirstPersonController GetController()
    {
        return playerControls;
    }

    public void Interact()
    {
        if (!playerControls.isHiding)
        {
            playerControls.transform.localScale = new Vector3(playerControls.originalScale.x, playerControls.originalScale.y, playerControls.originalScale.z);
            previousSpot = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            playerControls.DisableInput();
            playerControls.cameraCanMove = true;
            playerControls.lockCursor = true;
            playerControls.isWalking = false;
            playerControls.isHiding = true;
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            player.GetComponent<Transform>().LookAt(lookAt.transform);
            player.transform.position = new Vector3(transform.position.x, transform.position.y - hidingHeight, transform.position.z);
        }
        else {
            var playerControls = player.GetComponent<FirstPersonController>();
            playerControls.EnableInput();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<Rigidbody>().useGravity = true;
            playerControls.isHiding = false;
            playerControls.enableCrouch = true;
            player.transform.position = new Vector3(previousSpot.x, previousSpot.y, previousSpot.z); 
        }
    }
    
    public string GetActionType()
    {
        return "Press";
    }

}
