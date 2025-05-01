using UnityEngine;
using UnityEngine.UI;

public class UIBoosterManager : MonoBehaviour
{
    public static UIBoosterManager Instance;
    
    private float boostertimeRemaining;
    private float boosterDuration;
    private bool isBoosted = false;
    
    public Image boosterRemaining;
    private PlayerController playerController; // Reference ไปที่ PlayerController

    private void Awake()
    {
        // (ถ้ามี Instance อยู่แล้วให้ทำลายตัวใหม่)
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
        // ค้นหา "Car" PlayerController มาใช้
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();

        if (boosterRemaining != null)
        {
            boosterRemaining.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // ถ้าเกมจบแล้วไม่ต้องทำอะไรต่อ
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