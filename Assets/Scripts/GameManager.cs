using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
            [SerializeField] public GameObject enemyMelee;
            [SerializeField] public GameObject enemyRanged;

            [SerializeField] private float MeleeswarmerInterval = 3f;
            [SerializeField] private float RangedswarmerInterval = 0f;
            [SerializeField] private float buffer = 0f;
            [SerializeField] private int currentLevel = 1;
            [SerializeField] private int meleeEnemiesToSpawn = 0;
            [SerializeField] private int rangedEnemiesToSpawn = 0;
             private int enemiesLeft = 0;

            public static Vector3 RandomSpawnPositionAround(Transform spawner, float width = 10f, float height = 10f)
    {
        float halfW = width * 0.5f;
        float halfH = height * 0.5f;

        float randomX = Random.Range(-halfW, halfW);
        float randomY = Random.Range(-halfH, halfH);
        float z = spawner.position.z;

        return new Vector3(spawner.position.x + randomX, spawner.position.y, spawner.position.z + randomY);

    }

            private void Awake()
            {
                
                if (enemyMelee == null)
                {
                    enemyMelee = Resources.Load<GameObject>("EnemyMelee");
            if (enemyMelee != null)
            {
                return;
            }
       
        }

        if (enemyRanged == null)
        {
            enemyRanged = Resources.Load<GameObject>("EnemyRanged");
            if (enemyRanged != null)
            {
                return;
            }
        }
        
        enemiesLeft = meleeEnemiesToSpawn + rangedEnemiesToSpawn;
    }

    void Start()
    {
        //StartCoroutine(spawnMeleeEnemy(MeleeswarmerInterval, enemyMelee));
        //StartCoroutine(spawnRangedEnemy(RangedswarmerInterval, enemyRanged));
        levelOne();
        

        if (enemiesLeft == 0)
        {
            Debug.Log("All enemies have been spawned.");
        }

    }

    private IEnumerator spawnMeleeEnemy(float interval, GameObject enemyMelee)
    {
        for (int i = 0; i < meleeEnemiesToSpawn; i++)
        {
            yield return new WaitForSeconds(interval);
            if (enemyMelee != null)
            {
                var spawnPosition = RandomSpawnPositionAround(transform, 10f, 10f);
                Instantiate(enemyMelee, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Cannot spawn melee enemy: prefab is null.");
            }
            
        }
    }

    private IEnumerator spawnRangedEnemy(float interval, GameObject enemyRanged)
    {

        for (int i = 0; i < rangedEnemiesToSpawn; i++)
        {
            yield return new WaitForSeconds(0);
            if (enemyRanged != null)
            {
                Instantiate(enemyRanged, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Cannot spawn ranged enemy: prefab is null.");
            }
        }
    }

    void levelOne()
    {
        meleeEnemiesToSpawn = 10;
        
        StartCoroutine(spawnMeleeEnemy(MeleeswarmerInterval, enemyMelee));
        StartCoroutine(spawnRangedEnemy(RangedswarmerInterval, enemyRanged));


    }

    void LevelTwo()
    {

    }

    void LevelThree()
    {

    }

    void LevelFour()
    {

    }

    void LevelFive()
    {

    }
    private void OnLevelWasLoaded(int level)
    {

        
    }
}
