using UnityEngine;
using TMPro;

public class InmovilizedSound : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public AudioSource audioSource;
    public AudioClip visibleAudioClip; 
    public Timer timer; 

    private bool hasPlayedAudio = false;

    void Update()
    {
        if (IsTextVisible(textMeshPro) && !hasPlayedAudio && !IsTimerFinished())
        {
            if (audioSource != null && visibleAudioClip != null)
            {
                audioSource.clip = visibleAudioClip;
                audioSource.Play();
                hasPlayedAudio = true; 
            }
        }
    }

    bool IsTextVisible(TextMeshProUGUI text)
    {
        return text != null && text.gameObject.activeInHierarchy && text.color.a > 0;
    }

    bool IsTimerFinished()
    {
        return timer != null && timer.timerText != null && timer.timerText.text.Trim() == "00:00";
    }
}
