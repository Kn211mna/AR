using System;

[Serializable]
public class PlayerStats
{
    public int[] starsPerLevel;

    public PlayerStats(int totalLevels)
    {
        starsPerLevel = new int[totalLevels];
    }
}