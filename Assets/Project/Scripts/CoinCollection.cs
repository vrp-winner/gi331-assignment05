using System.IO;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int coin;
    private int highCoin;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI highCoinText;
    
    void Start()
    {
        // โหลดข้อมูล highCoin ตอนเริ่มเกม
        highCoin = SaveSystem.LoadHighCoin().highCoin;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coin.ToString("D2");
        if (highCoinText != null)
            highCoinText.text = "COIN " + highCoin.ToString("D2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            UpdateUI();
            // อัปเดต high coin ถ้าทำลายสถิติ
            if (coin > highCoin)
            {
                highCoin = coin;
                SaveSystem.SaveHighCoin(highCoin);
                UpdateUI();
            }
            other.gameObject.SetActive(false);
        }
    }
    
    [System.Serializable]
    public class CoinData
    {
        public int highCoin;
    }

    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/highcoin.json";

        public static void SaveHighCoin(int value)
        {
            CoinData data = new CoinData();
            data.highCoin = value;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        public static CoinData LoadHighCoin()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<CoinData>(json);
            }
            return new CoinData();
        }
    }
}