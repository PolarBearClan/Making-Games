using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestItemHold : QuestItemBase
{
    [Tooltip("Time in seconds")]
    [SerializeField] private float targetHoldTime;
    
    private bool holdingKey = false;
    private float holdingTime;
    private Image progressImg;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        
        progressImg = playerController.ProgressImage;
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

    private void UpdateUI()
    {
        progressImg.fillAmount = holdingTime / targetHoldTime;
    }

    public override void Interact()
    {
        // Start Hold interaction
        holdingKey = true;
    }

    public override void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
    
    public override void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        
        holdingKey = false;
        holdingTime = 0;
        UpdateUI();
    }
    
    public string GetActionType()
    {
        return "Hold";
    }
}
