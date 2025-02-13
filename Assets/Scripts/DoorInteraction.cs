using UnityEngine;
using UnityEngine.UI; // Necesario si usas un UI Text

public class DoorInteraction : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(100, 1, 100);
    public GameObject player;
    public GameObject interactionTextUI;

    private bool isPlayerNear = false;

    void Start()
    {
        if (interactionTextUI != null)
            interactionTextUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    player.transform.position = teleportPosition;
                    cc.enabled = true;
                }
                else
                {
                    player.transform.position = teleportPosition;
                }

                if (interactionTextUI != null)
                    interactionTextUI.SetActive(false);

                isPlayerNear = false;
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
            if (interactionTextUI != null)
                interactionTextUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            if (interactionTextUI != null)
                interactionTextUI.SetActive(false);
        }
    }
}
