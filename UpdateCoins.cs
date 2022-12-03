using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UpdateCoins : MonoBehaviour
{
    
    private void OnEnable()
    {
        UpdateTheFuckingCoins();
    }

    private void UpdateTheFuckingCoins()
    {
        PlayerDataStorer.LoadProgress();
        GetComponent<TextMeshProUGUI>().text = PlayerDataStorer.totalcoins.ToString("0");
    }
    public void UpdateCurrentCoins()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerDataStorer.totalcoins.ToString("0"); 
    }
}
