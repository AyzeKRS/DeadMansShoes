using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Keycodes
    [Header("Move controls")]
    [SerializeField] private KeyCode move_forwards  = KeyCode.W;
    [SerializeField] private KeyCode move_backwards = KeyCode.S;
    [SerializeField] private KeyCode move_left      = KeyCode.A;
    [SerializeField] private KeyCode move_right     = KeyCode.D;

    [SerializeField] private KeyCode jump           = KeyCode.Space;
    [SerializeField] private KeyCode sprint         = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouch         = KeyCode.LeftControl;

    [Header("Action controls")]
    [SerializeField] private KeyCode attack         = KeyCode.Mouse0;
    #endregion

    public Vector2 MouseAxisRaw()
    {
        Vector2 mar =
            new Vector2
            (
                Input.GetAxisRaw("Mouse X"),
                Input.GetAxisRaw("Mouse Y")
            );

        return mar;
    }

    public Vector2 MovementDirectionRawInput()
    {
        Vector2 md =
            new Vector2
            (
                Input.GetAxisRaw("Vertical"),
                Input.GetAxisRaw("Horizontal")
            );

        md.Normalize();
        return md;
    }

    public Vector2 MovementDirectionInput()
    {
        Vector2 md =
            new Vector2
            (
                Input.GetAxis("Vertical"),
                Input.GetAxis("Horizontal")
            );

        return md;
    }

    public Vector2 MovementDirectionInputClamped()
    {
        Vector2 md =
            new Vector2
            (
                Input.GetAxis("Vertical"),
                Input.GetAxis("Horizontal")
            );

        md = Vector2.ClampMagnitude(md, 1);

        return md;
    }

    public bool Jump    { get { return Input.GetKeyDown(jump); } }

    public bool Fire    { get { return Input.GetKeyDown(attack); } }

    public bool Sprint  { get { return Input.GetKey(sprint); } }

    public bool Crouch  { get { return Input.GetKeyDown(crouch); } }
}
