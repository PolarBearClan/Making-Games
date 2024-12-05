using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestItemHold : QuestItemBase
{
    [Tooltip("Time in seconds")]
    [SerializeField] private float targetHoldTime;
    [SerializeField] private bool isClothes = true;
    
    private bool holdingKey = false;
    private float holdingTime;
    private Image progressImg;

    private FirstPersonController firstPersonController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        
        progressImg = playerController.ProgressImage;
        
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingKey) return;
        firstPersonController.isWalking = false;
        holdingTime += Time.deltaTime;

        if (holdingTime >= targetHoldTime)
        {
            //print("Quest advanced");

            playerController.GetCurrentQuest().currentAmount++;
            PlayInteractSound();
                
            holdingTime = 0;
            UpdateUI();
            
            firstPersonController.EnableInput();
            
            if(isClothes)
                Destroy(gameObject);
            else if(GetComponentInChildren<ParticleSystem>() != null)
            {
                GetComponentInChildren<ParticleSystem>().Play();
                ItemCompleted();
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        progressImg.fillAmount = holdingTime / targetHoldTime;
    }

    public override void Interact()
    {
        // Start Hold interaction
        holdingKey = true;
        
        // Disabling movement when holding the key
        //firstPersonController.DisableInput(false);
    }

    public override void Activate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
    }
    
    public override void Deactivate()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
        
        //firstPersonController.EnableInput();
        
        holdingKey = false;
        holdingTime = 0;
        UpdateUI();
    }
    
    public override string GetActionType()
    {
        return "Hold";
    }

    private void ItemCompleted()
    {
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<InteractableLight>().transform.gameObject.SetActive(false);
    }
    
    private void PlayInteractSound()
    {
        EventInstance soundOnInteract = RuntimeManager.CreateInstance(soundToPlayOnPickUp);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
}
