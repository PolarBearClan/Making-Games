using UnityEngine;
public class DoorOpener : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider c) {

            DoorController door;
            KillDoorController door2;
            if (c.TryGetComponent<DoorController>(out door))
            {
            door.Interact();
            }
            if (c.TryGetComponent<KillDoorController>(out door2))
            {
            door2.Interact();
            } 
    
    }
    // Update is called once per frame
    void OnTriggerExit(Collider c) {

        DoorController door;
        KillDoorController door2;
            if (c.TryGetComponent<DoorController>(out door))
            {
            door.Interact();
            } 
            if (c.TryGetComponent<KillDoorController>(out door2))
            {
            door2.Interact();
            } 
    
    }
    void Update()
    {
        
    }
}
