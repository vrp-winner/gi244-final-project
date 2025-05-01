using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float mass = 1f;
    
    private float currentSpeed;
    private float currentMass;
    private float boostTimeRemaining = 0f;
    private bool isBoosted = false;
    private bool isGameOver = false;
    
    private Rigidbody rb;
    private InputAction moveAction;
    
    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentSpeed = speed;
        currentMass = mass;
    }

    void Update()
    {
        // ถ้าเกมจบแล้ว ไม่ต้องให้ผู้เล่นขยับ
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
        
        // จำกัดขอบเขตการเคลื่อนที่ของผู้เล่น (ไม่ให้ออกนอกจอ)
        float xRange = 4.5f; // ขอบเขตซ้าย-ขวา
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
        // ถ้าชนกับวัตถุที่มี tag "Obstacle" ให้จบเกม
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            UIBoosterManager.Instance.HideBooster();
            GameManager.Instance.GameOver();
        }
    }

    public void ActivateBooster(float boostForce, float duration)
    {
        // คำนวณความเร็วใหม่จากแรง Boost และมวล (F=ma)
        float acceleration = boostForce / currentMass;
        currentSpeed = speed + acceleration;
        isBoosted = true;
        boostTimeRemaining = duration;
        
        // หาวัตถุทั้งหมดที่มีสคริปต์ MoveBack และเพิ่มความเร็วพวกนั้นด้วย
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

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}