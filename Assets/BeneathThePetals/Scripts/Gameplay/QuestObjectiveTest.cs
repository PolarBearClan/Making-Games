using UnityEngine;

public class QuestObjectiveTest : MonoBehaviour, IInteractable
{
    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        print("Quest advanced");
        
        // TODO checks if the player is on any quest (+ this quest)
        // Maybe we dont need check for this quest
        // this because on each day there will be only items that will correspond to the current task
        
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
}
