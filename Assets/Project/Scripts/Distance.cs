using System.IO;
using TMPro;
using UnityEngine;

public class Distance : MonoBehaviour
{
    private float score;
    private int highScore;
    
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI highDistanceText;
    
    private PlayerController playerController;
    
    void Start()
    {
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();
        
        // โหลด high score ตอนเริ่มเกม
        highScore = SaveSystem.LoadHighScore().highDistance;

        if (highDistanceText != null)
            highDistanceText.text = "HI " + highScore.ToString("D5");
    }

    void Update()
    {
        if (!playerController.GetIsGameOver())
        {
            score += playerController.GetCurrentSpeed() * Time.deltaTime;
            
            int currentDistance = Mathf.RoundToInt(score);
            distanceText.text = currentDistance.ToString("D5");
            
            // ถ้าทำลายสถิติ → เซฟใหม่ + อัปเดต UI
            if (currentDistance > highScore)
            {
                highScore = currentDistance;
                SaveSystem.SaveHighScore(highScore);
                if (highDistanceText != null)
                    highDistanceText.text = "HI " + highScore.ToString("D5");
            }
        }
    }
    
    [System.Serializable]
    public class HighScoreData
    {
        public int highDistance;
    }

    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/highscore.json";

        public static void SaveHighScore(int distance)
        {
            HighScoreData data = new HighScoreData();
            data.highDistance = distance;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        public static HighScoreData LoadHighScore()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<HighScoreData>(json);
            }
            return new HighScoreData();
        }
    }
}