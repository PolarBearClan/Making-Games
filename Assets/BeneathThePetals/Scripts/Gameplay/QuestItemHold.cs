using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestItemHold : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string actionName;
    [SerializeField] private float targetHoldTime;
    
    private bool isActive = false;
    
    private bool holdingKey = false;
    private float holdingTime;
    private Image progressImg;
    
    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.ActivateQuestItemsCallback += () => { isActive = true; };
        
        progressImg = playerController.progressImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingKey) return;
        
        holdingTime += Time.deltaTime;

        if (holdingTime >= targetHoldTime)
        {
            print("Quest advanced");

            playerController.GetCurrentQuest().currentAmount++;
                
            holdingTime = 0;
            UpdateUI();
                
            Destroy(gameObject);
        }
        UpdateUI();
    }

    public void PlayInteractSound() { }
    private void UpdateUI()
    {
        progressImg.fillAmount = holdingTime / targetHoldTime;
    }

    public void Interact()
    {
        // Start Hold interaction
        holdingKey = true;
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
    
    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        
        holdingKey = false;
        holdingTime = 0;
        UpdateUI();
    }

    public string GetName()
    {
        return itemName;
    }

    public string GetActionName()
    {
        return actionName;
    }
    
    public string GetActionType()
    {
        return "Hold";
    }
    
    public bool IsInteractable()
    {
        return isActive;
    }
}
