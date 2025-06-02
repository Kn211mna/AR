using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject statsPanel;
    public Text statsText;
    public int totalLevels = 3;
    public LevelProgressData levelProgressData; 
    public GameObject continueButton; 

    void Start()
    {
        ShowMainMenu();
        if (levelProgressData != null)
            levelProgressData.Init(totalLevels);
        if (continueButton != null)
        {
            bool hasProgress = false;
            if (levelProgressData != null && levelProgressData.starsPerLevel != null)
            {
                foreach (var stars in levelProgressData.starsPerLevel)
                {
                    if (stars > 0)
                    {
                        hasProgress = true;
                        break;
                    }
                }
            }
            continueButton.SetActive(hasProgress);
        }
    }
    public void OnStartGame()
    {
        ResetProgressInPlayerPrefs();
        if (levelProgressData != null)
            levelProgressData.Init(totalLevels); 
        SceneManager.LoadScene(1); 
    }
    public void OnContinueGame()
    {
        if (levelProgressData == null || levelProgressData.starsPerLevel == null)
        {
            SceneManager.LoadScene(1);
            return;
        }

        int nextLevel = 0;
        bool allCompleted = true;
        for (int i = 0; i < levelProgressData.starsPerLevel.Length; i++)
        {
            if (levelProgressData.starsPerLevel[i] == 0)
            {
                nextLevel = i;
                allCompleted = false;
                break;
            }
        }
        if (allCompleted)
            nextLevel = levelProgressData.starsPerLevel.Length - 1;

        SceneManager.LoadScene(nextLevel + 1); 
    }
    private void ResetProgressInPlayerPrefs()
    {
        if (levelProgressData != null && levelProgressData.starsPerLevel != null)
        {
            for (int i = 0; i < levelProgressData.starsPerLevel.Length; i++)
            {
                string key = $"LevelStars_{i}";
                PlayerPrefs.DeleteKey(key);
            }
            PlayerPrefs.Save();
        }
    }
    public void OnShowStats()
    {
        mainMenuPanel.SetActive(false);
        statsPanel.SetActive(true);
        ShowStats();
    }

    public void OnBackToMenu()
    {
        statsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        statsPanel.SetActive(false);
    }
    private void ShowStats()
    {
        if (statsText == null || levelProgressData == null)
        {
            statsText.text = "Статистика відсутня";
            return;
        }
        statsText.text = "";
        for (int i = 0; i < levelProgressData.starsPerLevel.Length; i++)
        {
            int stars = levelProgressData.starsPerLevel[i];
            statsText.text += $"Рівень {i + 1}: ";
            for (int s = 0; s < stars; s++) statsText.text += "★";
            for (int s = stars; s < 3; s++) statsText.text += "☆";
            statsText.text += "\n";
        }
    }
}