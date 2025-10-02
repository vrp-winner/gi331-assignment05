using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    /*public int ranknumber;
    public int distance;
    public int coin;

    public PlayerData(int rankNumber, int distance, int coin)
    {
        this.ranknumber = rankNumber;
        this.distance = distance;
        this.coin = coin;
    }*/
    
    public string playerName;
    public int ranknumber;
    public int playerScore;

    public PlayerData(int rankNumber, string playerName, int playerScore)
    {
        this.ranknumber = rankNumber;
        this.playerName = playerName;
        this.playerScore = playerScore;
    }
}

public class RankData : MonoBehaviour
{
    /*public PlayerData playerData;
    [Space]
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text scoreText;

    public void UpdateData()
    {
        rankText.text = playerData.ranknumber.ToString();
        distanceText.text = playerData.distance.ToString() + " m.";
        scoreText.text = playerData.coin.ToString();
    }*/
    
    public PlayerData playerData;
    [Space]
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text scoreText;

    public void UpdateData()
    {
        rankText.text = playerData.ranknumber.ToString();
        playerNameText.text = playerData.playerName;
        scoreText.text = playerData.playerScore.ToString("0");
    }
}
