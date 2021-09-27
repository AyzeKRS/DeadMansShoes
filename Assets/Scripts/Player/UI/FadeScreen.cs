using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    #region Variables
    private Animator                    anim;
    [SerializeField] private bool       immediate_fade      = true;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (immediate_fade)
            FadeIn();
    }
    #endregion

    #region Animation fades
    public void FadeIn()    // Fade to Clear
    {
        anim.SetTrigger("FadeIn");
    }

    public void FadeOut()   // Clear to fade
    {
        anim.SetTrigger("FadeOut");
    }

    public void Dim()       // Clear to dim
    {
        anim.SetTrigger("Dim");
    }

    public void Show()      // Dim to clear
    {
        anim.SetTrigger("Show");
    }

    public void FadeDim()    // Dim to Fade
    {
        anim.SetTrigger("FadeDim");
    }
    #endregion
}
