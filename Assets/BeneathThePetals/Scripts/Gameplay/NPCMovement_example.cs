using UnityEngine;

public class NPCMovement_example : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public bool IsIdle = true;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform destination;

    [SerializeField]
    float speed;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsIdle", IsIdle);
        if (!IsIdle)
        {
            transform.LookAt(new Vector3(destination.position.x, 0, destination.position.z));
            Vector3 vector3 = new Vector3(destination.position.x, transform.position.y, destination.position.z);
            this.transform.position = Vector3.Lerp(transform.position, vector3, speed * Time.deltaTime);
        }
    }
}
