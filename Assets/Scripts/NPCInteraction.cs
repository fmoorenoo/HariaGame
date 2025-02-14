using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactionTextUI;
    public AudioSource audioSource;
    public string subtitlesText = "Eres imbécil... Salpica de aquí anda.";

    private bool isPlayerNear = false;
    private bool isAudioPlaying = false;
    private GameObject subtitlesUI;
    private Text uiTextComponent;
    private TextMeshProUGUI tmpTextComponent;

    void Start()
    {
        if (interactionTextUI != null)
            interactionTextUI.SetActive(false);

        subtitlesUI = GameObject.FindGameObjectWithTag("Subtitles");

        if (subtitlesUI != null)
        {
            subtitlesUI.SetActive(false);
            uiTextComponent = subtitlesUI.GetComponent<Text>();
            tmpTextComponent = subtitlesUI.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el Tag 'Subtitles'.");
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNear && !isAudioPlaying)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    isAudioPlaying = true;
                    interactionTextUI.SetActive(false);

                    if (subtitlesUI != null)
                    {
                        subtitlesUI.SetActive(true);

                        if (uiTextComponent != null)
                        {
                            uiTextComponent.text = subtitlesText;
                        }
                        else if (tmpTextComponent != null)
                        {
                            tmpTextComponent.text = subtitlesText;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
            if (!isAudioPlaying && interactionTextUI != null)
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

    void LateUpdate()
    {
        if (isAudioPlaying && !audioSource.isPlaying)
        {
            isAudioPlaying = false;

            if (isPlayerNear && interactionTextUI != null)
                interactionTextUI.SetActive(true);

            if (subtitlesUI != null)
                subtitlesUI.SetActive(false);
        }
    }
}
