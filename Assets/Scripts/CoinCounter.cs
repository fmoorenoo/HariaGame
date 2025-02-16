using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public AudioSource oneCoinLeftSound;
    private int totalCoins = 5;
    private int currentCoins = 0;
    private bool soundPlayed = false; 

    public void AddCoin()
    {
        currentCoins++;
        coinText.text = currentCoins + "/" + totalCoins;

        if (currentCoins == totalCoins - 1 && !soundPlayed)
        {
            if (oneCoinLeftSound != null)
            {
                oneCoinLeftSound.Play();
                Debug.Log("Reproduciendo sonido de última moneda.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado un sonido en `oneCoinLeftSound`.");
            }

            soundPlayed = true; 
        }
    }
}
