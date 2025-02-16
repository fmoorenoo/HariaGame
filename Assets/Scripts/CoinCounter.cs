using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public AudioSource oneCoinLeftSound;
    public GameObject wetFloor;
    public GameObject water;
    
    private int totalCoins = 5;
    private int currentCoins = 0;
    private bool soundPlayed = false;
    private bool hasActivatedFeatures = false; 

    void Start()
    {
        if (wetFloor != null) wetFloor.SetActive(false);
        if (water != null) water.SetActive(false);
    }

    public void AddCoin()
    {
        currentCoins++;
        coinText.text = currentCoins + "/" + totalCoins;

        if (currentCoins == totalCoins - 1)
        {
            if (!soundPlayed && oneCoinLeftSound != null)
            {
                oneCoinLeftSound.Play();
                soundPlayed = true;
            }

            if (!hasActivatedFeatures)
            {
                if (wetFloor != null) wetFloor.SetActive(true);
                if (water != null) water.SetActive(true);

                hasActivatedFeatures = true;
            }
        }
    }

    public bool HasCollectedAllCoins()
    {
        return currentCoins >= totalCoins;
    }
}
