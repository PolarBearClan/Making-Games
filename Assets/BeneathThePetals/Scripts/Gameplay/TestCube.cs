using UnityEngine;

public class TestCube : MonoBehaviour, IInteractable
{
    public string objectName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        print("I HAVE BEEN INTERACTED WITH! My name: " + gameObject.name);
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return objectName;
    }

    public string GetActionName()
    {
        return "interact with";
    }
}
