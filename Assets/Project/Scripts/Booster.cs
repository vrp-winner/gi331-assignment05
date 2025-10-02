using UnityEngine;

public class Booster : MonoBehaviour
{
    public float boostForce = 10f;
    public float boostDuration = 5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ActivateBooster(boostForce, boostDuration);
                gameObject.SetActive(false);
            }
        }
    }
}