using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    // this might be deleted, depends on the selected approach
    public delegate void DialogueEnd();
    public DialogueEnd DialogueEndCallback;
    
    public delegate void QuestFromDialogue();
    public QuestFromDialogue QuestFromDialogueCallback;
    
    [SerializeField] private GameObject mainTextGameObject;
    [SerializeField] private GameObject buttonChoice1;
    [SerializeField] private GameObject buttonChoice2;
    [SerializeField] private GameObject buttonContinue;
    
    private TMPro.TMP_Text mainText;
    private TMPro.TMP_Text button1Text;
    private TMPro.TMP_Text button2Text;
    
    private List<DialogueNode> dialogueNodes;
    private int currentDialogueNode = -1;


    private void Awake()
    {
        mainText = mainTextGameObject.GetComponent<TMPro.TMP_Text>();
        button1Text = buttonChoice1.GetComponentInChildren<TMPro.TMP_Text>();
        button2Text = buttonChoice2.GetComponentInChildren<TMPro.TMP_Text>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextText()
    {
        if (dialogueNodes[currentDialogueNode].givesQuest) 
            QuestFromDialogueCallback();
        LoadDialogueNode(currentDialogueNode + 1);
    }
    
    public void PlayDialogue(List<DialogueNode> dialogueFromNpc)
    {
        dialogueNodes = dialogueFromNpc;
        gameObject.SetActive(true);
        
        LoadDialogueNode(0);
    }

    void LoadDialogueNode(int idx)
    {
        // is this last node ?
        if (idx >= dialogueNodes.Count)
        {
            print("Closing dialogue");
            gameObject.SetActive(false);
            DialogueEndCallback();
            return;
        }
        
        var dialogueNode = dialogueNodes[idx];

        mainText.text = dialogueNode.mainText;
        buttonChoice1.SetActive(dialogueNode.isQuestion);
        button1Text.text = dialogueNode.option1Text;
        buttonChoice2.SetActive(dialogueNode.isQuestion);
        button2Text.text = dialogueNode.option2Text;
        buttonContinue.SetActive(!dialogueNode.isQuestion);
        
        currentDialogueNode = idx;
    }

    public void ChooseDialogueOption(int option)
    {
        if (option == 1)
        {
            mainText.text = dialogueNodes[currentDialogueNode].option1FollowUp;
        }
        else
        {
            mainText.text = dialogueNodes[currentDialogueNode].option2FollowUp;
        }
        buttonChoice1.SetActive(false);
        buttonChoice2.SetActive(false);
        buttonContinue.SetActive(true);
    }
}
