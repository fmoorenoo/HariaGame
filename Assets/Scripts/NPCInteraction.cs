using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactionTextUI;
    public AudioSource audioSource;
    public string subtitlesText = "A ver... Me vas a tener que traer todas las monedas del patio. Y no tengo todo el día.";

    private bool isPlayerNear = false;
    private bool isAudioPlaying = false;
    private bool hasActivatedFeatures = false; 
    private GameObject subtitlesUI;
    private Text uiTextComponent;
    private TextMeshProUGUI tmpTextComponent;

    public GameObject timerUI;
    public GameObject coinCounterUI;
    public CoinCounter coinCounterScript;
    public GameObject[] coins;

    public GameObject wetFloor;
    public GameObject water;

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

        if (timerUI != null) timerUI.SetActive(false);
        if (coinCounterUI != null) coinCounterUI.SetActive(false);

        if (coins != null)
        {
            foreach (GameObject coin in coins)
            {
                coin.SetActive(false);
            }
        }

        if (wetFloor != null) wetFloor.SetActive(false);
        if (water != null) water.SetActive(false);
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

            if (!hasActivatedFeatures) 
            {
                if (timerUI != null)
                {
                    timerUI.SetActive(true);
                    timerUI.GetComponent<Timer>().StartTimer();
                }

                if (coinCounterUI != null)
                {
                    coinCounterUI.SetActive(true);
                }

                if (coins != null)
                {
                    foreach (GameObject coin in coins)
                    {
                        coin.SetActive(true);
                    }
                }

                if (wetFloor != null)
                {
                    wetFloor.SetActive(true);
                }

                if (water != null)
                {
                    water.SetActive(true);
                }

                hasActivatedFeatures = true; 
            }
        }
    }
}
