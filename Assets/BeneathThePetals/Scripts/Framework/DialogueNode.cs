using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [TextArea]
    public string mainText; // The main text the NPC says

    public bool isQuestion; // Is this node a question?

    // These fields are relevant if it's a question
    public string option1Text;
    public string option1FollowUp;
    public string option2Text;
    public string option2FollowUp;

    public bool givesQuest;
}
