using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject creditPanel;
    
    public Button startButton;
    public Button homeButton;
    public Button creditButton;
    public Button backButton;
    
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
        
        gameOverPanel.SetActive(false); // ซ่อน Game Over Panel ตอนเริ่มเกม
    }

    private void Start()
    {
        // ค้นหา "Car" PlayerController มาใช้
        playerController = GameObject.Find("Car").GetComponent<PlayerController>();
        
        MainMenu(); // เริ่มเกมด้วย Main Menu
        
        startButton.onClick.AddListener(StartGame);
        homeButton.onClick.AddListener(Home);
        creditButton.onClick.AddListener(Credit);
        backButton.onClick.AddListener(MainMenu);
    }

    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        creditPanel.SetActive(false);
        
        Time.timeScale = 0f; // หยุดเวลาในเกม
        playerController.enabled = false; // ปิดการควบคุมผู้เล่น
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        creditPanel.SetActive(false);
        
        Time.timeScale = 1f; // เริ่มเวลาในเกม
        playerController.enabled = true; // เปิดการควบคุมผู้เล่น
    }

    public void GameOver()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(true); // แสดง Game Over Panel
        creditPanel.SetActive(false);
        
        Time.timeScale = 0f; // หยุดเวลาในเกม
        playerController.enabled = false; // ปิดการควบคุมผู้เล่น
    }

    public void Home()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        creditPanel.SetActive(false);
        
        Time.timeScale = 1f;
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name); // รีโหลดฉากปัจจุบัน
    }

    public void Credit()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        creditPanel.SetActive(true); // แสดง Credit Panel
        
        Time.timeScale = 0f; // หยุดเวลาในเกม
        playerController.enabled = false; // ปิดการควบคุมผู้เล่น
    }
}