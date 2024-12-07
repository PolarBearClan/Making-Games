using FMODUnity;
using FMOD.Studio;
using FMOD;
using UnityEngine;

public class CollectibleWithAnFunctionCall : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string collectibleName;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float rotationAmount = 360f;
    
    [SerializeField]
    private bool activeFromStart = true;

    private bool isCollected = false;

    private GameObject player;
    protected FirstPersonController firstPersonController;
    protected PlayerController playerController;
    public EventReference eventToPlay;

    protected bool shouldEnableInput = true;
    
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
        playerController.ActivateQuestItemsCallback += () => { activeFromStart = true; };
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isCollected)
        {
            firstPersonController.DisableInput();
            if (GetComponent<BoxCollider>())
            {
                Destroy(GetComponent<BoxCollider>());
            }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.up, rotationAmount * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            playerController.AddToInventory(collectibleName);
            if (shouldEnableInput) firstPersonController.EnableInput();
            Destroy(gameObject);
        }
    }

    public virtual void Interact()
    {
        isCollected = true;
        GetComponent<GenericScriptForDisablingObjects>().OnTriggerEnter(null);

    }

    public void PlayInteractSound()
    {
        EventInstance soundOnPickUp = RuntimeManager.CreateInstance(eventToPlay);
        RuntimeManager.AttachInstanceToGameObject(soundOnPickUp, transform);
        soundOnPickUp.start();
        soundOnPickUp.release();
    }

    public virtual void Activate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public virtual void Deactivate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return collectibleName;
    }

    public virtual string GetActionName()
    {
        return "collect";
    }

    protected void DisableInputEnabling()
    {
        shouldEnableInput = false;
    }

    public bool IsInteractable()
    {
        return activeFromStart;
    }
    
    public string GetActionType()
    {
        return "Press";
    }
}
