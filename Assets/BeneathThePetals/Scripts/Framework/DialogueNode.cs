using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [TextArea]
    public string mainText; // The main text the NPC says

    [Space]
    public bool isQuestion; // Is this node a question?

    // These fields are relevant if it's a question
    [Space]
    public string option1Text;
    public string option1FollowUp;
    [Space]
    public string option2Text;
    public string option2FollowUp;

    [Space]
    public bool givesQuest;
}
