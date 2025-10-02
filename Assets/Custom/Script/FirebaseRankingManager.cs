using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using SimpleJSON;
using System.Linq;
public class FirebaseRankingManager : MonoBehaviour
{
    public const string url = "https://leaderboard-de45b-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public const string secret = "UnffgJaMjlwLDbaSQIsXneqFnXyY6xJNI5Tq24yc";
    
    [Header("Main")]
    [SerializeField] private RankUIManager rankUIManager;

    [System.Serializable]
    public class Ranking
    {
        public List<PlayerData> playerDatas = new List<PlayerData>();
    }

    public Ranking ranking;
    
    [Header("New Data")]
    public PlayerData currentPlayerData;
    
    private List<PlayerData> sortPlayerDatas =  new List<PlayerData>();

    [Header("Test")] public int testNum;

    [System.Serializable]
    public class TestData
    {
        public int num = 1;
        public string name = "Name";
    }
    public TestData testData =  new TestData();

    private void Start()
    {
        DebugSetupWithLocaldata();
        
        TestSetData();
        TestGetData();
        /*TestSetData2();
        TestGetData2();*/
        
        SetLocalDataToDatabase();
        //AddData();
        ReloadSortingData();
    }

    public void CalculateRankFromScore()
    {
        sortPlayerDatas = ranking.playerDatas.OrderByDescending(data => data.playerScore).ToList();
        sortPlayerDatas.ForEach(data => data.ranknumber = sortPlayerDatas.IndexOf(data) + 1);
        ranking.playerDatas = sortPlayerDatas;
    }
    
    public void DebugSetupWithLocaldata()
    {
        ranking.playerDatas = rankUIManager.playerDatas;
        CalculateRankFromScore();
    }

    #region Test Function

    public void TestSetData()
    {
        string urlData = $"{url}.json?auth={secret}";
        RestClient.Put<TestData>(urlData, testData).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log($"error on set to server {error}");
        });
    }
    public void TestSetData2()
    {
        string urlData = $"{url}TestData.json?auth={secret}";
        RestClient.Put<TestData>(urlData, testData).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log($"error on set to server {error}");
        });
    }
    public void TestGetData()
    {
        string urlData = $"{url}.json?auth={secret}";
        RestClient.Get(urlData). Then (response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response. Text);
            testNum = jsonNode ["num"];
        }).Catch(error=>
        {
            Debug.Log(error);
        });
    }
    public void TestGetData2()
    {
        string urlData = $"{url}TestData.json?auth={secret}";
        RestClient.Get(urlData). Then (response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response. Text);
            testNum = jsonNode ["num"];
        }).Catch(error=>
        {
            Debug.Log(error);
        });
    }

    #endregion
    
    public void SetLocalDataToDatabase()
    {
        string urlData = $"{url}ranking.json?auth={secret}";
        RestClient.Put<Ranking>(urlData, ranking).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log(error.ToString());
        });
    }

    public void AddData()
    {
        string urlData = $"{url}ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);
            string urlPlayerData = $"{url}ranking/playerDatas/{jsonNode.Count}.json?auth={secret}";
            RestClient.Put<PlayerData>(urlPlayerData, currentPlayerData).Then(response =>
            {
                Debug.Log("Upload Player Data Complete");
            }).Catch(error => { Debug.Log(error.ToString()); });
        }).Catch(error => { Debug.Log(error.ToString()); });
    }
    
    public void FindYourDataInRanking()
    {
        rankUIManager.youRankData.playerData = ranking.playerDatas.Where(data => data.playerName == currentPlayerData.playerName).FirstOrDefault();
        rankUIManager.youRankData.UpdateData();
    }

    public void ReloadSortingData()
    {
        string urlData = $"{url}ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);
            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData(jsonNode[i]["rankNumber"],
                    jsonNode[i]["playerName"], jsonNode[i]["playerScore"]));
            }
            CalculateRankFromScore();
            string urlPlayerData = $"{url}ranking.json?auth={secret}";
            RestClient.Put<Ranking>(urlPlayerData, ranking).Then(response =>
            {
                Debug.Log("Upload Data Complete");
                rankUIManager.playerDatas = ranking.playerDatas;
                rankUIManager.ReloadRankdata();
                FindYourDataInRanking();
            }).Catch(error =>
            {
                Debug.Log(error);
            });
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }

    public void AddDataWithSorting()
    {
        string urlData = $"{url}ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);
            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData (jsonNode[i]["rankNumber"],
                    jsonNode[i]["playerName"], jsonNode[i]["playerScore"]));
            }
            PlayerData checkPlayerData = ranking.playerDatas.FirstOrDefault(data => data.playerName == currentPlayerData.playerName);
            if (checkPlayerData != null)
            {
                checkPlayerData.playerScore = currentPlayerData.playerScore;
            }
            else
            {
                ranking.playerDatas.Add(currentPlayerData);
            }
            CalculateRankFromScore();
            string urlPlayerData = $"{url}ranking.json?auth={secret}";
            RestClient.Put<Ranking>(urlPlayerData, ranking).Then(response =>
            {
                Debug.Log("Upload Data Complete");
                rankUIManager.playerDatas = ranking.playerDatas;
                rankUIManager.ReloadRankdata();
            }).Catch(error =>
            {
                Debug.Log(error);
            });
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
}
