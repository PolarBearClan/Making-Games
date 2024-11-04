using JetBrains.Annotations;
using UnityEngine;

public class HidingInteractable : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float hidingHeight;
    public GameObject lookAt;
    private PlayerController playerController;
    private Vector3 previousSpot;

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
        if (!playerController.GetHidingStatus())
        {
            return "Hide";
        }
        else {

            return "Stop hiding";

        }
    }

    public string GetName()
    {
        return "";
    }

    public void Interact()
    {
        if (!playerController.GetHidingStatus())
        {
            previousSpot = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            var playerControls = player.GetComponent<FirstPersonController>();
            playerControls.DisableInput();
            playerControls.cameraCanMove = true;
            playerControls.lockCursor = true;
            playerController.SetHidingStatus(true);
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Transform>().LookAt(lookAt.transform);
            player.transform.position = new Vector3(transform.position.x, transform.position.y - hidingHeight, transform.position.z);
        }
        else {
            var playerControls = player.GetComponent<FirstPersonController>();
            playerControls.EnableInput();
            player.GetComponent<Rigidbody>().useGravity = true;
            playerController.SetHidingStatus(false);
            player.transform.position = new Vector3(previousSpot.x, previousSpot.y, previousSpot.z); 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        previousSpot = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
