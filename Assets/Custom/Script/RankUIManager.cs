using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RankUIManager : MonoBehaviour
{
    public GameObject rankDataPrefab;
    public Transform rankPanel;

    public List<PlayerData> playerDatas = new List<PlayerData>();
    public List<GameObject> createPlayerDatas = new List<GameObject>();
    
    public RankData youRankData;

    private void Start()
    {
        CreateRankData();
    }

    public void CreateRankData()
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            GameObject rankObj = Instantiate(rankDataPrefab, rankPanel);
            RankData rankData = rankObj.GetComponent<RankData>();
            rankData.playerData = new PlayerData(
                playerDatas[i].ranknumber, 
                playerDatas[i].playerName, 
                playerDatas[i].playerScore
            );

            rankData.UpdateData();
            createPlayerDatas.Add(rankObj);
        }
    }

    private void SortRankData()
    {
        List<PlayerData> sortRankPlayer = new List<PlayerData>();
        sortRankPlayer = playerDatas.OrderByDescending(data => data.playerScore).ToList();

        for (int i = 0; i < sortRankPlayer.Count; i++)
        {
            PlayerData changeRankNum = sortRankPlayer[i];
            changeRankNum.ranknumber = i + 1;

            sortRankPlayer[i] = changeRankNum;
        }

        playerDatas = sortRankPlayer;
    }

    private void ClearRankData()
    {
        foreach (GameObject createdData in createPlayerDatas) 
        {
            Destroy(createdData);
        }

        createPlayerDatas.Clear();
    }

    [ContextMenu("Reload")]
    public void ReloadRankdata()
    {
        ClearRankData();
        SortRankData();
        CreateRankData();
    }
}
