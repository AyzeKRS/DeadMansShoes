using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour
{
    #region Variables
    [Header("Gun parameters")]
    [SerializeField] private GameObject     bullet;
    private Animator                        anim;
    private ParticleSystem                  part;
    private TrailRenderer                   tracer;
    private Outline                         outline;

    [SerializeField] private int            current_ammo                = 0;
    [SerializeField] private int            total_ammo                  = 0;
    [SerializeField] private int            clip_capacity               = 0;

    [SerializeField] private float          empty_reload_timer          = 0.0f;
    [SerializeField] private float          early_reload_timer          = 0.0f;
    [SerializeField] private float          cooldown_timer              = 0.0f;
    private float                           throw_timer                 = 0.0f;

    [SerializeField] private bool           no_total                    = true;
    [SerializeField] private bool           can_shoot                   = true;

    [SerializeField] private Transform      reset_point;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        anim    = GetComponentInChildren<Animator>();
        part    = GetComponentInChildren<ParticleSystem>();
        tracer  = GetComponent<TrailRenderer>();
        outline = transform.GetChild(2).GetComponent<Outline>();
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

            outline.enabled = false;
            tracer.enabled  = false;
            if (current_ammo != 0)
                ShootGun();

            if (Input.GetButtonDown("Reload") && can_shoot)
            {
                if (current_ammo != clip_capacity && total_ammo != 0)
                    StartCoroutine(Reload(true));
            }
        }
        else
            if (!tracer.enabled)
                StartCoroutine(ThrowTimer());
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Reset")
        {
            transform.position = reset_point.position;
        }
    }

        #endregion

    #region Custom Functions
    private void ShootGun()
    {
        if (Input.GetButtonDown("Shoot") && can_shoot)
        {
            Transform _bullet = Instantiate(bullet.transform, transform.GetChild(1).position, Quaternion.identity);
            _bullet.transform.rotation = Camera.main.transform.rotation;

            StartCoroutine(ShootCooldown());

            if (current_ammo == 0 && total_ammo != 0)
                StartCoroutine(Reload(false));

            Audio.Instance.Play2DSound("Pistol");
        }
    }

    private void CalculateReload()
    {
        int temp = 0;

        if (!no_total)
        {
            if (total_ammo > 0)
            {
                if (total_ammo < clip_capacity)
                {
                    temp = clip_capacity - current_ammo;

                    if (temp > total_ammo)
                        temp = total_ammo;

                    current_ammo += temp;
                    total_ammo -= temp;

                    if (total_ammo < 0)
                        total_ammo = 0;
                }
                else
                {
                    temp = clip_capacity - current_ammo;
                    current_ammo += temp;
                    total_ammo -= temp;
                }
            }
        }
        else
            current_ammo = clip_capacity;
    }

    private IEnumerator Reload(bool early_reload)
    {
        yield return new WaitUntil(() => can_shoot);

        can_shoot = false;

        if (early_reload)
        {
            anim.SetTrigger("ShortReload");
            yield return new WaitForSeconds(early_reload_timer);
        }
        else
        {
            anim.SetTrigger("LongReload");
            yield return new WaitForSeconds(empty_reload_timer);
        }

        CalculateReload();

        can_shoot = true;
    }

    private IEnumerator ShootCooldown()
    {
        can_shoot = false;
        current_ammo--;

        part.Emit(1);
        anim.SetTrigger("HasShot");

        yield return new WaitForSeconds(cooldown_timer);

        can_shoot = true;
    }

    private IEnumerator ThrowTimer()
    {
        yield return new WaitForSeconds(throw_timer);
        outline.enabled = true;
        tracer.enabled  = true;
    }
    #endregion
}
