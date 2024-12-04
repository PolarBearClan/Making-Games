using System;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;
using Febucci.UI.Core;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private int currentDialogueNodeIdx = -1;

    TypewriterCore typewriter;

    private InputAction navigateAction;
    
    private void Awake()
    {
        mainText = mainTextGameObject.GetComponent<TMPro.TMP_Text>();
        button1Text = buttonChoice1.GetComponentInChildren<TMPro.TMP_Text>();
        button2Text = buttonChoice2.GetComponentInChildren<TMPro.TMP_Text>();
        typewriter = GetComponentInChildren<TypewriterCore>();
        
        navigateAction = GetComponent<PlayerInput>().actions["navigate"];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();

        if(Input.GetKeyDown(KeyCode.Space) && mainTextGameObject.activeSelf)
        {
            typewriter.SkipTypewriter();
        }
    }

    private void CheckForInput()
    {
        if (navigateAction.WasPressedThisFrame() && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(buttonContinue.activeSelf ? buttonContinue : buttonChoice1);
    }

    public void NextText()
    {
        if (dialogueNodes[currentDialogueNodeIdx].givesQuest)
            QuestFromDialogueCallback();
        LoadDialogueNode(currentDialogueNodeIdx + 1);
    }

    public void PlayDialogue(List<DialogueNode> dialogueFromNpc)
    {
        dialogueNodes = dialogueFromNpc;
        Invoke(nameof(DelayedStart), .1f);
    }

    private void DelayedStart()
    {
        gameObject.SetActive(true);

        LoadDialogueNode(0);
    }

    void LoadDialogueNode(int idx)
    {
        // is this last node ?
        if (idx >= dialogueNodes.Count)
        {
            print("Closing dialogue");
            mainText.text = string.Empty;
            
            EventSystem.current.SetSelectedGameObject(null);
            
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

        Invoke(dialogueNode.isQuestion ? nameof(SelectChoice1Button) : nameof(SelectContinueButton), 0.01f);
        
        currentDialogueNodeIdx = idx;
    }

    public void ChooseDialogueOption(int option)
    {
        if (option == 1) mainText.text = dialogueNodes[currentDialogueNodeIdx].option1FollowUp;
        else mainText.text = dialogueNodes[currentDialogueNodeIdx].option2FollowUp;
        
        buttonChoice1.SetActive(false);
        buttonChoice2.SetActive(false);
        buttonContinue.SetActive(true);
        
        Invoke(nameof(SelectContinueButton), 0.01f);
    }

    private void SelectContinueButton()
    {
        EventSystem.current.SetSelectedGameObject(buttonContinue);
    }
    
    private void SelectChoice1Button()
    {
        EventSystem.current.SetSelectedGameObject(buttonChoice1);
    }
}
