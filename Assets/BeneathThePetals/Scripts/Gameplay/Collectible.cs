using UnityEngine;

public class Collectible : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string collectibleName;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float rotationAmount = 360f;

    private bool isCollected = false;

    private GameObject player;
    private FirstPersonController firstPersonController;
    private PlayerController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollected)
        {
            firstPersonController.DisableInput();
            if (GetComponent<BoxCollider>())
            {
                Destroy(GetComponent<BoxCollider>());
            }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.up, rotationAmount * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            playerController.AddToInventory(collectibleName);
            firstPersonController.EnableInput();
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        isCollected = true;
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
        return collectibleName;
    }

    public string GetActionName()
    {
        return "collect";
    }
}
