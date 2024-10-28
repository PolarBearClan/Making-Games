using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationAngle = 90;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private Ease rotationEase = Ease.OutBounce;
    
    [Space]
    [SerializeField]
    private bool doorLocked = false;
    
    private bool doorOpen = false;
    private bool interactable = true;
    private BoxCollider doorCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorCollider = GetComponent<BoxCollider>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (!interactable) return;
        
        if (doorLocked)
        {
            // Play locked sound effect
            // Animation too ? 
        }
        else
        {
            transform.DOComplete();
            
            var rotationChange = new Vector3(0, rotationAngle, 0);
            if (doorOpen) rotationChange *= -1;
            doorOpen = !doorOpen;
            interactable = false;
            doorCollider.enabled = false;
            
            transform.DORotate(transform.eulerAngles + rotationChange, rotationDuration).OnComplete(() =>
            {
                interactable = true;
                doorCollider.enabled = true;
            }).SetEase(rotationEase);
        }
    }

    public void Activate()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.green;
    }

    public void Deactivate()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    public string GetName()
    {
        return "door";
    }

    public string GetActionName()
    {
        return doorOpen ? "close" : "open";
    }
}
