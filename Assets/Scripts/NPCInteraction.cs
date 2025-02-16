using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactionTextUI;
    public AudioSource audioSource;

    public AudioClip initialAudio;
    public AudioClip notEnoughCoinsAudio;
    public AudioClip allCoinsCollectedAudio;

    public string initialSubtitles = "A ver... Me vas a tener que traer todas las monedas del patio. Y no tengo todo el día.";
    public string notEnoughCoinsSubtitles = "Todavía te faltan monedas, no me hagas perder el tiempo.";
    public string allCoinsCollectedSubtitles = "¡Muy bien! Has recogido todas las monedas, ahora sigue adelante.";
    public bool HasKey { get; private set; } = false;

    private bool isPlayerNear = false;
    private bool isAudioPlaying = false;
    private bool hasActivatedFeatures = false;
    private bool hasCompletedFinalDialogue = false; 

    private GameObject subtitlesUI;
    private TextMeshProUGUI tmpTextComponent;

    public GameObject timerUI;
    public GameObject coinCounterUI;
    public CoinCounter coinCounterScript;
    public GameObject[] coins;
    
    public Image keyImageUI; 

    private bool hasTalkedBefore = false; 

    void Start()
    {
        if (interactionTextUI != null)
            interactionTextUI.SetActive(false);

        subtitlesUI = GameObject.FindGameObjectWithTag("Subtitles");

        if (subtitlesUI != null)
        {
            subtitlesUI.SetActive(false);
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

        if (keyImageUI != null)
        {
            keyImageUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && !isAudioPlaying && Input.GetKeyDown(KeyCode.E))
        {
            PrepareFinalDialogue(); 
            PlayDialogue(); 
        }
    }

    void PrepareFinalDialogue()
    {
        if (coinCounterScript != null && coinCounterScript.HasCollectedAllCoins())
        {
            hasCompletedFinalDialogue = true; 

            if (timerUI != null)
            {
                Timer timerScript = timerUI.GetComponent<Timer>();
                if (timerScript != null)
                {
                    timerScript.StopTimer();
                }
            }

            if (timerUI != null) timerUI.SetActive(false);
            if (coinCounterUI != null) coinCounterUI.SetActive(false);
        }
    }

    void PlayDialogue()
    {
        if (audioSource == null) return;

        isAudioPlaying = true;
        interactionTextUI.SetActive(false);

        if (subtitlesUI != null)
        {
            subtitlesUI.SetActive(true);
        }

        if (!hasTalkedBefore)
        {
            if (initialAudio != null)
            {
                audioSource.clip = initialAudio;
                audioSource.Play();
                UpdateSubtitles(initialSubtitles);
            }
            hasTalkedBefore = true;
        }
        else if (coinCounterScript != null && coinCounterScript.HasCollectedAllCoins())
        {
            if (allCoinsCollectedAudio != null)
            {
                audioSource.clip = allCoinsCollectedAudio;
                audioSource.Play();
                UpdateSubtitles(allCoinsCollectedSubtitles);
            }
            
            HasKey = true;

            if (keyImageUI != null)
            {
                keyImageUI.gameObject.SetActive(true);
            }
        }
        else
        {
            if (notEnoughCoinsAudio != null)
            {
                audioSource.clip = notEnoughCoinsAudio;
                audioSource.Play();
                UpdateSubtitles(notEnoughCoinsSubtitles);
            }
        }
    }

    void UpdateSubtitles(string text)
    {
        if (tmpTextComponent != null)
        {
            tmpTextComponent.text = text;
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

                hasActivatedFeatures = true;
            }
        }
    }
}
