using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Quest
{
    public delegate void QuestAdvanced();
    public QuestAdvanced OnQuestAdvanced;
    
    public delegate void QuestFinished(List<DialogueNode> newDialogue);
    public QuestFinished OnQuestFinished;
    
    public bool Completed { get; private set; }
    [SerializeField] private string description;
    [SerializeField] private int goalAmount;
    
    private int _currentAmount;
    
    [Space]
    [SerializeField] private List<DialogueNode> dialogueAfterQuestCompleted;
    
    [Space]
    [SerializeField] private bool notificationAfterQuestCompleted = false;
    [SerializeField] private int notificationDuration = 2;
    [SerializeField] [TextArea] private string notificationText;
    
    public int currentAmount
    {
        get => _currentAmount;
        set
        {
            _currentAmount = value;
            Evaluate();
            OnQuestAdvanced();
        }
    }


    public Quest(string description, int currentAmount, int goalAmount)
    {
        this.description = description;
        this.Completed = false;
        this._currentAmount = currentAmount;
        this.goalAmount = goalAmount;
    }
    
    public void Evaluate()
    {
        if (currentAmount >= goalAmount)
        {
            Completed = true;
            Complete();
        }
    }

    private void Complete()
    {
        Debug.Log("Quest completed!");
        
        // Unlock new dialogue
        OnQuestFinished(dialogueAfterQuestCompleted);
    }

    public string Description => description;

    public int GoalAmount => goalAmount;
    public bool ShouldNotify => notificationAfterQuestCompleted;
    public string NotificationText => notificationText;
    public int NotificationDuration => notificationDuration;
}