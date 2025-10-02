using UnityEngine;
using UnityEngine.UI;

public class UIBoosterManager : MonoBehaviour
{
    public static UIBoosterManager Instance;
    
    private float boostertimeRemaining;
    private float boosterDuration;
    private bool isBoosted = false;
    
    public Image boosterRemaining;
    private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();

        if (boosterRemaining != null)
        {
            boosterRemaining.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerController.GetIsGameOver())
        {
            return;
        }

        if (isBoosted)
        {
            boostertimeRemaining -= Time.deltaTime;
            boosterRemaining.fillAmount = boostertimeRemaining / boosterDuration;

            if (boostertimeRemaining <= 0)
            {
                isBoosted = false;
                boosterRemaining.gameObject.SetActive(false);
            }
        }
    }

    public void ShowBooster(float duration)
    {
        boosterDuration = duration;
        boostertimeRemaining = duration;
        isBoosted = true;
        boosterRemaining.gameObject.SetActive(true);
    }

    public void HideBooster()
    {
        isBoosted = false;
        boosterRemaining.gameObject.SetActive(false);
    }
}