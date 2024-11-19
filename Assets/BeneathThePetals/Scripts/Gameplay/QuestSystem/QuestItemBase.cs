using UnityEngine;

public abstract class QuestItemBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string itemName;
    [SerializeField] protected string actionName;

    protected bool isActive = false;

    protected PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.ActivateQuestItemsCallback += () => { isActive = true; };
    }

    public abstract void Interact();
    public void PlayInteractSound()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Activate();
    public abstract void Deactivate();

    public virtual string GetName() => itemName;
    public virtual string GetActionName() => actionName;
    public virtual bool IsInteractable() => isActive;

    public void DeactivateItem() => isActive = false;
}
