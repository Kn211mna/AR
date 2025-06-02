using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject gamePanel;
    public Image[] hearts;
    public Image[] stars;
    public int maxLives = 3;
    public int playerLives = 3;
    public float levelTime = 30f;
    public Text timerText;
    private float timeLeft;
    private bool timerActive = true;
    public int totalLevels = 3;
    public int firstLevelBuildIndex = 1; 
    public LevelProgressData levelProgressData; 
    private bool healBonusSpawned = false;
    public GameObject finalWinPanel; 
    public Image[] finalStars;       
    void Awake()
    {
        if (levelProgressData != null)
            levelProgressData.Init(totalLevels);
    }

    void Start()
    {
        if (gamePanel != null) gamePanel.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (finalWinPanel != null) finalWinPanel.SetActive(false);
        playerLives = maxLives;
        timeLeft = levelTime;
        timerActive = true;
        Time.timeScale = 1f;
        healBonusSpawned = false;
        UpdateHeartsUI();
    }

    void Update()
    {
        if (timerActive)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI();
            if (timeLeft <= 0)
            {
                timerActive = false;
                CompleteLevel(CalculateStars());
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Залишилося: " + Mathf.CeilToInt(timeLeft);
    }

    public void OnPause()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
        Time.timeScale = 0f;
        timerActive = false;
    }

    public void OnContinueGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
        Time.timeScale = 1f;
        timerActive = true;
    }

    public void OnReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void OnFinalReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnLoseReplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].gameObject.SetActive(i < playerLives);
    }
    void ShowStars(int starsCount)
    {
        for (int i = 0; i < stars.Length; i++)
            stars[i].gameObject.SetActive(i < starsCount);
    }
    void ShowFinalStars(int starsCount)
    {
        if (finalStars == null) return;
        for (int i = 0; i < finalStars.Length; i++)
            finalStars[i].gameObject.SetActive(i < starsCount);
    }
    public void DecreaseLife()
    {
        playerLives--;
        UpdateHeartsUI();
        if (playerLives <= 0)
            ShowLosePanel();
    }
    public void OnNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene(0); 
    }
    public void AddLife()
    {
        if (playerLives < maxLives)
        {
            playerLives++;
            UpdateHeartsUI();
        }
    }
    public void CompleteLevel(int starsEarned)
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex - firstLevelBuildIndex;
        if (levelProgressData != null)
            levelProgressData.SetStars(currentLevel, starsEarned);
        ShowWinPanel(starsEarned);
    }

    public void ShowLosePanel()
    {
        if (gamePanel != null) gamePanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (finalWinPanel != null) finalWinPanel.SetActive(false);
        timerActive = false;
        Time.timeScale = 0f;
    }

    public void ShowWinPanel(int starsEarned)
    {
        if (gamePanel != null) gamePanel.SetActive(false);

        int currentLevel = SceneManager.GetActiveScene().buildIndex - firstLevelBuildIndex;
        bool isLastLevel = (currentLevel == totalLevels - 1);

        if (isLastLevel && finalWinPanel != null)
        {
            if (winPanel != null) winPanel.SetActive(false);
            finalWinPanel.SetActive(true);
            ShowFinalStars(starsEarned);
        }
        else
        {
            if (winPanel != null) winPanel.SetActive(true);
            ShowStars(starsEarned);
            if (finalWinPanel != null) finalWinPanel.SetActive(false);
        }

        timerActive = false;
        Time.timeScale = 0f;
    }
    int CalculateStars()
    {
        if (playerLives == maxLives)
            return 3;
        if (playerLives == maxLives - 1)
            return 2;
        return 1;
    }
    public bool CanSpawnHealBonus()
    {
        return playerLives < maxLives && !healBonusSpawned;
    }

    public void MarkHealBonusSpawned()
    {
        healBonusSpawned = true;
    }
}