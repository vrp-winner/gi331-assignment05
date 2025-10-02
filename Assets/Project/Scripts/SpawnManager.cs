using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject[] obstaclePrefab;
    public GameObject boosterPrefab;
    public GameObject coinPrefab;
    
    public float startDelay = 0.1f;
    public float repeatLate = 1f;
    
    private PlayerController playerController;

    private bool isSpawning = true;
    
    void Start()
    {
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();
        
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(startDelay); // รอเวลาเริ่มต้น
        while (isSpawning)
        {
            // ถ้าเกมจบแล้ว ให้หยุด Spawn
            if (playerController.GetIsGameOver())
            {
                isSpawning = false;
                yield break; // หยุด Coroutine
            }
            
            // สุ่ม Spawn (Boost, เหรียญ, หรือสิ่งกีดขวาง)
            float randomValue = Random.value;
            int spawnIndex = Random.Range(0, spawnPoint.Length);
            
            if (randomValue < 0.1f) // (โอกาส Spawn Booster) 
            {
                Instantiate(boosterPrefab, spawnPoint[spawnIndex].position, Quaternion.identity);
            }
            else if (randomValue < 0.3f) // (โอกาส Spawn Coin) 
            {
                Instantiate(coinPrefab, spawnPoint[spawnIndex].position, Quaternion.identity);
            }
            else // (โอกาส Spawn Obstacle)
            {
                // สุ่มจุด Spawn 2 จุดที่ไม่ซ้ำกัน
                int firstSpawnIndex = Random.Range(0, spawnPoint.Length);
                int secondSpawnIndex;
                        
                do
                {
                    secondSpawnIndex = Random.Range(0, spawnPoint.Length);
                } while (secondSpawnIndex == firstSpawnIndex); // ห้ามซ้ำกับจุดแรก
                        
                // สุ่มเลือก Prefab ของ Obstacle
                int randomIndex1 = Random.Range(0, obstaclePrefab.Length);
                        
                int randomIndex2;
                if (randomValue < 0.1f) // (โอกาสใช้ Prefab เดียวกันทั้งคู่)
                {
                    randomIndex2 = randomIndex1;
                }
                else
                {
                    randomIndex2 = Random.Range(0, obstaclePrefab.Length);
                }
                        
                // สร้าง Obstacle ที่จุดแรก
                Instantiate(obstaclePrefab[randomIndex1], spawnPoint[firstSpawnIndex].position, Quaternion.identity);
                        
                // สร้าง Obstacle ที่จุดสอง
                Instantiate(obstaclePrefab[randomIndex2], spawnPoint[secondSpawnIndex].position, Quaternion.identity);
            }
            
            yield return new WaitForSeconds(repeatLate); // รอเวลาต่อไปก่อนที่จะ spawn ใหม่
        }
    }
}