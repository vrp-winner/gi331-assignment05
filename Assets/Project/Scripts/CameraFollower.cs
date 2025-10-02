using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject car;
    public Vector3 offset;
    
    void LateUpdate()
    {
        transform.position = car.transform.position + offset;
    }
}