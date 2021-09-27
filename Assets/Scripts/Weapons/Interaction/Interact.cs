using UnityEngine;
using System.Collections;
using TMPro;

public class Interact : MonoBehaviour
{
    #region Variables
    [SerializeField] private Camera             cam;
    [SerializeField] private TMP_Text           text;
    private ItemHolder                          ih;

    [Header("Interact Parameters")]
    [SerializeField] private float              interact_range      = 0.0f;
    [SerializeField] private float              pick_up_rot         = 0.0f;
    [SerializeField] private float              pick_up_speed       = 0.0f;
    [SerializeField] private float              pick_up_timer       = 0.0f;
    private float                               timer               = 0.0f;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        ih = GetComponent<ItemHolder>();
    }

    private void Update()
    {
        GetInteract();
    }
        #endregion

    #region Custom Functions
    private void GetInteract()
    {
        text.text = "";

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, interact_range))
        {
            switch (hit.collider.tag)
            {
                case "Interactable":
                    hit.transform.TryGetComponent(out IInteractable interactable);
                    text.text = interactable.DisplayPrompt();

                    if (Input.GetButtonDown("Interact"))
                        interactable.Interact();

                    break;

                case "Button":
                    text.text = "Press E to press button";

                    if (Input.GetButtonDown("Interact"))
                        hit.transform.GetComponent<KeyNumber>().SendKey();

                    break;

                case "Item":
                    if (hit.collider.isTrigger && !ih.item_held)
                    {
                        text.text = "Press E to pick up";

                        if (Input.GetButtonDown("Interact"))
                            PickUpItem(hit);
                    }
                    else
                        text.text = "";

                    break;
            }
        }
    }

    private void PickUpItem(RaycastHit hit)
    {
        ih.item = hit.transform.gameObject;

        hit.transform.GetComponentInParent<Rigidbody>().useGravity          = false;
        hit.transform.GetComponentInParent<Rigidbody>().isKinematic         = true;
        hit.transform.GetComponentInParent<Collider>().enabled              = false;

        hit.transform.parent                                                = transform;

        Audio.Instance.Play2DSound("PickUp");

        StartCoroutine(TransformItem(hit));
    }

    private IEnumerator TransformItem(RaycastHit hit)
    {
        ih.being_picked_up = true;

        while (timer < pick_up_timer)
        {
            timer += Time.deltaTime;
            Vector3 vel = Vector3.zero;
            
            hit.transform.localPosition = Vector3.SmoothDamp
                (
                hit.transform.localPosition,
                Vector3.zero,
                ref vel,
                1 / pick_up_speed * Time.deltaTime
                );

            hit.transform.localRotation = Quaternion.Slerp
                (
                hit.transform.localRotation,
                Quaternion.Euler(0, 0, 0),
                pick_up_rot * Time.deltaTime
                );

            yield return new WaitForEndOfFrame();
        }

        timer                           = 0.0f;
        hit.collider.enabled            = false;
        hit.transform.localPosition     = Vector3.zero;
        hit.transform.localRotation     = Quaternion.Euler(0, 0, 0);
        ih.being_picked_up              = false;
    }
    #endregion
}
