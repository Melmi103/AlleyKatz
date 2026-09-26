using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
            [SerializeField] private GameObject enemyMelee;
            [SerializeField] private GameObject enemyRanged;

            [SerializeField] private float MeleeswarmerInterval = 3.5f;
            [SerializeField] private float RangedswarmerInterval = 5.0f;
            [SerializeField] private float buffer = 1.5f;

            private void Awake()
            {
                
                if (enemyMelee == null)
                {
                    enemyMelee = Resources.Load<GameObject>("EnemyMelee");
            if (enemyMelee == null)
            {
                return;
            }
       
        }

        if (enemyRanged == null)
        {
            enemyRanged = Resources.Load<GameObject>("EnemyRanged");
            if (enemyRanged == null)
            {
                return;
            }
        }
    }

    void Start()
    {
       StartCoroutine(spawnMeleeEnemy(MeleeswarmerInterval, enemyMelee));
       StartCoroutine(spawnRangedEnemy(RangedswarmerInterval, enemyRanged));
    }

    private IEnumerator spawnMeleeEnemy(float interval, GameObject enemyMelee)
    {
        yield return new WaitForSeconds(interval);
        if (enemyMelee != null)
        {
            Instantiate(enemyMelee, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Cannot spawn melee enemy: prefab is null.");
        }
        StartCoroutine(spawnMeleeEnemy(interval, enemyMelee));
    }

    private IEnumerator spawnRangedEnemy(float interval, GameObject enemyRanged)
    {
        yield return new WaitForSeconds(interval);
        if (enemyRanged != null)
        {
            Instantiate(enemyRanged, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Cannot spawn ranged enemy: prefab is null.");
        }
        StartCoroutine(spawnRangedEnemy(interval, enemyRanged));
    }
}
