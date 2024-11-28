using JetBrains.Annotations;
using UnityEngine;

public class HidingInteractable : MonoBehaviour, IInteractable
{
    [Header("Objects")]
    [SerializeField] private GameObject closetInside;
    [SerializeField] private Transform placeToSit;
    [SerializeField] private Transform faceToPoint;

    [HideInInspector]
    public bool isHiding = false;

    private MeshRenderer closet;
    private GameObject player;
    private FirstPersonController playerControls;

    private float playerFOV;
    private float playerClipping;
    private Vector3 previousSpot;


    public void PlayInteractSound() { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        closet = GetComponent<MeshRenderer>();  
        player = GameObject.FindGameObjectWithTag("Player");
        playerControls = player.GetComponent<FirstPersonController>();
        closetInside.SetActive(false);


        playerFOV = player.GetComponentInChildren<Camera>().fieldOfView;
        playerClipping = player.GetComponentInChildren<Camera>().nearClipPlane;
    }
    public void Activate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Update()
    {
        if (isHiding)
        {
            player.transform.position = placeToSit.position;
        }
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

    public void Interact()
    {
        if (!playerControls.isHiding)
        {
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

    private void HideInCloset()
    {
        closet.enabled = false;
        closetInside.SetActive(true);

        previousSpot = player.transform.position;

        playerControls.DisableMovement();
        playerControls.isHiding = true;

        player.GetComponentInChildren<Light>().enabled = false;
        player.GetComponent<Collider>().enabled = false;

        player.GetComponentInChildren<Camera>().fieldOfView = 40f;
        player.GetComponentInChildren<Camera>().nearClipPlane = 0.01f;
        player.GetComponent<Transform>().LookAt(faceToPoint);
        player.transform.position = placeToSit.position;
    }

    private void StopHiding()
    {
        closet.enabled = true;
        closetInside.SetActive(false);

        playerControls.EnableMovement();
        playerControls.isHiding = false;

        player.GetComponentInChildren<Light>().enabled = true;
        player.GetComponent<Collider>().enabled = true;

        player.GetComponentInChildren<Camera>().fieldOfView = playerFOV;
        player.GetComponentInChildren<Camera>().nearClipPlane = playerClipping;
        player.transform.position = previousSpot;
    }
}
