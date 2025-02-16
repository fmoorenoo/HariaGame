using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitDoorInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactionTextUI; 
    public NPCInteraction npcInteraction;
    public string nextSceneName = "GameMenu"; 

    private bool isPlayerNear = false;
    private TextMeshProUGUI textComponent; 
    void Start()
    {
        if (interactionTextUI != null)
        {
            textComponent = interactionTextUI.GetComponent<TextMeshProUGUI>();
            interactionTextUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && textComponent != null)
        {
            if (npcInteraction != null && npcInteraction.HasKey)
            {
                textComponent.text = "'E' para salir del instituto";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ChangeScene();
                }
            }
            else
            {
                textComponent.text = "Necesitas una llave";
            }
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
            if (interactionTextUI != null)
            {
                interactionTextUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            if (interactionTextUI != null)
            {
                interactionTextUI.SetActive(false);
            }
        }
    }
}
