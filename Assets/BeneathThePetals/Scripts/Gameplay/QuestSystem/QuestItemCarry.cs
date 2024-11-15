using UnityEngine;

public class QuestItemCarry : QuestItemBase
{
    [Space]
    [SerializeField] private QuestItemType itemType;
    
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
        if (playerController.HasFreeSpot()) playerController.StartCarryingItem(gameObject);
        else
        {
            // TODO possible screen note
            print("Player can only carry 2 items at a time!");
        }
    }

    public override void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    
    public QuestItemType QuestItemType => itemType;
}

public enum QuestItemType
{
    WoodLog,
    FlowerPot,
    None
}
