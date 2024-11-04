using UnityEngine;

public class NPCWalkAround : MonoBehaviour
{

    Vector3 movementVector;
    Vector3 rotationVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementVector = new Vector3(1, 0 ,0);
        rotationVector = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementVector);
        transform.Rotate(rotationVector);
    }
}
