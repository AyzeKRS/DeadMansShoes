using System.Collections;
using UnityEngine;
using TMPro;

public class Throwable : MonoBehaviour
{
    #region Variable
    [SerializeField] private Transform      reset_point;
    private TrailRenderer                   trail;
    private Outline                         outline;

    [SerializeField] private float          reset_timer             = 0.0f;
    private float                           throw_timer             = 0.0f;

    [SerializeField] private TMP_Text       subtitle;
    public bool                             can_insult              = true;
    private bool                            insult_flag             = true;
    [SerializeField] private string[]       insults                 = { "", "", "", "", "" };
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        trail   = GetComponent<TrailRenderer>();
        outline = transform.GetChild(1).GetComponent<Outline>();
    }

    private void Update()
    {
        if (
            transform.parent != null &&
            transform.parent.GetComponent<ItemHolder>() != null
            )
        {
            if (throw_timer == 0)
                throw_timer = transform.parent.GetComponent<ItemHolder>().throw_timer;

            trail.enabled   = false;
            outline.enabled = false;
        }
            
        else
        {
            if (!trail.enabled && insult_flag)
                StartCoroutine(ThrowTimer());
        }
    }
    #endregion

    #region Throw Timer
    private IEnumerator ThrowTimer()
    {
        insult_flag = false;

        yield return new WaitForSeconds(throw_timer);

        trail.enabled   = true;
        insult_flag     = true;

        yield return new WaitForSeconds(reset_timer);

        transform.position = reset_point.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        trail.Clear();

        outline.enabled = true;

        if (can_insult)
            InsultPlayer();
            
    }

    private void InsultPlayer()
    {
        int num = Random.Range(0, 5);
        StartCoroutine(DisplayInsult(num));
    }

    private IEnumerator DisplayInsult(int num)
    {
        subtitle.text = insults[num];

        yield return new WaitForSeconds(1.5f);

        subtitle.text = string.Empty;
    }
    #endregion
}