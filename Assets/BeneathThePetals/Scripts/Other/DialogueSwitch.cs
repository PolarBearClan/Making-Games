using UnityEngine;

public class DialogueSwitch : MonoBehaviour
{

    private NPCBaseController[] baseControllers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponents<NPCBaseController>() != null)
        {
            baseControllers = GetComponents<NPCBaseController>();
            baseControllers[0].enabled = true;
            baseControllers[1].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeDialogue()
    {
        if (baseControllers != null)
        {
            baseControllers[0].enabled = false;
            baseControllers[1].enabled = true;
        }
    }
}
