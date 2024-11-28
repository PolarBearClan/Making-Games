using UnityEngine;

public class BreakoutWindow : MonoBehaviour
{
    [SerializeField] private string itemName = "Bus Key";

    private Collider windowCollider;
    private InteractableLight interactableLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        windowCollider = GetComponent<Collider>();
        windowCollider.enabled = false;

        interactableLight = GetComponentInChildren<InteractableLight>();
        interactableLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(InventoryManager.Instance.inventoryItems.Contains(itemName))
        {
            windowCollider.enabled = true;
            interactableLight.gameObject.SetActive(true);
        }
    }
}
