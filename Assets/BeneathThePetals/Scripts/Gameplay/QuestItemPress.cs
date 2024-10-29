using UnityEngine;

public class QuestItemPress : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string actionName;
    
    private bool isActive = false;
    
    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.ActivateQuestItemsCallback += () => { isActive = true; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        print("Quest advanced");
            
        playerController.GetCurrentQuest().currentAmount++;
            
        Destroy(gameObject);
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return itemName;
    }

    public string GetActionName()
    {
        return actionName;
    }

    public bool IsInteractable()
    {
        return isActive;
    }
}
