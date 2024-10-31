using UnityEngine;

public class NPCMovement_example : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public bool IsIdle = true;

    [SerializeField]
    Animator animator;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsIdle", IsIdle);
    }
}
