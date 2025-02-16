using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;
    private bool isCollected = false;
    private Vector3 startPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * 2f) * 0.3f, 0);
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            CoinCounter coinCounter = FindObjectOfType<CoinCounter>();
            if (coinCounter != null)
            {
                coinCounter.AddCoin();
            }

            if (audioSource != null && pickupSound != null)
            {
                Debug.Log("Playing pickup sound.");
                audioSource.PlayOneShot(pickupSound);
                StartCoroutine(DisableAfterSound(pickupSound.length));
            }
        }
    }

    private IEnumerator DisableAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
