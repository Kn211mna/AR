using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgressData", menuName = "Scriptable Objects/LevelProgressData")]
public class LevelProgressData : ScriptableObject
{
    public int[] starsPerLevel;

    public void Init(int totalLevels)
    {
        if (starsPerLevel == null || starsPerLevel.Length != totalLevels)
            starsPerLevel = new int[totalLevels];
    }
    public void SetStars(int levelIndex, int stars)
    {
        if (levelIndex >= 0 && levelIndex < starsPerLevel.Length)
        {
            if (starsPerLevel[levelIndex] < stars)
                starsPerLevel[levelIndex] = stars;
            string key = $"LevelStars_{levelIndex}";
            PlayerPrefs.SetInt(key, starsPerLevel[levelIndex]);
            PlayerPrefs.Save();
        }
    }
    public int GetStars(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < starsPerLevel.Length)
            return starsPerLevel[levelIndex];
        return 0;
    }
}


