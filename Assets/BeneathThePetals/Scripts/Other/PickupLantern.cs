using UnityEngine;

public class PickupLantern : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 lanternPosition = new Vector3(-0.8f, -0.9f, 1f);

    private bool isPickedUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Activate()
    {
        //
    }

    public void Deactivate()
    {
        //
    }

    public string GetActionName()
    {
        return "Pick up";
    }

    public string GetActionType()
    {
        return "Press";
    }

    public string GetName()
    {
        return "Lantern";
    }

    public void Interact()
    {
        if (!isPickedUp)
        {
            Transform mainCamera = Camera.main.transform;
            transform.SetParent(mainCamera);
            transform.localPosition = lanternPosition;
            transform.localRotation = Quaternion.identity;
            isPickedUp = true;
            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponentInChildren<InteractableLight>().transform.gameObject.SetActive(false);
        }
    }

    public void PlayInteractSound()
    {
        //
    }
}
