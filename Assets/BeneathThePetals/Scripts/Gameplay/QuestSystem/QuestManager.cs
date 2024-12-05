using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text description;
    [SerializeField] private TMPro.TMP_Text progress;

    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        UpdateLog();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLog()
    {
        UpdateLog(playerController.GetCurrentQuest());
    }

    public void UpdateLog(Quest q)
    {
        if (q != null)
        {
            gameObject.SetActive(true);
            if (q.Completed)
            {
                description.text = "Completed!";
                //progress.text = "Completed!";
            }
            else
            {        
                description.text = description.text = q.Description + ": " + q.currentAmount + "/" + q.GoalAmount;
                //progress.text = q.currentAmount + "/" + q.GoalAmount;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
