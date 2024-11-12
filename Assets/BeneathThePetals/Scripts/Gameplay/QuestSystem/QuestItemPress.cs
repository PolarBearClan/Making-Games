using UnityEngine;

public class QuestItemPress : QuestItemBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Interact()
    {
        print("Quest advanced");
            
        playerController.GetCurrentQuest().currentAmount++;
            
        Destroy(gameObject);
    }

    public override void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
