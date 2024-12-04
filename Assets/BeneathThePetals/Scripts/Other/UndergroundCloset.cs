using System.Collections;
using UnityEngine;
using TMPro;

public class UndergroundCloset : MonoBehaviour, IInteractable
{
    [Header("Objects")]
    [SerializeField] private GameObject closetInside;
    [SerializeField] private Transform placeToSit;
    [SerializeField] private Transform faceToPoint;
    [SerializeField] private Transform afterHide;

    [Header("Dialogue Variables")]
    [SerializeField] private int[] notificationDuration;
    [SerializeField, TextArea] private string[] notificationText;
    [SerializeField] private TMP_Text nameText;

    [HideInInspector]
    public bool isHiding = false;

    private MeshRenderer closet;
    private GameObject player;
    private FirstPersonController playerControls;
    private InteractableLight interactableLight;
    private PlayerController playerController;

    private float playerFOV;
    private float playerClipping;
    private int currentText = 0;
    private bool coroutineStarted = false;


    private void Start()
    {
        closet = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerControls = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
        interactableLight = GetComponentInChildren<InteractableLight>();
        closetInside.SetActive(false);


        playerFOV = player.GetComponentInChildren<Camera>().fieldOfView;
        playerClipping = player.GetComponentInChildren<Camera>().nearClipPlane;
    }

    private void Update()
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

    public void Interact()
    {
        if (!playerControls.isHiding)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerControls.isWalking = false;
            HideInCloset();
            isHiding = true;
        }
        else
        {
            StopHiding();
            isHiding = false;
        }
    }

    private void HideInCloset()
    {
        closet.enabled = false;
        closetInside.SetActive(true);

        playerControls.DisableMovement();
        playerControls.isHiding = true;

        player.GetComponentInChildren<Light>().enabled = false;
        player.GetComponent<Collider>().enabled = false;

        player.GetComponentInChildren<Camera>().fieldOfView = 40f;
        player.GetComponentInChildren<Camera>().nearClipPlane = 0.01f;
        player.GetComponent<Transform>().LookAt(faceToPoint);
        player.transform.position = placeToSit.position;

        interactableLight.transform.gameObject.SetActive(false);

    }

    private void StopHiding()
    {
        closet.enabled = true;
        closetInside.SetActive(false);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        playerControls.EnableMovement();
        playerControls.isHiding = false;

        player.GetComponentInChildren<Light>().enabled = true;
        player.GetComponent<Collider>().enabled = true;

        player.GetComponentInChildren<Camera>().fieldOfView = playerFOV;
        player.GetComponentInChildren<Camera>().nearClipPlane = playerClipping;
        player.transform.position = afterHide.position;

        interactableLight.transform.gameObject.SetActive(true);
        playerController.ScreenNoteManagerScript.CloseNote();
        StopAllCoroutines();
    }

    private IEnumerator NextDialogue()
    {
        while (currentText < notificationText.Length)
        {
            if (currentText > 0)
                nameText.text = "Leader Valgaris:";
            coroutineStarted = true;
            playerController.ScreenNoteManagerScript.ShowNoteNotification(notificationText[currentText], notificationDuration[currentText]);
            yield return new WaitForSeconds(notificationDuration[currentText] + 1);
            currentText++;
        }

        coroutineStarted = false;
    }

    public void StartDialogue()
    {
        if(!coroutineStarted)
            StartCoroutine(NextDialogue());
    }

    public void Activate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public string GetName()
    {
        return "";
    }
    
    public string GetActionType()
    {
        return "Press";
    }
    
    public void PlayInteractSound() { }
}
