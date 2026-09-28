using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public GameObject enemyMelee;
    [SerializeField] public GameObject enemyRanged;

    [Header("Spawn area")]
    [SerializeField] private float spawnMinRadius = 20f;
    [SerializeField] private float spawnMaxRadius = 50f;
    [SerializeField] private float minSpacing = 2f;
    [SerializeField] private int maxSpawnAttempts = 12;

    [Header("Wave / timing")]
    [SerializeField] private float MeleeswarmerInterval = 3f;
    [SerializeField] private float RangedswarmerInterval = 0f;
    [SerializeField] private float buffer = 0f;
    [SerializeField] private int currentLevel = 1;

    
    [SerializeField] private int meleeEnemiesToSpawn = 0;
    [SerializeField] private int rangedEnemiesToSpawn = 0;

    
    private int enemiesLeft = 0;

    private void Awake()
    {
        
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        
        if (enemyMelee == null)
            enemyMelee = Resources.Load<GameObject>("EnemyMelee");

        if (enemyRanged == null)
            enemyRanged = Resources.Load<GameObject>("EnemyRanged");

       
        enemiesLeft = 0;
    }

    void Start()
    {
        
        StartCoroutine(RunLevels());
    }


    private IEnumerator RunLevels()
    {
        yield return StartCoroutine(RunLevel(1, 10, 0));   
        yield return StartCoroutine(RunLevel(2, 25, 0));  
        yield return StartCoroutine(RunLevel(3, 50, 0));   
        // all levels cleared
        LoadScene("WinScene");
    }


    private IEnumerator RunLevel(int levelNumber, int meleeCount, int rangedCount)
    {
        currentLevel = levelNumber;
        meleeEnemiesToSpawn = meleeCount;
        rangedEnemiesToSpawn = rangedCount;

        Debug.Log($"Level {levelNumber} started: spawning {meleeCount} melee and {rangedCount} ranged.");

       
        if (meleeCount > 0)
            StartCoroutine(spawnMeleeEnemy(MeleeswarmerInterval, enemyMelee, meleeCount));

        if (rangedCount > 0)
            StartCoroutine(spawnRangedEnemy(RangedswarmerInterval, enemyRanged, rangedCount));

        
        yield return new WaitUntil(() => enemiesLeft == 0);

        Debug.Log($"Level {levelNumber} cleared.");
  
        if (buffer > 0f) yield return new WaitForSeconds(buffer);
    }

    private IEnumerator spawnMeleeEnemy(float interval, GameObject prefab, int count)
    {
        var spawnedPositions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);

            if (prefab != null)
            {
                var spawnPosition = GetNonOverlappingSpawnPosition(spawnedPositions, transform);
                spawnedPositions.Add(spawnPosition);

                var go = Instantiate(prefab, spawnPosition, Quaternion.identity);

              
                if (string.IsNullOrEmpty(go.tag) || go.tag == "Untagged")
                {
                    try { go.tag = "Enemy"; } catch {}
                }

            
                enemiesLeft++;
            }
            else
            {
                Debug.LogWarning("Cannot spawn melee enemy: prefab is null.");
            }
        }
    }

    private IEnumerator spawnRangedEnemy(float interval, GameObject prefab, int count)
    {
        var spawnedPositions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);

            if (prefab != null)
            {
                var spawnPosition = GetNonOverlappingSpawnPosition(spawnedPositions, transform);
                spawnedPositions.Add(spawnPosition);

                var go = Instantiate(prefab, spawnPosition, Quaternion.identity);

                if (string.IsNullOrEmpty(go.tag) || go.tag == "Untagged")
                {
                    try { go.tag = "Enemy"; } catch { }
                }

                enemiesLeft++;
            }
            else
            {
                Debug.LogWarning("Cannot spawn ranged enemy: prefab is null.");
            }
        }
    }

    
    public void NotifyEnemyDied()
    {
        enemiesLeft = Mathf.Max(0, enemiesLeft - 1);
    }

    
    private Vector3 GetNonOverlappingSpawnPosition(List<Vector3> existingPositions, Transform spawner)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(spawnMinRadius, spawnMaxRadius);
            float x = spawner.position.x + Mathf.Cos(angle) * radius;
            float z = spawner.position.z + Mathf.Sin(angle) * radius;
            Vector3 candidate = new Vector3(x, spawner.position.y, z);

            bool ok = true;
            for (int i = 0; i < existingPositions.Count; i++)
            {
                if (Vector3.Distance(candidate, existingPositions[i]) < minSpacing)
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
                return candidate;
        }

        float fallbackAngle = Random.Range(0f, Mathf.PI * 2f);
        float fallbackRadius = Random.Range(spawnMinRadius, spawnMaxRadius);
        return new Vector3(spawner.position.x + Mathf.Cos(fallbackAngle) * fallbackRadius, spawner.position.y, spawner.position.z + Mathf.Sin(fallbackAngle) * fallbackRadius);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
