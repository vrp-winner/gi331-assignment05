using UnityEngine;

public class MoveBack : MonoBehaviour
{
    public float speed = 10f;
    public float mass = 1f;
    private float currentSpeed;
    private float baseSpeed;
    private float zLimit = -15f; 
    
    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();
        currentSpeed = speed;
        baseSpeed = speed;
    }

    void Update()
    {
        if (!playerController.GetIsGameOver())
        {
            transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);
        }

        if (transform.position.z < zLimit)
        {
            gameObject.SetActive(false);
        }
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        currentSpeed = speed;
    }
}