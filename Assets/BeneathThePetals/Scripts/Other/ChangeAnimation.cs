using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetWalking(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);
    }
}
