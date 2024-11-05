using UnityEngine;

public class QuestItemCarry : QuestItemBase
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
        playerController.StartCarryingItem(gameObject);
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
