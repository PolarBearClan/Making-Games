using UnityEngine;

public class GenericScriptForDisablingObjects : MonoBehaviour
{

    public GameObject objectToDisable;
    public GameObject objectToEnable;

   public void OnTriggerEnter(Collider c) { 
    
    objectToDisable.SetActive(false);
    objectToEnable.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
