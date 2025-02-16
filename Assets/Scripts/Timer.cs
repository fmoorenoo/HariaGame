using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeRemaining = 180f;
    private bool isRunning = false;
    public Animator jugadorAnimator; 

    void Start()
    {
        if (jugadorAnimator == null)
        {
            Debug.LogError("Animator del jugador no asignado.");
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
    }
}
