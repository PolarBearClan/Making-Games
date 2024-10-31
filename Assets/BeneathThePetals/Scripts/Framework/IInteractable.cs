public interface IInteractable
{
    void Interact();

    bool IsInteractable()
    {
        return true;
    }
    void Activate();
    void Deactivate();
    string GetName();
    string GetActionName();

    string GetActionType()
    {
        return "Press";
    }
}