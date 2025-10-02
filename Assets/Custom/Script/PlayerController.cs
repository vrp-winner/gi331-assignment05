using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float mass = 1f;
    
    private float currentSpeed;
    private float currentMass;
    private float boostTimeRemaining = 0f;
    private bool isBoosted = false;
    private bool isGameOver = false;
    
    private Rigidbody rb;
    private InputAction moveAction;
    public bool GetIsGameOver() => isGameOver;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        currentMass = mass;
        
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }
    private void OnEnable()
    {
        moveAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
    }
    void Update()
    {
        if(isGameOver) return;
        if (isBoosted)
        {
            boostTimeRemaining -= Time.deltaTime;
            if (boostTimeRemaining <= 0)
            {
                DeactivateBooster();   
            }
        }
        float horizontalInput = moveAction.ReadValue<Vector2>().x;
        transform.Translate(horizontalInput * currentSpeed * Time.deltaTime * Vector3.right);
        
        float xRange = 4.5f;
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            UIBoosterManager.Instance.HideBooster();
            GameManager.Instance.GameOver();
        }
    }
    public void ActivateBooster(float boostForce, float duration)
    {
        float acceleration = boostForce / currentMass;
        currentSpeed = speed + acceleration;
        isBoosted = true;
        boostTimeRemaining = duration;
        
        MoveBack[] movingObjects = FindObjectsByType<MoveBack>(FindObjectsSortMode.None);
        foreach (MoveBack obj in movingObjects)
        {
            float objAcc = boostForce / obj.GetMass();
            obj.SetSpeed(obj.GetBaseSpeed() + objAcc);
        }
        UIBoosterManager.Instance.ShowBooster(duration);
    }
    private void DeactivateBooster()
    {
        currentSpeed = speed;
        isBoosted = false;
        
        MoveBack[] movingObjects = FindObjectsByType<MoveBack>(FindObjectsSortMode.None);
        foreach (MoveBack obj in movingObjects)
        {
            obj.ResetSpeed();
        }
        UIBoosterManager.Instance.HideBooster();
    }
    public float GetCurrentSpeed() => currentSpeed;
}