using UnityEngine;

public class QuestObjectiveTest : MonoBehaviour, IInteractable
{
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
        return "QuestGiverTest";
    }

    public string GetActionName()
    {
        return "interact with";
    }

    public bool IsInteractable()
    {
        return isActive;
    }
}
