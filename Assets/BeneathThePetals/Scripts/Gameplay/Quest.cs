using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public delegate void QuestAdvanced();
    public QuestAdvanced OnQuestAdvanced;
    
    public delegate void QuestFinished(List<DialogueNode> newDialogue);
    public QuestFinished OnQuestFinished;
    
    public bool Completed { get; private set; }
    public string description;
    public int goalAmount;
    private int _currentAmount;
    
    public List<DialogueNode> dialogueToUnlock;
    
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
        
        // Change UI
        // -- done via the OnQuestAdvancedCallback
        
        // Unlock new dialogue
        OnQuestFinished(dialogueToUnlock);
    }
}