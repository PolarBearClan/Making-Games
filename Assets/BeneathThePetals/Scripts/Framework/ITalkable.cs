public interface ITalkable : IInteractable
{
    string IInteractable.GetActionName()
    {
        return "talk to";
    }
}