using UnityEngine;

public enum States
{
    FROZEN,
    ROTATE,
    MOVE
}

public class FreezePlayerStates : MonoBehaviour
{
    #region Change movement state
    public void ChangeState(States s)
    {
        switch (s)
        {
            case States.FROZEN:
                transform.GetChild(1).GetComponent<CameraRotation>().CanLook = false;
                GetComponent<Movement>().CanMove                             = false;
                transform.GetChild(1).GetComponent<CameraBob>().CanBob       = false;
                break;

            case States.ROTATE:
                transform.GetChild(1).GetComponent<CameraRotation>().CanLook = true;
                GetComponent<Movement>().CanMove                             = false;
                transform.GetChild(1).GetComponent<CameraBob>().CanBob       = false;
                break;

            case States.MOVE:
                transform.GetChild(1).GetComponent<CameraRotation>().CanLook = true;
                GetComponent<Movement>().CanMove                             = true;
                transform.GetChild(1).GetComponent<CameraBob>().CanBob       = true;
                break;
        }
    }
    #endregion
}
