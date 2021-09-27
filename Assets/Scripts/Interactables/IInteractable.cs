public interface IInteractable
{
    void Interact();        // Interact; what happens when the player does a thing?
    string DisplayPrompt(); // Press E to interact? Return a string to be more specific
}
