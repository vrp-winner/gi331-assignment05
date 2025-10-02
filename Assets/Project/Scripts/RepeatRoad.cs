using UnityEngine;

public class RepeatRoad : MonoBehaviour
{
    private Vector3 startPosition;

    private float length;
    
    void Start()
    {
        startPosition = transform.position; // บันทึกตำแหน่งตอนเริ่มเกม
        
        length = GetComponent<BoxCollider>().size.z * 14.2f; // คำนวณความยาวของถนนจาก BoxCollider (แกน Z)
    }

    void Update()
    {
        // เช็คว่าถนนเคลื่อนที่ไปไกลเกินกว่าจุดที่กำหนดหรือไม่
        if (transform.position.z < startPosition.z - length)
        {
            transform.position = startPosition;
        }
    }
}