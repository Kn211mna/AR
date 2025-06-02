using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnScript : MonoBehaviour
{
    public int virusesPerLevel = 20;
    public int healthyPerLevel = 10;
    public float levelTime = 30f;
    public GameObject virusPrefab;
    public GameObject healthyPrefab;
    public float spawnRadius = 2.0f;
    public ARPlaneManager planeManager;
    public GameObject arCamera; 
    public GameObject bonusHealPrefab;
    public float bonusSpawnRadius = 2.5f;
    public float bonusChance = 0.1f; 

    private int virusesSpawned = 0;
    private int healthySpawned = 0;
    private float spawnInterval;

    private GameUIManager uiManager;

    void Start()
    {
        spawnInterval = levelTime / (virusesPerLevel + healthyPerLevel);
        uiManager = FindFirstObjectByType<GameUIManager>();
        if (arCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                arCamera = cam.gameObject;
        }
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (virusesSpawned < virusesPerLevel || healthySpawned < healthyPerLevel)
        {
            yield return new WaitForSeconds(spawnInterval);
            List<ARPlane> planes = new List<ARPlane>();
            foreach (var arPlane in planeManager.trackables)
            {
                planes.Add(arPlane);
            }
            if (planes.Count == 0)
                continue;

            ARPlane plane = planes[Random.Range(0, planes.Count)];
            Vector3 planeCenter = plane.center;
            Vector3 spawnPos;
            int maxTries = 10;
            int tryCount = 0;
            do
            {
                Vector2 randomInPlane = Random.insideUnitCircle * spawnRadius;
                spawnPos = planeCenter + new Vector3(randomInPlane.x, 0, randomInPlane.y);
                tryCount++;
            }
            while (arCamera != null && Vector3.Distance(spawnPos, arCamera.transform.position) < 1.0f && tryCount < maxTries);
            bool spawnVirus = false;
            if (virusesSpawned < virusesPerLevel && healthySpawned < healthyPerLevel)
                spawnVirus = Random.value > 0.5f;
            else if (virusesSpawned < virusesPerLevel)
                spawnVirus = true;

            if (spawnVirus)
            {
                Instantiate(virusPrefab, spawnPos, Quaternion.identity);
                virusesSpawned++;
            }
            else
            {
                Instantiate(healthyPrefab, spawnPos, Quaternion.identity);
                healthySpawned++;
            }
            if (Random.value < bonusChance && uiManager != null && uiManager.CanSpawnHealBonus())
            {
                Vector2 bonusOffset2D = Random.insideUnitCircle * bonusSpawnRadius;
                Vector3 bonusSpawnPos = planeCenter + new Vector3(bonusOffset2D.x, 0, bonusOffset2D.y);

                Instantiate(bonusHealPrefab, bonusSpawnPos, Quaternion.identity);
                uiManager.MarkHealBonusSpawned();
            }
        }
    }
}