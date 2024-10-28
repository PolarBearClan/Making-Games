using UnityEngine;
using UnityEngine.UI;

public class QuestItemHold : MonoBehaviour, IInteractable
{
    [SerializeField] private float targetTime;
    [SerializeField] private Image progressImg;
    
    private bool holdingKey = false;
    private float holdingTime;
    
    private PlayerController playerController;
    private bool isActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.ActivateQuestItemsCallback += () => { isActive = true; };
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingKey) return;
        
        holdingTime += Time.deltaTime;

        if (holdingTime >= targetTime)
        {
            print("Quest advanced");

            playerController.GetCurrentQuest().currentAmount++;
                
            holdingTime = 0;
            UpdateUI();
                
            Destroy(gameObject);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        progressImg.fillAmount = holdingTime / targetTime;
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
        return "broken cube";
    }

    public string GetActionName()
    {
        return "fix";
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
