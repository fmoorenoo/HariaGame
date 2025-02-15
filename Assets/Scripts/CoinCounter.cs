using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private int totalCoins = 3;
    private int currentCoins = 0;

    public void AddCoin()
    {
        currentCoins++;
        coinText.text = currentCoins + "/" + totalCoins;

        if (currentCoins >= totalCoins)
        {
            Debug.Log("¡Has recolectado todas las monedas!");
        }
    }
}

