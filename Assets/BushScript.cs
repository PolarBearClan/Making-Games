using UnityEngine;

public class BushScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player" && collider.gameObject.GetComponent<FirstPersonController>().isCrouched)
        {
            collider.gameObject.GetComponent<FirstPersonController>().isHiding = true; 
        } 
    }

    void OnTriggerStay(Collider collider)
    {

        if (collider.gameObject.name == "Player" && collider.gameObject.GetComponent<FirstPersonController>().isCrouched)
        {
            collider.gameObject.GetComponent<FirstPersonController>().isHiding = true;
        }
        else if (collider.gameObject.name == "Player" && !collider.gameObject.GetComponent<FirstPersonController>().isCrouched) 
        { 
            collider.gameObject.GetComponent<FirstPersonController>().isHiding = false;
        }


    }

    void OnTriggerExit(Collider collider) 
    { 
        if (collider.gameObject.name == "Player")
        {
            collider.gameObject.GetComponent<FirstPersonController>().isHiding = false; 
        } 
    
    }
}
