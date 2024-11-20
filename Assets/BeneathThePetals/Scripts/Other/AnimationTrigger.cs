using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private NPCBaseController npcBaseController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(anim != null)
                anim.SetTrigger("Open");
            if (npcBaseController != null)
                npcBaseController.Interact();

            transform.gameObject.SetActive(false);
        }
    }
}
