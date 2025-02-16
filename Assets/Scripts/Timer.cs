using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public AudioSource audioSource;
    public AudioClip timeUpSound; 

    private float timeRemaining = 10f;
    private bool isRunning = false;
    private bool hasPlayedSound = false; 

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= 1f;
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
        }

        isRunning = false;
        timerText.text = "00:00";

        if (!hasPlayedSound && audioSource != null && timeUpSound != null)
        {
            audioSource.PlayOneShot(timeUpSound);
            hasPlayedSound = true;
        }
    }
}
