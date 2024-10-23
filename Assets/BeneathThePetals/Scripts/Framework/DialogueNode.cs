using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    private int num;
    
    [TextArea]
    public string mainText; // The main text the NPC says

    public bool isQuestion; // Is this node a question?

    // These fields are relevant if it's a question
    public string option1Text;
    public string option1FollowUp;
    public string option2Text;
    public string option2FollowUp;
}

/* Custom editor - works on its own when the node is mono behaviour, otherwise it doesnt work.
 *              Ideal goal of this: working custom editor of nodes in a list -> possible future TODO
 * 
[CustomEditor(typeof(DialogueNode))]
public class DialogueNode_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (DialogueNode)target;
        
        script.mainText = EditorGUILayout.TextField("Main Text", script.mainText);
        script.isQuestion = EditorGUILayout.Toggle("Is Question", script.isQuestion);

        if (script.isQuestion)
        {
            script.option1Text = EditorGUILayout.TextField("Option 1", script.option1Text);
            script.option2Text = EditorGUILayout.TextField("Option 2", script.option2Text);
            script.option1FollowUp = EditorGUILayout.TextField("Option 1 follow up", script.option1Text);
            script.option2FollowUp = EditorGUILayout.TextField("Option 2 follow up", script.option2Text);
        }
    }
}
*/