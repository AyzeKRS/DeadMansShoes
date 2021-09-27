using UnityEngine;

public class ActivatePC : MonoBehaviour
{
    [SerializeField] private GameObject     pc;
    [SerializeField] private Outline        outline;
    [SerializeField] private GameObject[]   buttons;
    [SerializeField] private GameObject     button;
    public bool                             pc_enabled      = false;
    public bool                             has_component   = false;


    private void OnTriggerEnter(Collider o)
    {
        if (o.tag == "Player")
        {
            if (!pc_enabled)
            {
                pc_enabled      = true;
                pc.tag          = "Interactable";
                outline.enabled = true;
                GetComponent<Collider>().enabled = false;
            }

            if (has_component)
            {
                has_component = false;

                button.SetActive(true);

                foreach (GameObject button in buttons)
                    button.tag = "Interactable";
            }
        }
    }
}
