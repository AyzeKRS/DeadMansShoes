using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    #region Variables

    #region Cursor Flags
    private bool lock_cursor = true;
    #endregion

    #endregion

    /************************************************************************************************************************************************************************************************/

    #region BuiltIn Functions

    private void Awake()
    {
        if (lock_cursor)
            HideCursor();
        else
            ShowCursor();
    }

    #endregion

    /************************************************************************************************************************************************************************************************/

    #region Custom Functions

    public void HideCursor()
    {
        Cursor.lockState    = CursorLockMode.Locked;
        Cursor.visible      = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState    = CursorLockMode.Confined;
        Cursor.visible      = true;
    }

    #endregion

    /************************************************************************************************************************************************************************************************/
}
